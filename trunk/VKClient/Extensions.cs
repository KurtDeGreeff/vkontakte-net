
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Vkontakte
{
    public static class Extensions
    {
        public static string TryGetElementValue(this XElement element, string elementName)
        {
            var el = element.Element(elementName);
            return (el == null ? "" : el.Value);
        }

        public static string ToConcatenetedString<T>(this List<T> uids)
        {
            if (uids.Count == 0)
            {
                return "";
            }
 
            string uidsString =
                uids.Select(item => item.ToString()).Aggregate((current, next) => String.Format("{0},{1},", current, next));

            var chars = new char[] {','};
            return uidsString.TrimEnd(chars);
        }
    }
}
