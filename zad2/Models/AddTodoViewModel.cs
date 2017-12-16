using System;
using System.ComponentModel.DataAnnotations;
using zad2.Models.TodoViewModels;

namespace zad2.Models
{
    public class AddTodoViewModel
    {
        [Required]
        public string Text { get; set; }
        public DateTime DateDue { get; set; }
        public TodoItemLabel Label { get; set; }

        public AddTodoViewModel() { }
        public AddTodoViewModel(string text, DateTime dateDue, TodoItemLabel label)
        {
            Text = text;
            DateDue = dateDue;
            Label = label;
        }
    }
}
