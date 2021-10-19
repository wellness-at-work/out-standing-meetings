using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OutstandingMeetings.Data;

namespace OutstandingMeetings.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeetingController : ControllerBase
    {
        [HttpGet("Participants")]
        public MeetingParticipantReponse GetParticipants(string orgCode)
        {
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
    }
}
