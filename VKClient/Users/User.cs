using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vkontakte.Users
{
    /// <summary>
    /// 
    /// </summary>
    public class User
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public Int32 Uid { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия пользователя.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Ник пользователя.
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Пол пользователя.
        /// </summary>
        public UserSex Sex { get; set; }

        /// <summary>
        /// Дата рождения пользователя.
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Id города, указанного у пользователя в разделе "Контакты. Название города можно узнать вызвав метод <see cref="Geo.GetCities" />;
        /// </summary>
        public Int32 CityId { get; set; }

        /// <summary>
        /// Id страны, указанной у пользователя в разделе "Контакты. Название страны можно узнать вызвав метод <see cref="Geo.GetCountries" />;
        /// </summary>
        public Int32 CountryId { get; set; }

        /// <summary>
        /// Временная зона
        /// </summary>
        public string Timezone { get; set; }

        /// <summary>
        /// Url фотографии пользователя, имеющей ширину 50 пикселей.
        /// В случае отсутствия у пользователя фотографии выдаётся ответ: "images/question_c.gif" 
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Url фотографии пользователя, имеющей ширину 100 пикселей.
        /// В случае отсутствия у пользователя фотографии выдаётся ответ: "images/question_b.gif" 
        /// </summary>
        public string PhotoMediumUrl { get; set; }

        /// <summary>
        /// Url фотографии пользователя, имеющей ширину 200 пикселей.
        /// В случае отсутствия у пользователя фотографии выдаётся ответ: "images/question_a.gif" 
        /// </summary>
        public string PhotoBigUrl { get; set; }

        /// <summary>
        /// Показывает, известен ли номер мобильного телефона пользователя.
        /// Рекомендуется перед вызовом метода <see cref="Secure.SendSMSNotification" />
        /// </summary>
        public bool HasMobile { get; set; }

        /// <summary>
        /// Рейтинг пользователя.
        /// </summary>
        public Int32 Rate { get; set; }


        /// <summary>
        /// Домашний телефон пользователя. 
        /// </summary>
        public string HomePhone { get; set; }

        /// <summary>
        /// Мобильный телефон пользователя.
        /// </summary>
        public string MobilePhone { get; set; }

        
        /// <summary>
        /// Код университета пользователя. 
        /// </summary>
        public Int32 UniversityId { get; set; }

        /// <summary>
        /// Название университета пользователя. 
        /// </summary>
        public string UniversityName { get; set; }

        /// <summary>
        /// Код факультета пользователя. 
        /// </summary>
        public Int32 FacultyId { get; set; }

        /// <summary>
        /// Название факультета пользователя.
        /// </summary>
        public string FacultyName { get; set; }

        /// <summary>
        /// Год выпуска. 
        /// </summary>
        public DateTime? Graduation { get; set; }        

        public User()
        {
            Uid = -1;
            FirstName = "";
            LastName = "";
            Nickname = "";
            Sex = UserSex.NoSex;
            BirthDate = null;
            CityId = -1;
            CountryId = -1;
            Timezone = "";
            HomePhone = "";
            MobilePhone = "";
            UniversityId = -1;
            UniversityName = "";
            FacultyId = -1;
            FacultyName = "";
            Graduation = null;
            HasMobile = false;
            Rate = -1;
            PhotoUrl = "";
            PhotoMediumUrl = "";
            PhotoBigUrl = "";
        }
    }
}
