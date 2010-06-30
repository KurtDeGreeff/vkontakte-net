using System;
using System.Linq;
using Vkontakte.Constants;

namespace Vkontakte.Activity
{
    public static class Activity
    {
        private static IVkAdapter Adapter;
        
        static Activity()
        {
            Adapter = VkAdapter.Instance;
        }

        public static ActivityResult Get()
        {
            //var methodParams = new Dictionary<string, string>() { {"uid", Adapter.UserId.ToString()} };
            var resultActivity =
            Adapter.CallRemoteMethod(MethodNames.ActivityGet, "3.0", methodResult =>
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

        //TODO: Заимплементить остальные методы по работе со статусами
    }
}
