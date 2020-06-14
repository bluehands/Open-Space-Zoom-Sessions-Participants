using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SessionParticipants.Domain;

namespace SessionParticipants.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SessionsController : ControllerBase
    {
        private readonly ILogger<SessionsController> logger;
        private readonly int cacheAgeInSeconds;
        private readonly ISessionRepository sessionRepository;

        public SessionsController(ILogger<SessionsController> logger, IOptions<GeneralSettings> generalSettings, ISessionRepository sessionRepository)
        {
            this.logger = logger;
            cacheAgeInSeconds = generalSettings.Value?.CacheAgeInSeconds ?? 30;
            if (cacheAgeInSeconds < 10)
            {
                cacheAgeInSeconds = 30;
            }
            this.sessionRepository = sessionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var sessions = await sessionRepository.GetSessionsAsync(TimeSpan.FromSeconds(cacheAgeInSeconds));
            return Ok(sessions);
        }
    }
}
