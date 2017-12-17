using System;
using System.ComponentModel.DataAnnotations;

namespace zad2.Models
{
    public class AddTodoViewModel
    {
        [Required]
        [Display(Name = "Todo name")]
        public string Text { get; set; }
        [Display(Name = "Labels")]
        public string Label { get; set; }
        [Display(Name = "Date due")]
        public DateTime? DateDue { get; set; }
    }
}
