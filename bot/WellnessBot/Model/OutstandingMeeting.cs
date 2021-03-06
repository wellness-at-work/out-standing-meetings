namespace SidePanel.Model
{
    public class OutstandingMeeting
    {
        public string UserId { get; set; }
        public string Group { get; set; }
        public string User { get; set; }
        public WellnessActivityType ActivityType { get; set; }
    }

    public enum WellnessActivityType
    {
        Standing = 1,
        Sitting = 2
    }
}
