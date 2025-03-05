using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Common.Exceptions
{
    public class BaseException : System.Exception
    {
        public string? Type { get; set; }
        public string? Title { get; set; }
        public int Status { get; set; }
        public string? Detail { get; set; }
        public BaseException() : base()
        {

        }
        public BaseException(string details) : base(details)
        {
            Detail = details;
        }
    }
}
