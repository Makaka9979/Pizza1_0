using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View
{
    /// <summary>
    /// Интерфейс, который должны реализовывать все представления
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Отображает представление.
        /// </summary>
        public void Render();
    }
}
