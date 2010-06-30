using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Vkontakte.MethodResults;

namespace Vkontakte
{
    public interface IVkAdapter
    {
        T CallRemoteMethod<T>(string name, string version, Func<XElement, T> resultMethod, Dictionary<String, String> methodParams = null);
        void CallRemoteMethodAsync();
        int UserId { get; set; }
        int AppId { get; set; }
        bool Authenticated { get; set; }
        bool Authenticate(SessionData sessionData, int userId, int appId);
    }
}
