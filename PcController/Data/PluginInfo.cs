using PluginContracts;

namespace PcController.Data
{
    /// <summary>
    /// Хранилище плагина
    /// </summary>
    public class DllInfo
    {
        public IEnumerable<PluginData> Plugin { get; private set; }

        public DllInfo(IEnumerable<PluginData> plugin)
        {
            Plugin = plugin;
        }
    }

    public class PluginData
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string PluginPath { get; private set; }

        internal static PluginData FromPlugin(string pluginPath, string pluginName, string pluginDescription)
        {
            return new PluginData
            {
                Name = pluginName,
                PluginPath = pluginPath,
                Description = pluginDescription
            };
        }
    }
}