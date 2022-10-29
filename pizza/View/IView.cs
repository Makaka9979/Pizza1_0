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
