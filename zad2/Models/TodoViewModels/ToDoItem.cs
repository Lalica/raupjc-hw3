using System;
using System.Collections.Generic;

namespace zad2.Models.TodoViewModels
{
    public class TodoItem
    {
        public TodoItem(string text, Guid userId)
        {
            Id = Guid.NewGuid();
            DateCreated = DateTime.UtcNow;
            Text = text;
            UserId = userId;
            IsCompleted = false;
            Labels = new List<TodoItemLabel>();
        }

        public TodoItem(string text, Guid userId, DateTime dateDue)
        {
            Id = Guid.NewGuid();
            DateCreated = DateTime.UtcNow;
            Text = text;
            UserId = userId;
            IsCompleted = false;
            DateDue = dateDue;
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
        public DateTime? DateDue { get; set; }
        public Guid UserId { get; set; }
        public List<TodoItemLabel> Labels { get; set; }

        public bool MarkAsCompleted()
        {
            if (!IsCompleted)
            {
                DateCompleted = DateTime.Now;
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
    }


    public class TodoItemLabel
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public List<TodoItem> LabelTodoItems { get; set; }

        public TodoItemLabel(string value)
        {
            Id = Guid.NewGuid();
            Value = value;
            LabelTodoItems = new List<TodoItem>();
        }
    }
}
