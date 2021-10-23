using EnOutstandingMeetings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FnOutstandingMeetings
{
    public static class FnOrchestrator
    {
        [FunctionName(nameof(Process))]
        public static async Task<IActionResult> Process(
          [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
          [DurableClient] IDurableClient client,
          ILogger log)
        {
            var res = FnUtil.GetProcessingRequest(req);

            switch (res.ProcessingType)
            {
                case ProcessingType.CreateGroup:
                    {

                        await client.StartNewAsync(nameof(FnGroup.CreateGroup), res);
                        break;
                    }
                case ProcessingType.JoinGroup:
                    {

                        await client.StartNewAsync(nameof(FnGroup.JoinGroup), res);
                        break;
                    }
            }

            return (ActionResult)new OkObjectResult(true);
        }
    }
}