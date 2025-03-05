using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Common.Exceptions
{
    public class ForbiddenException : BaseException
    {
        public ForbiddenException(string details) : base(details)
        {
            Type = HttpCodeTypes.Error403Type;
            Title = HttpStatusCode.Forbidden.ToString();
            Status = (int)HttpStatusCode.Forbidden;
        }
    }
}
