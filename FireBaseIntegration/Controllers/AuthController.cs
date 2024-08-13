using FireBaseIntegration.DTO;
using FireBaseIntegration.Service;
using Microsoft.AspNetCore.Mvc;

namespace FireBaseIntegration.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        public FireBaseService FireBaseService { get; set; }
        public SQLService sqlsvc { get; set; }
        public AuthController(FireBaseService fireBaseService, SQLService sqlsvc)
        {
            this.FireBaseService = fireBaseService;
            this.sqlsvc = sqlsvc;
        }
        public HttpResponseMessage Auth(string username, string password)
        {

            HttpResponseMessage orp = new HttpResponseMessage();
            return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
        }
        [HttpGet]
        public async Task<IActionResult> SignIn(string user, string pass)
        {
            //Destination destination = new Destination();
            //await sqlsvc.InsertDestinationData(destination);
            bool result = false;
            if (!string.IsNullOrEmpty(user) || !string.IsNullOrEmpty(pass))
            {
                result = await FireBaseService.Users(user, pass);
            }
            if (result)
            {
                var response = new
                {
                    Message = $"Welcome {user}",
                    status = true,
                    model = ""
                };

                return new ObjectResult(response)
                {
                };
            }
            else
            {
                var response = new
                {
                    Message = "User doesn't exists",
                    status = false,
                    model = ""
                };

                return new ObjectResult(response)
                {
                };
            }
        }
    }
}
