using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutstandingMeetings.Data
{
    public class MeetingParticipantReponse
    {
        public List< MeetingParticipant> Participants { get; set; }
        public List<MeetingParticipant> AllTimeRecord { get; set; }
    }
}
