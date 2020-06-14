using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace SessionParticipants
{
    public class ConfiguredMeetings
    {
        public List<ConfiguredMeeting> Meetings { get; }

        public ConfiguredMeetings(IConfiguration configuration)
        {
            var settings = configuration.GetSection("Meetings");
            var rawSettings = settings.AsEnumerable();
            Meetings = rawSettings.Where(kv => !string.IsNullOrWhiteSpace(kv.Value)).Select(kv =>
           {
               try
               {
                   return new ConfiguredMeeting
                   {
                       OrderNr = int.Parse(kv.Key.Split('.')[1]),
                       MeetingId = kv.Value
                   };
               }
               catch (Exception)
               {
                   return null;
               }
           }).Where(m => m != null).ToList();
            Meetings.Sort(new MeetingOrderComparer());
        }
    }
    public class ConfiguredMeeting
    {
        public int OrderNr { get; set; }
        public string MeetingId { get; set; }
    }
    public class MeetingOrderComparer : IComparer<ConfiguredMeeting>
    {
        public int Compare(ConfiguredMeeting x, ConfiguredMeeting y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null && y != null)
            {
                return -1;
            }
            if (x != null && y == null)
            {
                return 1;
            }

            return x.OrderNr.CompareTo(y.OrderNr);
        }
    }
}