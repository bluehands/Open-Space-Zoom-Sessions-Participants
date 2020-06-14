using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SessionParticipants.Domain
{
    public interface ISessionRepository
    {
        Task<List<Session>> GetSessionsAsync(TimeSpan cacheAge);
        Task UpdateSessionParticipantHasJoinedAsync(string sessionId, Participant participant);
        Task UpdateSessionParticipantHasLeftAsync(string sessionId, Participant participant);
    }
}