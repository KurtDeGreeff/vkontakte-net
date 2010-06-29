using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Vkontakte.MethodResults;

namespace Vkontakte.Activity
{
    public class Activity:MethodLibraryBase
    {
        public Activity(IVkAdapter adapter)
        {
            base.Adapter = adapter;
        }

        public ActivityResult Get()
        {
            //var methodParams = new Dictionary<string, string>() { {"uid", Adapter.UserId.ToString()} };
            var resultActivity =
            Adapter.CallRemoteMethod("activity.get", "3.0", methodResult =>
            {
                var result = (from item in methodResult.Elements("response")
                                select new ActivityResult
                                {
                                    Activity = item.Element("activity").Value,
                                    TimeStamp = DateTime.Parse(item.Element("time").Value),
                                    UserId = item.Element("id").Value
                                }).First();
                return result;
            });

            return resultActivity;
            
        }
    }
}
