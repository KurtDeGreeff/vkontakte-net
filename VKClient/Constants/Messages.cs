using System.Collections.Generic;

namespace Vkontakte.Constants
{
    // TODO: Записать внятные описания ошибок для всех ЕррорКодов
    public class Messages
    {
        public static Dictionary<ErrorCode, string> ErrorMessages = new Dictionary<ErrorCode, string>() { { ErrorCode.ParsingOfResultFailed, "Parsing of method results failed" } };
    }
}
