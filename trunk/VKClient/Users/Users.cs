using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Vkontakte.Constants;
using Vkontakte.Date;
using Vkontakte.MethodResults;

namespace Vkontakte.Users
{
    public static class Users
    {
        private static IVkAdapter Adapter;

        static Users()
        {
            Adapter = VkAdapter.Instance;
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
        public static int GetUserSettings()
        {
            return GetUserSettings(Adapter.UserId);
        }

        /// <summary>
        /// Получает настройки пользователя в данном приложении.
        /// Возвращает битовую маску настроек текущего пользователя в данном приложени.
        /// Например, если метод возвращает 3, это означает, что пользователь разрешил отправлять ему уведомления и получать список его друзей.
        /// </summary>
        /// <param name="uid">Uid пользователя</param>
        /// <returns></returns>
        public static int GetUserSettings(int uid)
        {
            var methodParams = new Dictionary<string, string>() { { "uid", uid.ToString()}};
            var resultSettings =
            Adapter.CallRemoteMethod(MethodNames.GetUserSettings, "3.0", (XElement methodResult) =>
            {
                var result = methodResult.TryGetElementValue("settings");
                return Int32.Parse(result);
            }, methodParams);

            return resultSettings;
        }

        /// <summary>
        /// Данный метод возвращает информацию о том, установил ли текущий пользователь приложение или нет. 
        /// </summary>
        /// <returns>boolean</returns>
        public static bool IsAppUser()
        {
            return IsAppUser(Adapter.UserId);
        }

        /// <summary>
        /// Данный метод возвращает информацию о том, установил ли пользователь приложение или нет. 
        /// </summary>
        /// <param name="uid">Uid пользователя</param>
        /// <returns>boolean</returns>
        public static bool IsAppUser(int uid)
        {
            var methodParams = new Dictionary<string, string>() { { "uid", uid.ToString() } };
            bool resultAppUser =
            Adapter.CallRemoteMethod(MethodNames.IsAppUser, "3.0", (XElement methodResult) =>
            {
                bool result = Byte.Parse(methodResult.Value) == 1;
                return result;
            }, methodParams);

            return resultAppUser;
        }

        
        /// <summary>
        /// Возвращает список идентификаторов друзей текущего пользователя.
        /// </summary>
        /// <returns></returns>
        public static List<int> GetFriends()
        {
            var resultUsers =
             Adapter.CallRemoteMethod(MethodNames.GetFriends, "3.0", (XElement methodResult) =>
             {
                 var result = (from item in methodResult.Elements("uid")
                               select Int32.Parse(item.Value)).ToList();

                 return result;
             });

            return resultUsers;   
        }

        /// <summary>
        /// Возвращает профайлы списка пользователей.
        /// </summary>
        /// <param name="uids">Список uid пользователей, чьи профайлы необходимо загрузить</param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static List<User> GetProfiles(List<int> uids, List<string> fields)
        {
            string uidsList = uids.ToConcatenetedString();
                //uids.Select(item => item.ToString()).Aggregate((current, next) => String.Format("{0},{1},", current, next));

            string fieldsList = fields.ToConcatenetedString();
               //fields.Select(item => item.ToString()).Aggregate((current, next) => String.Format("{0},{1},", current, next));

            var methodParams = new Dictionary<string, string>() { { "uids", uidsList }, { "fields", fieldsList } };
            var resultProfiles =
                Adapter.CallRemoteMethod(MethodNames.GetProfiles, "3.0", (XElement methodResult) => methodResult.Elements("user").Select(ParseUser).ToList(), methodParams);

            return resultProfiles;
        }


        /// <summary>
        /// Возвращает профайлы списка пользователей. По умолчанию возвращаются поля Uid, Имя, Фамилия.
        /// </summary>
        /// <param name="uids">Список uid пользователей, чьи профайлы необходимо загрузить</param>
        /// <returns></returns>
        public static List<User> GetProfiles(List<int> uids)
        {
            return GetProfiles(uids, new List<string>());
        }

        private static User ParseUser(XElement item)
        {
            var user = new User();
            
            user.Uid = Int32.Parse(item.TryGetElementValue(ProfileFields.Uid));
            user.FirstName = item.TryGetElementValue(ProfileFields.FirstName);
            user.LastName = item.TryGetElementValue(ProfileFields.LastName);
            user.Nickname = item.TryGetElementValue(ProfileFields.Nickname);
            
            var sex = item.TryGetElementValue(ProfileFields.Sex);
            user.Sex = sex == "" ? UserSex.NoSex : (UserSex)Enum.Parse(typeof(UserSex), item.TryGetElementValue(ProfileFields.Sex));
            
            user.BirthDate = new BirthDate(item.TryGetElementValue(ProfileFields.BirdthDate));

            var city = item.TryGetElementValue(ProfileFields.CityId);
            user.CityId = city == "" ? -1 : Int32.Parse(city);

            var country = item.TryGetElementValue(ProfileFields.CountryId);
            user.CountryId = country == "" ? -1 : Int32.Parse(country);

            user.PhotoUrl = item.TryGetElementValue(ProfileFields.PhotoUrl);
            user.PhotoMediumUrl = item.TryGetElementValue(ProfileFields.PhotoMediumUrl);
            user.PhotoBigUrl = item.TryGetElementValue(ProfileFields.PhotoBigUrl);

            user.Timezone = item.TryGetElementValue(ProfileFields.Timezone);

            var hasMobile = item.TryGetElementValue(ProfileFields.HasMobile);
            user.HasMobile = hasMobile == "" ? false : Byte.Parse(hasMobile) == 1;

            var rate = item.TryGetElementValue(ProfileFields.Rate);
            user.Rate = rate == "" ? -1 : Int32.Parse(rate);

            user.HomePhone = item.TryGetElementValue(ProfileFields.HomePhone);
            user.MobilePhone = item.TryGetElementValue(ProfileFields.MobilePhone);

            var universityId = item.TryGetElementValue(ProfileFields.UniversityId);
            user.UniversityId = universityId == "" ? -1 : Int32.Parse(universityId);

            user.UniversityName = item.TryGetElementValue(ProfileFields.UniversityName);

            var facultyId = item.TryGetElementValue(ProfileFields.FacultyId);
            user.FacultyId = facultyId == "" ? -1 : Int32.Parse(facultyId);

            user.FacultyName = item.TryGetElementValue(ProfileFields.FacultyName);
            
            var graduation = item.TryGetElementValue(ProfileFields.Graduation);
            user.Graduation = graduation == "" ? -1 : Int32.Parse(graduation);

            return user;
        }


        /// <summary>
        /// Возвращает баланс текущего пользователя на счету приложения в сотых долях голоса.
        /// </summary>
        /// <returns></returns>
        public static int GetUserBalance()
        {
            var resultBalance =
            Adapter.CallRemoteMethod(MethodNames.GetUserBalance, "3.0", (XElement methodResult) =>
            {
                var result = methodResult.TryGetElementValue("balance");
                return Int32.Parse(result);
            });

            return resultBalance;
        }

        /// <summary>
        /// Возвращает список id групп текущего пользователя
        /// </summary>
        /// <returns></returns>
        public static List<int> GetGroups()
        {

            var resultGroups =
              Adapter.CallRemoteMethod(MethodNames.GetGroups, "3.0", (XElement methodResult) =>
              {
                  var result = (from item in methodResult.Elements("gid")
                                select Int32.Parse(item.Value)).ToList();

                  return result;
              });

            return resultGroups;   
        }

        /// <summary>
        /// Возвращает базовую информацию о группах текущего пользователя
        /// </summary>
        /// <returns></returns>
        public static List<Group> GetGroupsFull()
        {
            return GetGroupsFull(new List<int>());
        }


        /// <summary>
        /// Возвращает базовую информацию о группах из списка uids
        /// </summary>
        /// <param name="gids">Список id групп, по которым надо получить информацию</param>
        /// <returns></returns>
        public static List<Group> GetGroupsFull(List<int> gids)
        {
            var methodParams = new Dictionary<string, string>();
            
            if(gids.Count > 0)
            {
                methodParams.Add("gids", gids.ToConcatenetedString());    
            }

            var resultGroups =
               Adapter.CallRemoteMethod(MethodNames.GetGroupsFull, "3.0", (XElement methodResult) => methodResult.Elements("group").Select(ParseGroup).ToList(), methodParams);

            return resultGroups;
        }

        private static Group ParseGroup(XElement element)
        {
            var group = new Group();

            group.Id = Int32.Parse(element.TryGetElementValue(GroupFields.Id));
            group.Name = element.TryGetElementValue(GroupFields.Name);
            group.PhotoUrl = element.TryGetElementValue(GroupFields.PhotoUrl);
            var isClosed = element.TryGetElementValue(GroupFields.IsClosed);
            group.IsClosed = isClosed == "" ? false : int.Parse(isClosed) == 1;
            
            return group;
        }


    }
}
