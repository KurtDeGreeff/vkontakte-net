using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Vkontakte.MethodResults;

namespace Vkontakte
{
    public interface IVkAdapter
    {
        IMethodResult CallRemoteMethod(string name, string version, Dictionary<String, String> methodParams, Func<XElement, IMethodResult> resultMethod);
        void CallRemoteMethodAsync();
        int UserId { get; set; }
        int AppId { get; set; }
    }
}
