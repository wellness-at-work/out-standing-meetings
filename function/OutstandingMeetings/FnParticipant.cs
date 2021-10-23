using EnOutstandingMeetings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FnOutstandingMeetings
{
    public static class FnParticipant
    {
        [FunctionName(nameof(UpdateStatus))]
        public static async Task<IActionResult> UpdateStatus(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
           [Table(nameof(MeetingGroup))] CloudTable meetingGroupTable,
            ILogger log)
        {
            var participantId = req.Headers["ParticipantId"];
            var groupId = req.Headers["GroupId"];
            var status = int.Parse(req.Headers["Status"]);

            var groupClient = new DataAccess<MeetingGroup>(meetingGroupTable);
            var response = new MeetingParticipantReponse();

            var dbGroup = await groupClient.GetAsync(nameof(MeetingGroup), groupId);

            if (dbGroup != null)
            {

                var participants = string.IsNullOrEmpty(dbGroup.ParticipantsSerialized) ? new List<MeetingParticipant>() :
                    JsonConvert.DeserializeObject<List<MeetingParticipant>>(dbGroup.ParticipantsSerialized);
                var participant = participants.Where(p => p.Id == participantId).FirstOrDefault();

                if (participant == null)
                {
                    var newparticipant = new MeetingParticipant
                    {
                        Id = participantId,
                        GroupId = groupId,
                        Activity = new List<ParticipantActivity>
                        {
                            new ParticipantActivity
                            {
                                EpochTimeStamp = ConvertFromUnixTimestamp(DateTime.Now),
                                Status = (StandingStatus) status
                            }
                        }
                    };
                    participants.Add(newparticipant);
                }
                else
                {
                    if(participant.Activity == null)
                    {
                        participant.Activity = new List<ParticipantActivity>();
                    }

                    participant.Activity.Add(
                        new ParticipantActivity
                        {
                            EpochTimeStamp = ConvertFromUnixTimestamp(DateTime.Now),
                            Status = (StandingStatus)status
                        });
                }
               
                dbGroup.ParticipantsSerialized = JsonConvert.SerializeObject(participants);
                await groupClient.ReplaceAsync(dbGroup);
            }
            return (ActionResult)new OkObjectResult(response);
        }

        [FunctionName(nameof(GetStats))]
        public static async Task<IActionResult> GetStats(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
           [Table(nameof(MeetingGroup))] CloudTable meetingGroupTable,
            ILogger log)
        {
            var groupCode = req.Headers["GroupId"];
            var groupClient = new DataAccess<MeetingGroup>(meetingGroupTable);
            var response = new MeetingParticipantReponse();

            var dbGroup = await groupClient.GetAsync(nameof(MeetingGroup), groupCode);

            if (dbGroup != null)
            {
                var participants = JsonConvert.DeserializeObject<List<MeetingParticipant>>(dbGroup.ParticipantsSerialized);
                response = SortedParticipants(participants);
            }
            return (ActionResult)new OkObjectResult(response);
        }

        private static MeetingParticipantReponse SortedParticipants(List<MeetingParticipant> participants)
        {
            var currentDate = ConvertFromUnixTimestamp(DateTime.Today);
            participants = participants.Where(p => p != null).ToList();
            participants = participants.Where(p => p.Activity != null && p.Name != null).ToList();

            var todaysParticipants = (from p in participants
                                      from a in p.Activity
                                      where a.EpochTimeStamp >= currentDate
                                      select p)
                                     .Distinct()
                                     .ToList();

            return new MeetingParticipantReponse
            {
                AllTimeRecord = Sorted(participants).Distinct().Take(3).ToList(),
                Participants = Sorted(todaysParticipants).Distinct().ToList()
            };
        }

        private static List<MeetingParticipant> Sorted(List<MeetingParticipant> participants)
        {
            foreach (var p in participants)
            {
                p.Duration = CalculateDuration(p.Activity);
            }

            return participants.OrderByDescending(p => p.Duration)
                .ThenBy(p => p.Name)
                .ToList();
        }

        private static double CalculateDuration(List<ParticipantActivity> activity)
        {
            double totalDuration = 0;
            // Count duration for person standing up
            // Standing up is defined as activity change from standing to sitting
            for (var i = 0; i < activity.Count - 1; i++)
            {
                var currentActivity = activity[i];
                for (var j = i + 1; j < activity.Count; j++)
                {
                    var nextActivity = activity[j];
                    if (currentActivity.Status == StandingStatus.Standing && nextActivity.Status == StandingStatus.Sitting)
                    {
                        totalDuration += nextActivity.EpochTimeStamp - currentActivity.EpochTimeStamp;
                        i = j;
                        break;
                    }
                }
            }
            return totalDuration;
        }

        public static double ConvertFromUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}