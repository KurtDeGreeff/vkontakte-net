using System;

namespace Vkontakte
{
    [Serializable]
    public class SessionData
    {
        public string SessionId { get; set; }

        public string SecretKey { get; set; }

        public int UserId { get; set; }

        public DateTime SessionExpires { get; set; }

    }
}
