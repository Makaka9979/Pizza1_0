using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Libs
{
    /// <summary>
    /// Статический класс,
    /// который связывает все контроллеры с действиями,
    /// которые они обрабатывают.
    /// </summary>
    public static class ControllerRegistry
    {
        private static Dictionary<string, IController> _controllerRegistry = new Dictionary<string, IController>();

        /// <summary>
        /// Регистрирует контроллер.
        /// </summary>
        /// <remarks>
        /// Этот метод может зарегистрировать любой объект,
        /// класс которого реализует интерфейс IController.
        /// </remarks>
        /// <param name="controllerName">Ключ за которым регистрируется контроллер,
        /// также, является действием, которое этот контроллер обрабатывает.</param>
        /// <param name="controller">Экземпляр класса который реализует интерфейс IController.</param>
        public static void Add(string controllerName, IController controller)
        {
            // Добавляем контроллер в словарь контроллеров
            _controllerRegistry.Add(controllerName, controller);
        }

        /// <summary>
        /// Возвращает контроллер за именем/действием.
        /// </summary>
        /// <param name="controllerName">Имя контроллера/Действие, которое контроллер обрабатывает.</param>
        /// <returns>Объект реализующий интерфейс IController или null.</returns>
        public static IController? Get(string controllerName)
        {
            // Получаем контроллер из словаря контроллеров
            return _controllerRegistry.GetValueOrDefault(controllerName);
        }
    }
}
