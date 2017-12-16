using System;
using System.ComponentModel.DataAnnotations;

namespace zad2.Models
{
    public class AddTodoViewModel
    {
        [Required]
        public string Text { get; set; }
        public DateTime DateDue { get; set; }

        public AddTodoViewModel() { }
        public AddTodoViewModel(string text, DateTime dateDue)
        {
            Text = text;
            DateDue = dateDue;
        }
    }
}
