using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vkontakte.MethodResults
{
    public class ErrorResult:IMethodResult
    {
        public ErrorCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
