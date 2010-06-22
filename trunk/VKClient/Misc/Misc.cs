using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Vkontakte.MethodResults;

namespace Vkontakte.Misc
{
    public class Misc:MethodLibraryBase
    {
        public Misc(IVkAdapter adapter)
        {
            base.Adapter = adapter;
        }

        public IMethodResult GetUserSettings()
        {
            try
            {
                var resultSettings =
                Adapter.CallRemoteMethod("getUserSettings", "3.0", new Dictionary<string, string>(), (XElement methodResult) =>
                {
                    var result = (from item in methodResult.Elements("settings")
                                  select new UserSettingsResult()
                                             {
                                                 Settings = Int32.Parse(item.Value),
                                             }).First();
                    return result;
                });

                return resultSettings;
            }
            catch (NullReferenceException)
            {

                throw;
            }
        }
    }
}
