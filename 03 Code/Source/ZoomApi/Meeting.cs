using System;
using Newtonsoft.Json;

namespace ZoomApi
{
    public class Meeting
    {
        public string Id { get; set; }
        [JsonProperty("host_id")]
        public string HostId { get; set; }
        [JsonProperty("topic")]
        public string Topic { get; set; }
        [JsonProperty("start_time")]
        public DateTime StartTime { get; set; }
        [JsonProperty("join_url")]
        public Uri JoinUrl { get; set; }
    }
}