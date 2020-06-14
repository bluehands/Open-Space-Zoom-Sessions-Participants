using System;
using System.Collections.Generic;

namespace SessionParticipants.Domain
{
    public class Session
    {
        public Session()
        {
            Participants = new List<Participant>();
        }
        public string Title { get; set; }
        public string Id { get; set; }
        public Uri ParticipationUrl { get; set; }
        public int SortOrder { get; set; }
        public List<Participant> Participants { get; set; }
    }
}