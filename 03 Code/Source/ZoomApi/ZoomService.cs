using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace ZoomApi
{
    public class ZoomService
    {
        private readonly TokenGenerator tokenGenerator;
        private string token;
        private const string ZoomApiBaseUrl = "https://api.zoom.us/v2";

        public ZoomService(string apiKey, string secret)
        {
            tokenGenerator = new TokenGenerator(apiKey, secret);
            token = tokenGenerator.Generate();

        }

        public async Task<List<User>> ListUsers()
        {
            var token = GetToken();
            var listResult = await ZoomApiBaseUrl
                .AppendPathSegment("users")
                .SetQueryParams(new { page_size = 100 })
                .WithOAuthBearerToken(token)
                .GetJsonAsync<ListUserResult>();
            return listResult.Users.ToList();
        }


        public async Task<List<Meeting>> ListMeetings(string userId, MeetingState state)
        {
            var token = GetToken();
            var listResult = await ZoomApiBaseUrl
                .AppendPathSegments("users", userId, "meetings")
                .SetQueryParams(new { page_size = 50, type = state.ToString().ToLower() })
                .WithOAuthBearerToken(token)
                .GetJsonAsync<ListMeetingsResult>();
            return listResult.Meetings.ToList();
        }

        public async Task<List<Participant>> ListParticipants(string meetingId)
        {
            var token = GetToken();
            var listResult = await ZoomApiBaseUrl
                .AppendPathSegments("metrics", "meetings", meetingId, "participants")
                .SetQueryParams(new { page_size = 50 })
                .WithOAuthBearerToken(token)
                .GetJsonAsync<ListParticipantsResult>();
            return listResult.Participants.Where(p=>p.LeaveTime<=p.JoinTime).ToList();
        }

        private string GetToken()
        {
            var currentToken = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(token);
            
            if (currentToken.ValidTo - DateTime.UtcNow<TimeSpan.FromSeconds(10))
            {
                token = tokenGenerator.Generate();
            }
            if (DateTime.UtcNow - currentToken.ValidFrom < TimeSpan.FromSeconds(1))
            {
                Task.Delay(1000);
            }
            return token;
        }

    }

    public enum MeetingState
    {
        Scheduled,
        Live,
        Upcoming
    }
}