namespace VkDownloader.Settings
{
    public interface ISettings
    {
        /// <summary>
        /// Получает идентификатор приложения
        /// </summary>
        string AppId { get; }

        /// <summary>
        /// Получает или задает токен авторизации
        /// </summary>
        string AccessToken { get; set; }
    }
}
