using System.ComponentModel.DataAnnotations;

namespace zad2.Models
{
    public class AddTodoViewModel
    {
        [Required]
        [Display(Name = "Todo name")]
        public string Text { get; set; }
    }
}
