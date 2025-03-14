using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

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
        public string title;
        public string description;
        public int userID;

        public int plantID;
        public int categoryID;

        public DateTime createdDate;
        public DateTime dueDate ;
        public Status status;
        public Priority priority;
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