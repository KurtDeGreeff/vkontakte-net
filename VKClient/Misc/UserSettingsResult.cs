using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vkontakte.MethodResults;

namespace Vkontakte.Misc
{
    public class UserSettingsResult:IMethodResult
    {
        public int Settings { get; set; }
    }
}
