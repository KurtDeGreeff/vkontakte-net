using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Vkontakte
{
    public class Utils
    {
        public static string apiUrl = "http://api.vkontakte.ru/api.php";
        public static string apiVersion = "3.0";
        public static string format = "XML";

        
        public static string MakeMethodSig(int userId, int apiId, string methodName, 
            string sid, string secret, Dictionary<String,String> methodParams)
        {
            string sig = userId.ToString();

            methodParams = methodParams ?? new Dictionary<string, string>();
#if(DEBUG)
            methodParams.Add("test_mode", "1");
#endif
            methodParams.Add("api_id", apiId.ToString());
            methodParams.Add("method", methodName);
            methodParams.Add("v", apiVersion);
            methodParams.Add("format", format);

            sig += methodParams.OrderBy(item => item.Key).
                Select(item => string.Format("{0}={1}", item.Key, item.Value)).
                Aggregate((current, next) => current + next).ToString();

            sig += secret;
            
            var hash = GetMd5Hash(sig);

            return hash;
        }

        public static string GetMd5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            var md5Hasher = new MD5CryptoServiceProvider();

            // Convert the input string to a byte array and compute the hash.
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static string GetRequestUrl(string sig, string sessionId, Dictionary<string,string> methodParams)
        {
            var paramsString = methodParams.Select(item => string.Format("&{0}={1}", item.Key, item.Value)).
                                    Aggregate((current, next) => current + next).ToString();
            return String.Format("{0}?sig={1}&sid={2}{3}", apiUrl, sig, sessionId, paramsString);
        }

        public static SessionData ParseLogin(Uri responseUrl)
        {
            string data = Uri.UnescapeDataString(responseUrl.Fragment.ToString().Replace("#session=", "").Replace("\\",""));

            string pattern = "(\"(?<key>[a-z]+)\":\"*?(?<val>[a-zA-Z0-9]+)\"*?)";
            
            var regex = new Regex(pattern);
            var matches = regex.Matches(data);

            SessionData session = new SessionData();
            if(matches.Count < 4)
            {
                return null;
            }
            else
            {
                foreach (Match match in matches)
                {
                    var value = match.Groups["val"].Value;
                    
                    switch(match.Groups["key"].Value)
                    {
                        case "expire":
                                var d = new DateTime(1970,1,1,0,0,0);
                                session.SessionExpires = d.AddSeconds(Int32.Parse(value));
                            break;

                        case "sid":
                            session.SessionId = value;
                            break;

                        case "secret":
                            session.SecretKey = value;
                            break;

                        case "mid":
                            session.UserId = Int32.Parse(value);
                            break;
                    }
                }

            }

            
            return session;
        }

      
       
    }

}
