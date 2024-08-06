using FireBaseIntegration.Service;
using Microsoft.AspNetCore.Mvc;

namespace FireBaseIntegration.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SourceController : ControllerBase
    {
        public FireBaseService FireBaseService { get; set; }
        public SourceController(FireBaseService fireBaseService)
        {
            this.FireBaseService = fireBaseService;
        }

        [HttpGet]
        public async Task<IActionResult> Id(string user, string pass)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(user) || !string.IsNullOrEmpty(pass))
            {
                result = await FireBaseService.Users(user, pass);
            }
            if (result)
            {
                return new JsonResult($"Welcome {user}");
            }
            else
            {
                return new JsonResult($"User {user} credentials doesn't match");
            }
        }
    }
}
