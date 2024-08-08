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
        public SQLService SqlService { get; set; }
        public SourceController(FireBaseService fireBaseService, SQLService sQLService)
        {
            this.FireBaseService = fireBaseService;
            this.SqlService = sQLService;
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
        [HttpGet]
        //public async Task<IActionResult> Set([FromBody] Source src)
        public async Task<IActionResult> Set(string code, string quantity)
        {
            PayLoad payload = new PayLoad();
            Source src = new Source();
            src.Code = code;
            src.Quantity = quantity;
            bool done = false;
            if (!string.IsNullOrEmpty(src.Quantity))
            {
                payload = await FireBaseService.DestinationUpdate(src);
            }
            if (payload.updated)
            {
                await SqlService.InsertDestinationData(payload.Destination);
            }
            return Ok(new JsonResult("Update: " + done));
        }
    }
}
