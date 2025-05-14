using PcController.Data;
using PcController.Interfaces;
using PluginContracts;

namespace PcController.Services
{
    public class PluginManager(string plugin_folder) : IPluginManager
    {
        /// <summary>
        /// Кэшированный список плагинов
        /// </summary>
        private IEnumerable<PluginInfo> _plugins = [];
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public Task Excecute(IPlugin plugin)
        {
            return plugin.Execute();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public IPlugin? FindPlugin(string name)
        {
            return GetAllPlugins().FirstOrDefault(x => string.Equals(name, x.Name));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IPlugin> GetAllPlugins()
        {
            if (!_plugins.Any())
                _plugins = LoadPlugins();

            return _plugins.SelectMany(x => x.Plugin);
        }
        /// <summary>
        /// Загрузить все плагины
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PluginInfo> LoadPlugins()
        {
            if (!Directory.Exists(plugin_folder))
                return [];

            List<PluginInfo> plugins = [];

            foreach (var file in Directory.GetFiles(plugin_folder, "*.dll", SearchOption.AllDirectories))
            {
                var _plugin = LoadPlugin(file);

                if (_plugin != null)
                    plugins.Add(_plugin);
            }

            return plugins;
        }
        /// <summary>
        /// Загрузить плагин по пути
        /// </summary>
        /// <param name="pluginPath"></param>
        /// <returns></returns>
        private PluginInfo? LoadPlugin(string pluginPath)
        {
            var loadContext = new PluginLoadContext(pluginPath);
            var assembly = loadContext.LoadFromAssemblyPath(pluginPath);

            List<IPlugin> plugin = [];

            foreach (var type in assembly.GetTypes())
            {
                var aa = type.GetInterfaces();
                var bb = typeof(IPlugin);
                if (typeof(IPlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                {
                    var _plugin = (IPlugin?)Activator.CreateInstance(type);

                    if (_plugin != null)
                        plugin.Add(_plugin);
                }
            }

            if (plugin.Count != 0)
                return new PluginInfo(plugin, loadContext);
            else
            {
                loadContext.Dispose();
                return null;
            }
        }
    }
}