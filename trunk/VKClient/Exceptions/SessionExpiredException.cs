using System;

namespace Vkontakte.Exceptions
{
    public class SessionExpiredException:Exception
    {
        public override string Message
        {
            get
            {
                return "Сессия закончилась. Необходимо заново произвести авторизацию.";
            }
        }
    }
}
