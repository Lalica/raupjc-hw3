using System.Collections.Generic;

namespace zad2.Models
{
    public class CompletedViewModel
    {
        public List<TodoViewModel> Items;

        public CompletedViewModel()
        {
            Items = new List<TodoViewModel>();
        }
    }
}
