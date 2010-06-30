namespace Vkontakte
{
    public enum ErrorCode { UnknownErrorOccured = 1, ApplicationIsDisabled = 2, IncorrectSignature = 4, UserAuthorizationFailed = 5, 
        TooManyRequestsPerSecond = 6, PermissionDeniedByUser = 7, InvalidOrMissingParameterSpecified = 100, ParsingOfResultFailed = 789, InvalidUserIds = 113, 	PermissionDeniedVotesProcessingDisabled = 500};
}
