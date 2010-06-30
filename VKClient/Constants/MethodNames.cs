namespace Vkontakte.Constants
{
    class MethodNames
    {
        // Пользователи

        public static readonly string IsAppUser = "isAppUser";
        public static readonly string GetProfiles = "getProfiles";
        public static readonly string GetFriends = "getFriends";
        public static readonly string GetAppFriends = "getAppFriends";
        public static readonly string GetUserBalance = "getUserBalance";
        public static readonly string GetUserSettings = "getUserSettings";
        public static readonly string GetGroups = "getGroups";
        public static readonly string GetGroupsFull = "getGroupsFull";

        // Личный статус

        public static readonly string ActivityGet = "activity.get";
        public static readonly string ActivitySet = "activity.set";
        public static readonly string ActivityGetHistory = "activity.getHistory";
        public static readonly string DeleteHistoryItem = "activity.deleteHistoryItem";
        public static readonly string GetNews = "activity.getNews";

        // TODO: Записать остальные названия методов
    }
}
