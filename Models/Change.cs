using System;

namespace Models
{
    public class Change
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public DateTime ChangeDate { get; set; }
        public int Lesson { get; set; }
        public string SubjectOn { get; set; }
    }
}
