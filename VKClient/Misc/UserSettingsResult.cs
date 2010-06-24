using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vkontakte.MethodResults;

namespace Vkontakte.Users
{
    /// <summary>
    /// Текущие настройки приложения.
    /// Данный класс возвращается методом <see cref="Users.GetUserSettings"/>.
    /// </summary>
    public class UserSettingsResult:IMethodResult
    {
        /// <summary>
        /// Битовая маска текущих настроек прилождения.
        /// </summary>
        public int Settings { get; set; }
    }
}
