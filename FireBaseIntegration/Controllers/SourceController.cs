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
                Message = payload.exc,
                status = payload.status,
                model = payload.source
            };
            return new ObjectResult(response)
            {
            };

        }
        [HttpPost]
        //public async Task<IActionResult> Set([FromBody] Source src)
        public async Task<IActionResult> Set(List<SourceSetPayload> srclst)
        {
            PayLoad payload = new PayLoad();
            SQLPayload sQLPayload = new SQLPayload();
            //Source src = new Source();
            //src.Code = code;
            //src.Quantity = quantity;
            bool done = false;
            string su = "";
            if (srclst.Count > 0)
            {
                payload = await FireBaseService.DestinationUpdate(srclst);
            }
            if (payload.updated)
            {
                foreach (var item in payload.Destination)
                {

                    sQLPayload = await SqlService.InsertDestinationData(item);
                }

                if (sQLPayload.status)
                {
                    var response = new
                    {
                        Message = "Record Updated in Firebase and Inserted in SQL",
                        status = true,
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
                        Message = $"Record Updated in Firebase but Insertion in SQL Failed. Exception : {sQLPayload.sqlissue}",
                        status = true,
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
                    Message = $"Record Updation Failed in Firebase. Issue: {payload.exc}",
                    status = false,
                    model = payload.Destination
                };

                return new ObjectResult(response)
                {
                };

            }
        }
    }
}
