using System;
using System.Text.Json.Serialization;

namespace Frontend.Model
{
    public class TaskModel
    {
        private int id;
        private DateTime creationTime;
        private string title;
        private string description;
        private DateTime dueDate;
        private string assignee;


        [JsonConstructor]
        public TaskModel(int Id, DateTime CreationTime, string Title, string Description, DateTime DueDate, string Assignee)
        {
            this.Id = Id;
            this.CreationTime = CreationTime;
            this.Title = Title;
            this.Description = Description;
            this.DueDate = DueDate;
            this.Assignee = Assignee;       
        }
        /// <summary>
        /// Get and set for id of task.
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Get and set for creation time of task.
        /// </summary>
        public DateTime CreationTime
        {
            get { return creationTime; }
            set { creationTime = value; }
        }

        /// <summary>
        /// Get and set for title of task.
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// Get and set for description of task.
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Get and set for due date of task.
        /// </summary>
        public DateTime DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }

        /// <summary>
        /// Get and set for assignee of task.
        /// </summary>
        public string Assignee
        {
            get { return assignee; }
            set { assignee = value; }
        }
    }
}
