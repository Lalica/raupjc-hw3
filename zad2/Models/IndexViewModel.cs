﻿using System.Collections.Generic;

namespace zad2.Models
{
    public class IndexViewModel
    {
        public  List<TodoViewModel> Items { get; set; }

        public IndexViewModel()
        {
            Items = new List<TodoViewModel>();
        }
    }
}
