using System;

namespace Vkontakte.Exceptions
{
    /// <summary>
    /// Исключение выбрасывается, когда метод апи vkontakte.ru вернул ошибку
    /// </summary>
    public class RemoteMethodException:Exception
    {
        public ErrorCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public RemoteMethodException(ErrorCode errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}
