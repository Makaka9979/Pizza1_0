using Telegram.Bot.Types;
using Telegram.Bot;

namespace Libs
{
    /// <summary>
    /// Интерфейс, который должны реализовывать контроллеры.
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// Обрабатывает обновления/действие и возвращает представление.
        /// </summary>
        /// <param name="update">Информация о обновлении, которое нужно обработать.</param>
        /// <returns>Представление, которое отображается пользователю.</returns>
        public void Run(ITelegramBotClient botClient, Update update);
    }
}
