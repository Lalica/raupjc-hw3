using System.Collections.Generic;

namespace zad2.Models
{
    public class IndexViewModel
    {
        public  List<TodoViewModel> _Items { get; set; }

        public IndexViewModel()
        {
            _Items = new List<TodoViewModel>();
        }
    }
}
