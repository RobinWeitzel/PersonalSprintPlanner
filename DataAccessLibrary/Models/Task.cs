using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models
{
    public enum Status
    {
        ToDo = 1,
        InProgress = 2,
        Review = 3,
        Completed = 4,
        Archived = 5
    }

    public enum Priority
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    public class Task : IEquatable<Task>
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Position { get; set; }
        public string SprintPosition { get; set; }
        public long BoardID { get; set; }
        public Status Status { get; set; }
        public bool SprintRelevant { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public Priority Priority { get; set; }

        public bool Equals(Task other)
        {
            return other.ID == ID;
        }
    }
}
