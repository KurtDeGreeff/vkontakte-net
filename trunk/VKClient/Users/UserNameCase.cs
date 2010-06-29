using System.Collections.Generic;

namespace Vkontakte.Users
{
    public class UserNameCase
    {
        public static readonly Dictionary<string, string> Case = new Dictionary<string, string>()
                                                                     {
                                                                         { "Nominative", "nom" },
                                                                         { "Genitive", "gen" },
                                                                         { "Dative", "dat" },
                                                                         { "Accusative", "acc" },
                                                                         { "Ablative", "ins" },
                                                                         { "Preposition", "abl" },
                                                                     };
    }
}
