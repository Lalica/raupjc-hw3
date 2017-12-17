using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zad1
{
    public class ErrorLogs
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        public ErrorLogs(string text)
        {
            Id = Guid.NewGuid();
            Text = text;
        }
    }
}
