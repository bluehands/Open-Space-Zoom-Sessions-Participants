using Newtonsoft.Json;

namespace ZoomApi
{
    public class Participant
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }
        [JsonProperty("user_name")]
        public string UserName { get; set; }
    }
}