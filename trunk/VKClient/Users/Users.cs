using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Vkontakte.MethodResults;

namespace Vkontakte.Users
{
    public class Users:MethodLibraryBase
    {
        public Users(IVkAdapter adapter)
        {
            Adapter = adapter;
        }

        /// <summary>
        /// Получает настройки текущего пользователя в данном приложении.
        /// Возвращает битовую маску настроек текущего пользователя в данном приложени.
        /// Например, если метод возвращает 3, это означает, что пользователь разрешил отправлять ему уведомления и получать список его друзей.
        /// 
        /// Список возможных настроек:
        ///<list>
        ///<item><term>+1</term><description>пользователь разрешил отправлять ему уведомления.</description></item>
        ///<item><term>+2</term><description>доступ к друзьям.</description></item>
        ///<item><term>+4</term><description>доступ к фотографиям.</description></item>
        ///<item><term>+8</term><description>доступ к аудиозаписям.</description></item>
        ///<item><term>+16</term><description>доступ к видеозаписям.</description></item>
        ///<item><term>+32</term><description>доступ к предложениям.</description></item>
        ///<item><term>+64</term><description>доступ к вопросам.</description></item>
        ///<item><term>+128</term><description>доступ к wiki-страницам.</description></item>
        ///<item><term>+256</term><description>добавление ссылки на приложение в меню слева.</description></item>
        ///<item><term>+512</term><description>добавление ссылки на приложение для быстрой публикации на стенах пользователей.</description></item>
        ///<item><term>+1024</term><description>доступ к статусам пользователя.</description></item>
        ///<item><term>+2048</term><description>доступ заметкам пользователя.</description></item>
        ///</list>
        /// </summary>
        /// <returns>
        /// UserSettingsResult
        /// </returns>
        public int GetUserSettings()
        {
            var resultSettings =
            Adapter.CallRemoteMethod("getUserSettings", "3.0", (XElement methodResult) =>
            {
                var result = methodResult.Element("settings").Value;
                return Int32.Parse(result);
            });

            return resultSettings;
        }

        /// <summary>
        /// Данный метод возвращает информацию о том, установил ли текущий пользователь приложение или нет. 
        /// </summary>
        /// <returns>boolean</returns>
        public bool IsAppUser()
        {
            bool resultAppUser =
            Adapter.CallRemoteMethod("isAppUser", "3.0", (XElement methodResult) =>
            {
                bool result = Byte.Parse(methodResult.Value) == 1;
                return result;
            });

            return resultAppUser;
        }

        /// <summary>
        /// Возвращает список идентификаторов друзей текущего пользователя.
        /// </summary>
        /// <param name="userId">Id текущего пользователя</param>
        /// <returns>UserFriendResult</returns>
        public List<int> GetFriends(int userId)
        {
            var resultUsers =
            Adapter.CallRemoteMethod("getFriends", "3.0", (XElement methodResult) =>
            {
                var result = (from item in methodResult.Elements("uid")
                                    select Int32.Parse(item.Value)).ToList();
                    
                return result;
            });

            return resultUsers;
        }

        /// <summary>
        /// Возвращает список идентификаторов друзей текущего пользователя.
        /// </summary>
        /// <returns></returns>
        public List<int> GetFriends()
        {
            return GetFriends(Adapter.UserId);    
        }

        
        public List<User> GetProfiles(List<int> uids)
        {
            string uidsList =
                uids.Select(item => item.ToString()).Aggregate((current, next) => String.Format("{0},{1},", current, next));
            var methodParams = new Dictionary<string, string>() {{"uids", uidsList}, {"fields","photo"}};
            var resultProfiles =
            Adapter.CallRemoteMethod("getProfiles", "3.0", (XElement methodResult) =>
            {
                return methodResult.Elements("user").Select(item => ParseUser(item)).ToList();
            }, methodParams);

            return resultProfiles;
        }

        private User ParseUser(XElement item)
        {
            var user = new User();
            user.Uid = Int32.Parse(item.TryGetElementValue("uid"));
            user.FirstName = item.TryGetElementValue("first_name");
            user.LastName = item.TryGetElementValue("last_name");
            user.PhotoUrl = item.TryGetElementValue("photo");

            //TODO: fill other User fields

            return user;
        }

        


        public IMethodResult GetProfiles(List<int> uids, List<string> fields)
        {
            throw new NotImplementedException();
        }




    }
}
