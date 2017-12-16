using System;

namespace zad2.Models
{
    public class TodoViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateCompleted { get; set; }
        public DateTime? DateDue { get; set; }
        public int DaysLeft { get; set; }

        public TodoViewModel(Guid id, string text, DateTime dateCreated, DateTime? dateDue, bool isCompleted)
        {
            Id = id;
            Text = text;
            DateDue = dateDue;
            DateCreated = dateCreated;
            IsCompleted = isCompleted;
            if ((dateDue - DateTime.Today).HasValue)
            {
                DaysLeft = (int)(dateDue - DateTime.Today).Value.TotalDays;
            }
        }
    }
}
