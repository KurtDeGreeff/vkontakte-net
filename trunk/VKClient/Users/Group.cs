namespace Vkontakte.Users
{
    public class Group
    {
        /// <summary>
        /// Id группы
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название группы
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// URL аватара группы
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Является ли группа закрытой
        /// </summary>
        public bool IsClosed { get; set; }

        public Group()
        {
            Id = -1;
            Name = "";
            PhotoUrl = "";
            IsClosed = false;
        }
    }
}
