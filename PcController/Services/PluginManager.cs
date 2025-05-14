using PcController.Data;
using PcController.Interfaces;
using PluginContracts;
using System.Reflection;

namespace PcController.Services
{
    public class PluginManager : IPluginManager
    {
        private readonly string plugin_folder;

        /// <summary>
        /// Кэшированный список плагинов
        /// </summary>
        private IEnumerable<DllInfo> _plugins = [];
        private readonly FileSystemWatcher fileWatcher;

        public PluginManager(string plugin_folder)
        {
            this.plugin_folder = plugin_folder;

            fileWatcher = new FileSystemWatcher(plugin_folder, "*.dll")
            {
                NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size,
                IncludeSubdirectories = true,
                EnableRaisingEvents = true
            };
     
            fileWatcher.Created += FileWatcher_Created;
            fileWatcher.Changed += FileWatcher_Created;
            fileWatcher.Deleted += FileWatcher_Created;
            fileWatcher.Renamed += FileWatcher_Created;

        }
        /// <summary>
        /// Что-то изменилось в папке plugins
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            _plugins = LoadPlugins();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public async Task<ResultInfo> Execute(PluginData plugin)
        {
            using var loadContext = new PluginLoadContext(plugin.PluginPath);

            var assembly = loadContext.LoadFromAssemblyPath(plugin.PluginPath);

            var _loadedPlugin = LoadAssembly(plugin.PluginPath, assembly).FirstOrDefault(x => x.Name == plugin.Name);

            if (_loadedPlugin == null)
                return await Task.FromResult(new ResultInfo { Error = new ErrorInfo("Плагин не найден") });
            else
                return await _loadedPlugin.Execute();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public PluginData? FindPlugin(string name)
        {
            return GetAllPlugins().FirstOrDefault(x => string.Equals(name, x.Name));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PluginData> GetAllPlugins()
        {
            if (!_plugins.Any())
                _plugins = LoadPlugins();

            return _plugins.SelectMany(x => x.Plugin);
        }
        /// <summary>
        /// Загрузить все плагины
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DllInfo> LoadPlugins()
        {
            if (!Directory.Exists(plugin_folder))
                return [];

            List<DllInfo> dlls = [];

            foreach (var file in Directory.GetFiles(plugin_folder, "*.dll", SearchOption.AllDirectories))
            {
                var dllInfo = LoadDll(file);

                if (dllInfo != null)
                    dlls.Add(dllInfo);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            return dlls;
        }
        /// <summary>
        /// Загрузить плагин по пути
        /// </summary>
        /// <param name="pluginPath"></param>
        /// <returns></returns>
        private DllInfo? LoadDll(string pluginPath)
        {
            using var loadContext = new PluginLoadContext(pluginPath);

            var assembly = loadContext.LoadFromAssemblyPath(pluginPath);

            List<IPlugin> allPlugins = LoadAssembly(pluginPath, assembly).ToList();

            List<PluginData> plugin = [];

            foreach (var x in allPlugins)
            {
                plugin.Add(PluginData.FromPlugin(pluginPath, x.Name, x.Description));
            }

            if (plugin.Count != 0)
                return new DllInfo(plugin);
            else
                return null;
        }

        private IEnumerable<IPlugin> LoadAssembly(string pluginPath, Assembly assembly)
        {
            List<IPlugin> pl = [];

            foreach (var type in assembly.GetTypes())
            {
                if (typeof(IPlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                {
                    var _plugin = (IPlugin?)Activator.CreateInstance(type);

                    if (_plugin == null)
                        continue;

                    pl.Add(_plugin);
                }
            }

            return pl;
        }
    }
}