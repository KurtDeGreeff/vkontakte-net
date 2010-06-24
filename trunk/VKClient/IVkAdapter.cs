using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Vkontakte.MethodResults;

namespace Vkontakte
{
    public interface IVkAdapter
    {
        IMethodResult CallRemoteMethod(string name, string version, Func<XElement, IMethodResult> resultMethod, Dictionary<String, String> methodParams = null);
        void CallRemoteMethodAsync();
        int UserId { get; set; }
        int AppId { get; set; }
    }
}
