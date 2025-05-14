using PcController.Data;
using PluginContracts;

namespace PcController.Interfaces
{
    public interface IPluginManager
    {
        /// <summary>
        /// Получить все плагины
        /// </summary>
        /// <returns></returns>
        IEnumerable<PluginData> GetAllPlugins();
        /// <summary>
        /// Найти плагин
        /// </summary>
        /// <param name="name">Имя плагина <see cref="PluginData.Name"/></param>
        /// <returns></returns>
        PluginData? FindPlugin(string name);
        /// <summary>
        /// Запустить плагин
        /// </summary>
        /// <param name="plugin"></param>
        /// <returns></returns>
        Task<ResultInfo> Execute(PluginData plugin);
    }
}