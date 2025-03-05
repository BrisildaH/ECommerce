using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Common.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string details) : base(details)
        {
            Type = HttpCodeTypes.Error400Type;
            Title = HttpStatusCode.BadRequest.ToString();
            Status = (int)HttpStatusCode.BadRequest;
        }
    }
}
