namespace Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public int Lesson { get; set; }
        public int WeekDay { get; set; }
        public string Subject { get; set; }
        public string Room { get; set; }
        public string GroupName { get; set; }
        public string Teacher1 { get; set; }
        public string Teacher2 { get; set; }
    }
}
