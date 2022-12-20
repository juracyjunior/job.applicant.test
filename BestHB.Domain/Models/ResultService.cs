using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BestHB.Domain.Models
{
    public class ResultService<T>
    {
        public bool HasError { get { return Errors != null && Errors.Any(); } }
        public IList<string> Errors { get; set; }
        public T Data { get; set; }

        public ResultService() { 
            Errors = new List<string>();
        }
    }
}
