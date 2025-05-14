using PcController.Services;
using PluginContracts;

namespace PcController.Data
{
    /// <summary>
    /// Хранилище плагина
    /// </summary>
    public class PluginInfo : IDisposable
    {
        public IPlugin Plugin { get; private set; }

        public PluginLoadContext LoadContext { get; private set; }

        public PluginInfo(IPlugin plugin, PluginLoadContext pluginLoadContext)
        {
            Plugin = plugin;
            LoadContext = pluginLoadContext;
        }

        public void Dispose()
        {
            LoadContext.Dispose();
        }
    }
}