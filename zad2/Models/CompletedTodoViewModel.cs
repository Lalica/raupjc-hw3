using System.Collections.Generic;
using System.Linq;

namespace zad2.Models
{
    public class CompletedTodoViewModel
    {
        public List<TodoViewModel> Items { get; set; }

        public CompletedTodoViewModel()
        {
            Items = new List<TodoViewModel>();
        }
    }
}
