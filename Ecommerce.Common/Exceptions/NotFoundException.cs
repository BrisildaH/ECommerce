using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Common.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string details) : base(details)
        {
            Type = HttpCodeTypes.Error404Type;
            Title = HttpStatusCode.NotFound.ToString();
            Status = (int)HttpStatusCode.NotFound;
        }
    }
}

