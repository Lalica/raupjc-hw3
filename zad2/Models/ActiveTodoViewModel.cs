using System.Collections.Generic;
using System.Linq;

namespace zad2.Models
{
    public class ActiveTodoViewModel
    {
        public List<TodoViewModel> Items { get; set; }

        public ActiveTodoViewModel()
        {
            Items = new List<TodoViewModel>();
        }
    }
}
