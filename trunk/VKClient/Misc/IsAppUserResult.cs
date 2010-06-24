using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vkontakte.MethodResults;

namespace Vkontakte.Users
{
    /// <summary>
    /// Установил ли пользователь себе приложение.
    /// Данный класс возвращается методом <see cref="Users.IsAppUser"/>.
    /// </summary>
    class IsAppUserResult:IMethodResult
    {
        /// <summary>
        /// Установил ли пользователь себе приложение.
        /// </summary>
        public bool AppInstalled { get; set; }
    }
}
