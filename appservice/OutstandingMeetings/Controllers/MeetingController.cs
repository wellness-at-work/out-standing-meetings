using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using EnOutstandingMeetings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace OutstandingMeetings.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeetingController : ControllerBase
    {
        IConfiguration _configuration;
        [HttpGet("Participants")]
        public async Task<MeetingParticipantReponse> GetParticipants(string orgCode)
        {

            var statUrl = this._configuration["StatUrl"];

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("GroupId", orgCode);
                var response = await client.GetStringAsync(statUrl);
                var data = JsonConvert.DeserializeObject<MeetingParticipantReponse>(response);

                if (data.AllTimeRecord != null)
                {
                    return data;
                }
            }



            var attendantList = new List<MeetingParticipant>();
            var allTimeRecord = new List<MeetingParticipant>();
            Random rnd = new Random();
            var i = 0;
            for (i = 0; i < 15; i++)
            {
                attendantList.Add(new MeetingParticipant
                {
                    Duration = rnd.Next(5500, 100000),
                    Name = $" Person #{i}",
                    Id = Guid.NewGuid().ToString()
                });
            }



            allTimeRecord.Add(new MeetingParticipant
            {
                Duration = 103000,
                Name = $"Dorothy Vaughan",
                Id = Guid.NewGuid().ToString()
            });

            allTimeRecord.Add(new MeetingParticipant
            {
                Duration = 101000,
                Name = $"Steve Wozniak",
                Id = Guid.NewGuid().ToString()
            });

            allTimeRecord.Add(new MeetingParticipant
            {
                Duration = 100000,
                Name = $"Nikolay Yegorovich Zhukovsky",
                Id = Guid.NewGuid().ToString()
            });

            return new MeetingParticipantReponse { AllTimeRecord = allTimeRecord, Participants = attendantList };
        }

        public MeetingController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
    }
}
