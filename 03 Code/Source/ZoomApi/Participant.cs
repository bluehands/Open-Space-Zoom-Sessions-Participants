using System;
using Newtonsoft.Json;

namespace ZoomApi
{
    public class Participant
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }
        [JsonProperty("user_name")]
        public string UserName { get; set; }
        [JsonProperty("join_time")]
        public DateTime JoinTime { get; set; }
        [JsonProperty("leave_time")]
        public DateTime LeaveTime { get; set; }
    }
}