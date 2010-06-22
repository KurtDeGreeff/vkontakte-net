using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vkontakte.MethodResults;

namespace Vkontakte.Activity
{
    public class ActivityResult: IMethodResult
    {
        public string UserId { get; set; }
        public string Activity { get; set; }
        public DateTime TimeStamp { get; set; } 
    }
}
