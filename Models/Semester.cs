using System;

namespace Models
{
    public class Semester
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public DateTime DateStartFirst { get; set; }
        public DateTime DateStopFirst { get; set; }
        public DateTime DateStartSecond { get; set; }
        public DateTime DateStopSecond { get; set; }
        public int OnlyFirst { get; set; }
    }
}
