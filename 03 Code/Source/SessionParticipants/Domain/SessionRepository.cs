﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Bluehands.Diagnostics.LogExtensions;
using ZoomApi;

namespace SessionParticipants.Domain
{
    public class SessionRepository : ISessionRepository
    {
        private readonly ILogger<SessionRepository> logger;
        private readonly ConfiguredMeetings configuredMeetings;
        private readonly ZoomService zoomService;
        private List<Session> sessions;
        private DateTime lastCacheUpdate = DateTime.MinValue;

        public SessionRepository(ILogger<SessionRepository> logger, IOptions<ZoomCredentials> zoomCredentials, ConfiguredMeetings configuredMeetings)
        {
            this.logger = logger;
            this.configuredMeetings = configuredMeetings;
            zoomService = new ZoomService(zoomCredentials.Value.ApiKey, zoomCredentials.Value.Secret);
        }
        public async Task<List<Session>> GetSessionsAsync(TimeSpan cacheAge)
        {
            using (logger.AutoTrace())
            {
                try
                {
                    if (sessions == null)
                    {
                        sessions = await LoadConfiguredSession();
                    }

                    if (!IsCacheValid(cacheAge))
                    {
                        foreach (var session in sessions)
                        {
                            var participants = await zoomService.ListParticipants(session.Id).ConfigureAwait(false);
                            session.Participants.Clear();
                            session.Participants.AddRange(participants.Select(p => new Participant
                            {
                                Id = p.UserId,
                                Name = p.UserName
                            }));
                        }

                        lastCacheUpdate = DateTime.UtcNow;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(() => "Unexpeted error", ex);
                    throw;
                }

                return new List<Session>(sessions);
            }
        }

        private bool IsCacheValid(in TimeSpan cacheAge)
        {
            if (lastCacheUpdate == DateTime.MinValue)
            {
                return false;
            }
            if (DateTime.UtcNow - lastCacheUpdate > cacheAge)
            {
                return false;
            }
            return true;
        }

        private async Task<List<Session>> LoadConfiguredSession()
        {
            using (logger.AutoTrace())
            {
                var sessionsToLoad = new List<Session>();
                var allMeetings = new Dictionary<string, Meeting>();
                var users = await zoomService.ListUsers().ConfigureAwait(false);
                foreach (var user in users)
                {
                    var meetings = await zoomService.ListMeetings(user.Id, MeetingState.Scheduled).ConfigureAwait(false);
                    foreach (var meeting in meetings)
                    {
                        allMeetings[meeting.Id] = meeting;
                    }
                }

                foreach (var configuredMeeting in configuredMeetings.Meetings)
                {
                    if (allMeetings.TryGetValue(configuredMeeting.MeetingId, out Meeting meeting))
                    {
                        sessionsToLoad.Add(new Session
                        {
                            Id = meeting.Id,
                            Title = meeting.Topic,
                            ParticipationUrl = meeting.JoinUrl,
                            SortOrder = configuredMeeting.OrderNr
                        });
                    }
                    else
                    {
                        logger.LogError(() => $"Configured meeting with id {configuredMeeting.MeetingId} not found in current zoom meetings");
                    }
                }

                return sessionsToLoad;
            }
        }
    }
}