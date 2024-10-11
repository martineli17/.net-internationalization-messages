using Api.Resources.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class MessageController : ControllerBase
    {
        [HttpGet("messages:success")]
        public IActionResult Success()
        {
            return Ok(new { Value = ResourceMessages.Success });
        }

        [HttpGet("messages:error")]
        public IActionResult Error()
        {
            return Ok(new { Value = ResourceMessages.Error });
        }

        [HttpGet("messages:culture")]
        public IActionResult Culture()
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;
            return Ok(new { Value = currentCulture });
        }
    }
}
