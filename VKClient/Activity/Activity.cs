using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Vkontakte.MethodResults;

namespace Vkontakte.Activity
{
    public class Activity
    {
        private IVkAdapter Adapter;

        public Activity(IVkAdapter vkAdapter)
        {
            this.Adapter = vkAdapter;
        }

        public IMethodResult Get()
        {
            try
            {
                var methodParams = new Dictionary<string, string>() { { "test_mode", "1" }, { "uid", Adapter.UserId.ToString() } };
                var resultActivity =
                Adapter.CallRemoteMethod("activity.get", "3.0", methodParams, (XElement methodResult) =>
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
            catch (NullReferenceException)
            {
                
                throw;
            }
            
        }
    }
}
