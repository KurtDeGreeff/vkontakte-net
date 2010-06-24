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
        public IMethodResult GetUserSettings()
        {
            try
            {
                var resultSettings =
                Adapter.CallRemoteMethod("getUserSettings", "3.0", (XElement methodResult) =>
                {
                    var result = (from item in methodResult.Elements("settings")
                                  select new UserSettingsResult() {
                                                 Settings = Int32.Parse(item.Value),
                                             }).First();
                    return result;
                });

                return resultSettings;
            }
            catch (NullReferenceException)
            {
                return new ErrorResult()
                           {ErrorCode = ErrorCode.UnknownErrorOccured, ErrorMessage = "Unknown error occured."};
            }
        }

        /// <summary>
        /// Данный метод возвращает информацию о том, установил ли текущий пользователь приложение или нет. 
        /// </summary>
        /// <returns>IsAppUserResult </returns>
        public IMethodResult IsAppUser()
        {
            try
            {
                var resultAppUser =
                Adapter.CallRemoteMethod("getUserSettings", "3.0", (XElement methodResult) =>
                {
                    var result = new IsAppUserResult() {AppInstalled = Byte.Parse(methodResult.Value) == 1};
                    return result;
                });

                return resultAppUser;
            }
            catch (NullReferenceException)
            {
                return new ErrorResult() { ErrorCode = ErrorCode.UnknownErrorOccured, ErrorMessage = "Unknown error occured." };
            }
        }


    }
}
