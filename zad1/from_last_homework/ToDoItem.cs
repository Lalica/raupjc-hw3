using System;
using System.Collections.Generic;

namespace zad1.from_last_homework
{
    public class TodoItem
    {
        public TodoItem(string text, Guid userId)
        {
            // Generates new unique identifier
            Id = Guid.NewGuid();
            // DateTime .Now returns local time , it wont always be what you expect (depending where the server is).
            // We want to use universal (UTC ) time which we can easily convert to local when needed.
            // ( usually done in browser on the client side )
            DateCreated = DateTime.UtcNow;
            Text = text;
            UserId = userId;
            IsCompleted = false;
            Labels = new List<TodoItemLabel>();
        }

        public TodoItem()
        { // entity framework needs this one 
          // not for use :) 
        }

        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? DateCompleted { get; set; }
        public DateTime DateCreated { get; set; }


        public bool MarkAsCompleted()
        {
            if (!IsCompleted)
            {
                DateCompleted = DateTime.Now;
                IsCompleted = true;
                return true;
            }
            return false;
        }

        protected bool Equals(TodoItem other)
        {
            return Guid.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TodoItem)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Id.GetHashCode() * 397;
            }
        }

        /// <summary>
        /// User id that owns this TodoItem 
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// List of labels associated with TodoItem 
        /// </summary> 
        public List<TodoItemLabel> Labels { get; set; }

        /// <summary> 
        /// Date due. If null, no date was set by the user 
        /// </summary> 
        public DateTime? DateDue {get;set;}
    }


    /// <summary> 
    /// /// Label describing the TodoItem. 
    /// /// e.g. Critical, Personal, Work... 
    /// /// </summary> 
    public class TodoItemLabel
    {
        public Guid Id { get; set; }
        public string Value { get; set; }

        /// <summary> 
        /// All TodoItems that are associated with this label 
        /// </summary> 
        public List<TodoItem> LabelTodoItems { get; set; }
        public TodoItemLabel(string value)
        {
            Id = Guid.NewGuid();
            Value = value;
            LabelTodoItems = new List<TodoItem>();
        }
    }
}
