using FireBaseIntegration.Service;
using Microsoft.AspNetCore.Mvc;

namespace FireBaseIntegration.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SigninController : ControllerBase
    {
        public FireBaseService FireBaseService { get; set; }
        public SigninController(FireBaseService fireBaseService)
        {
            this.FireBaseService = fireBaseService;
        }
        public HttpResponseMessage Auth(string username, string password)
        {
            HttpResponseMessage orp = new HttpResponseMessage();
            return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
        }
        [HttpGet]
        public async Task SignIn()
        {
            await FireBaseService.Users();
            //return new JsonResult("Hello");
        }
    }
}
