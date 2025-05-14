using PluginContracts;

namespace PcController.Interfaces
{
    public interface IPluginManager
    {
        /// <summary>
        /// Получить все плагины
        /// </summary>
        /// <returns></returns>
        IEnumerable<IPlugin> GetAllPlugins();
        /// <summary>
        /// Найти плагин
        /// </summary>
        /// <param name="name">Имя плагина <see cref="IPlugin.Name"/></param>
        /// <returns></returns>
        IPlugin? FindPlugin(string name);
        /// <summary>
        /// Запустить плагин
        /// </summary>
        /// <param name="plugin"></param>
        /// <returns></returns>
        Task Excecute(IPlugin plugin);
    }
}