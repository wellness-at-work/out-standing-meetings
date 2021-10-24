using System.Collections.Generic;

namespace EnOutstandingMeetings
{
    public class MeetingParticipant 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Duration { get; set; }
        public string GroupId { get; set; }
        public List<ParticipantActivity> Activity { get; set; } 

        public MeetingParticipant()
        {
            Activity  = new List<ParticipantActivity>();
        }
    }
}
