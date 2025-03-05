using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Common.Exceptions
{
    public class ConflictException : BaseException
    {
        public ConflictException(string details) : base(details)
        {
            Type = HttpCodeTypes.Error409Type;
            Title = HttpStatusCode.Conflict.ToString();
            Status = (int)HttpStatusCode.Conflict;
        }
    }
}
