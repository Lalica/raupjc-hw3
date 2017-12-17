using System.Collections.Generic;

namespace zad2.Models
{
    public class IndexViewModel
    {
        public List<TodoViewModel> Items;

        public IndexViewModel()
        {
            Items = new List<TodoViewModel>();
        }
    }
}
