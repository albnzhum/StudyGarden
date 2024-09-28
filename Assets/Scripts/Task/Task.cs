using System;
using System.Collections.Generic;

namespace Task
{
    [Serializable]
    public class Tasks
    {
        public List<Task> TaskList;
    }
    
    [Serializable]
    public class Task
    {
        public int ID;

        public int UserID;
        public string Title;
        public string Description;
        public int CategoryID;
        public int PlantID;

        public DateTime CreatedDate;
        public DateTime DueDate ;
        public Status Status;
        public Priority Priority;
    }

    public enum Status
    {
        NotStarted,
        InProgress,
        Done,
        Overdue
    }

    public enum Priority
    {
        Important,
        Urgently,
        NotUrgent
    }
}