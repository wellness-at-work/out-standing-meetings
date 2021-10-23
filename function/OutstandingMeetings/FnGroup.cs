using EnOutstandingMeetings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FnOutstandingMeetings
{
    public static class FnGroup
    {
        [FunctionName(nameof(CreateGroup))]
        public static async Task<IActionResult> CreateGroup(
             [OrchestrationTrigger] IDurableOrchestrationContext context,
             [Table(nameof(MeetingGroup))] CloudTable meetingGroupTable,
            ILogger log)
        {
            var groupClient = new DataAccess<MeetingGroup>(meetingGroupTable);
            var processingReq = context.GetInput<ProcessingRequest>();
            var group = JsonConvert.DeserializeObject<MeetingGroup>(processingReq.Payload);
            group.RowKey = group.Id;

            var dbGroup = await groupClient.GetAsync(nameof(MeetingGroup), group.Id);

            if (dbGroup == null)
            {
                await groupClient.InsertAsync(group);
            }

            return (ActionResult)new OkObjectResult(true);
        }

        [FunctionName(nameof(JoinGroup))]
        public static async Task<IActionResult> JoinGroup(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            [Table(nameof(MeetingGroup))] CloudTable meetingGroupTable,
           ILogger log)
        {
            var groupClient = new DataAccess<MeetingGroup>(meetingGroupTable);

            var processingReq = context.GetInput<ProcessingRequest>();
            var participant = JsonConvert.DeserializeObject<MeetingParticipant>(processingReq.Payload);

            var dbGroup = await groupClient.GetAsync(nameof(MeetingGroup), participant.GroupId.ToString());

            if (dbGroup != null)
            {
                if (dbGroup.ParticipantsSerialized != string.Empty)
                {
                    var participants = JsonConvert.DeserializeObject<List<MeetingParticipant>>(dbGroup.ParticipantsSerialized);
                    if (!participants.Any(p => p.Id == participant.Id))
                    {
                        participants.Add(participant);
                        dbGroup.ParticipantsSerialized = JsonConvert.SerializeObject(participants);
                        await groupClient.ReplaceAsync(dbGroup);
                    }
                }
                else
                {
                    dbGroup.ParticipantsSerialized = JsonConvert.SerializeObject(new List<MeetingParticipant>() { participant });
                    await groupClient.ReplaceAsync(dbGroup);
                }

            }

            return (ActionResult)new OkObjectResult(true);
        }

    }
}