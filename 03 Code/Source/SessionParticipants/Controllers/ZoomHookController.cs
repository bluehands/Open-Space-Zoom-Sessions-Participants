using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SessionParticipants.Domain;
using Bluehands.Diagnostics.LogExtensions;
// ReSharper disable InconsistentNaming

namespace SessionParticipants.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ZoomHookController : ControllerBase
    {
        private readonly ILogger<SessionsController> logger;
        private readonly IOptions<ZoomCredentials> zoomCredentials;
        private readonly ISessionRepository sessionRepository;

        public ZoomHookController(ILogger<SessionsController> logger, IOptions<ZoomCredentials> zoomCredentials, ISessionRepository sessionRepository)
        {
            this.logger = logger;
            this.zoomCredentials = zoomCredentials;
            this.sessionRepository = sessionRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]HookData body)
        {
            //ToDO Check Verification code
            using (logger.AutoTrace(() => $"Id: {body?.payload?.@object?.participant?.user_id}; Meeting: {body?.payload?.@object?.id}; Event: {body?.@event}"))
            {
                try
                {
                    var participant = new Participant { Id = body.payload.@object.participant.user_id, Name = body.payload.@object.participant.user_name };
                    if (body.@event.Equals("meeting.participant_joined"))
                    {
                        await sessionRepository.UpdateSessionParticipantHasJoinedAsync(body.payload.@object.id, participant);
                    }

                    if (body.@event.Equals("meeting.participant_left"))
                    {
                        await sessionRepository.UpdateSessionParticipantHasLeftAsync(body.payload.@object.id, participant);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(() => "Unexpected error", ex);
                }

                return Ok();
            }
        }
    }
    public class HookData
    {

        public string @event { get; set; }
        public Payload payload { get; set; }
    }

    public class Payload
    {
        public string account_id { get; set; }

        public PayloadData @object { get; set; }
    }

    public class PayloadData
    {
        public string uuid { get; set; }
        public string id { get; set; }
        public string host_id { get; set; }
        public string topic { get; set; }
        public int type { get; set; }
        public string start_time { get; set; }
        public int duration { get; set; }
        public string timezone { get; set; }
        public ParticipantPayloadData participant { get; set; }
    }

    public class ParticipantPayloadData
    {
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string id { get; set; }
        public string join_time { get; set; }
    }
}