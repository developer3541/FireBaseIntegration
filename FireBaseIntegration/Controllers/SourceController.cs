using FireBaseIntegration.DTO;
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
        public async Task<IActionResult> Get(string barcode)
        {
            Source src = new Source();
            if (!string.IsNullOrEmpty(barcode))
            {
                src = await FireBaseService.SourceRetrival(barcode);
            }
            return Ok(src);
        }
        [HttpPost]
        //public async Task<IActionResult> Set([FromBody] Source src)
        public async Task<IActionResult> Set(string code, string quantity)
        {
            Source src = new Source();
            bool done = false;
            if (!string.IsNullOrEmpty(src.Quantity))
            {
                done = await FireBaseService.DestinationUpdate(src);
            }
            return Ok(new JsonResult("Update"));
        }
    }
}
