using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using zad1.from_last_homework;

namespace zad2.Models
{
    public class TodoViewModel
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public Guid Id { get; set; }
        [Required]
        public bool IsCompleted { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public DateTime? DateDue { get; set; }
        public DateTime? DateCompleted { get; set; }
        public int DaysLeft { get; set; }
        public List<TodoItemLabel> Labels { get; set; }

        public TodoViewModel(TodoItem item)
        {
            Text = item.Text;
            Id = item.Id;
            IsCompleted = item.IsCompleted;
            DateCompleted = item.DateCompleted;
            DateCreated = item.DateCreated;
            DateDue = item.DateDue;
            if (item.Labels != null)
            {
                if (item.Labels.Count != 0)
                {
                    Labels = item.Labels;
                }
            }
            if ((DateTime.Now - DateDue).HasValue)
            {
                DaysLeft = (int)(DateDue - DateTime.Now).Value.TotalDays;
            }
        }
    }
}
