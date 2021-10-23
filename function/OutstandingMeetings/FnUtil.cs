using EnOutstandingMeetings;
using Microsoft.AspNetCore.Http;

namespace FnOutstandingMeetings
{
    public static class FnUtil
    {
        public static ProcessingRequest GetProcessingRequest(HttpRequest req)
        {
            return new ProcessingRequest()
            {
                ProcessingType = (ProcessingType) int.Parse(req.Form["ProcessingType"]),
                Payload = req.Form["Payload"]
            };
        }
    }
}
