using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vkontakte
{
    public enum ErrorCode { UnknownErrorOccured, ApplicationIsDisabled, IncorrectSignature, UserAuthorizationFailed, 
        TooManyRequestsPerSecond, PermissionDeniedByUser, InvalidOrMissingParameterSpecified = 100, ParsingOfResultFailed = 789   };

    public static class Messages
    {
        public static Dictionary<ErrorCode, string> ErrorMessages = new Dictionary<ErrorCode, string>()
                                                                        {{ErrorCode.ParsingOfResultFailed, "Parsing of method results failed"}};
    }

    

}
