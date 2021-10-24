using System;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;

namespace EnOutstandingMeetings
{
    public class MeetingGroup : TableEntity
    {
        public string Id { get; set; }

        public string ParticipantsSerialized { get; set; }

        public MeetingGroup()
        {
            PartitionKey = nameof(MeetingGroup);
            ParticipantsSerialized = "";
        }
    }
}
