namespace PluginContracts
{
    /// <summary>
    /// Интерйфес плагина
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Имя плагина
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Описание плагина
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Выполнить плагин
        /// </summary>
        Task<ResultInfo> Execute();
    }
}