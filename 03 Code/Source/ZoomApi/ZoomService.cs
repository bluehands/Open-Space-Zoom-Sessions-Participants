using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace ZoomApi
{
    public class ZoomService
    {
        private readonly TokenGenerator tokenGenerator;
        private const string ZoomApiBaseUrl = "https://api.zoom.us/v2";

        public ZoomService(string apiKey, string secret)
        {
            tokenGenerator = new TokenGenerator(apiKey, secret);
            var token = tokenGenerator.Generate();
        }

        public async Task<List<User>> ListUsers()
        {
            var listResult = await ZoomApiBaseUrl
                .AppendPathSegment("users")
                .SetQueryParams(new { page_size = 100 })
                .WithOAuthBearerToken(tokenGenerator.Generate())
                .GetJsonAsync<ListUserResult>();
            return listResult.Users.ToList();
        }
        public async Task<List<Meeting>> ListMeetings(string userId, MeetingState state)
        {
            var listResult = await ZoomApiBaseUrl
                .AppendPathSegments("users", userId, "meetings")
                .SetQueryParams(new { page_size = 100, type = state.ToString().ToLower() })
                .WithOAuthBearerToken(tokenGenerator.Generate())
                .GetJsonAsync<ListMeetingsResult>();
            return listResult.Meetings.ToList();
        }

        public async Task<List<Participant>> ListParticipants(string meetingId)
        {
            var listResult = await ZoomApiBaseUrl
                .AppendPathSegments("metrics", "meetings", meetingId, "participants")
                .SetQueryParams(new { page_size = 100 })
                .WithOAuthBearerToken(tokenGenerator.Generate())
                .GetJsonAsync<ListParticipantsResult>();
            return listResult.Participants.ToList();

        }

    }

    public enum MeetingState
    {
        Scheduled,
        Live,
        Upcoming
    }
}