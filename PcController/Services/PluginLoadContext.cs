using System.Reflection;
using System.Runtime.Loader;

namespace PcController.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path">Путь к плагину/param>
    public class PluginLoadContext(string path) : AssemblyLoadContext(isCollectible: true), IDisposable
    {
        private readonly AssemblyDependencyResolver _resolver = new(path);

        public void Dispose()
        {
            try
            {
                Unload();
            }
            catch { }
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            string? assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }
    }
}