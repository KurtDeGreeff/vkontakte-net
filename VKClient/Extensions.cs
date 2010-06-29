using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Vkontakte
{
    public static class Extensions
    {
        public static string TryGetElementValue(this XElement element, string elementName)
        {
            try
            {
                return element.Element(elementName).Value;
            }
            catch (NullReferenceException)
            {
                return "";
            }
        }
    }
}
