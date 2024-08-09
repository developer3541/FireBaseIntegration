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
            SourcePayload payload = new SourcePayload();
            if (!string.IsNullOrEmpty(barcode))
            {
                payload = await FireBaseService.SourceRetrival(barcode);
            }
            var response = new
            {
                Message = "",
                status = $"{payload.status}",
                model = payload.source
            };

            return new ObjectResult(response)
            {
            };
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
            string su = "";
            if (!string.IsNullOrEmpty(src.Quantity))
            {
                payload = await FireBaseService.DestinationUpdate(src);
            }
            if (payload.updated)
            {
                done = await SqlService.InsertDestinationData(payload.Destination);

                if (done)
                {
                    var response = new
                    {
                        Message = "Record Updated in Firebase and Inserted in SQL",
                        status = "true",
                        model = payload.Destination
                    };

                    return new ObjectResult(response)
                    {
                    };
                }
                else
                {
                    su = "failed";
                    var response = new
                    {
                        Message = "Record Updated in Firebase but Insertion in SQL Failed",
                        status = "true",
                        model = payload.Destination
                    };

                    return new ObjectResult(response)
                    {
                    };
                }
            }
            else
            {
                su = "failed";
                var response = new
                {
                    Message = "Record Updation Failed in Firebase",
                    status = "false",
                    model = payload.Destination
                };

                return new ObjectResult(response)
                {
                };

            }
        }
    }
}
