using PluginContracts;
using System.Reflection;

namespace VMStarter
{
    public class Plugin : IPlugin
    {
        public string Name => "work_vm";

        public string Description => "Запуск рабочей виртуальной машины";

        public Task<ResultInfo> Execute()
        {
            string jsonPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "vm_starter_data.json");

            string? json;

            try
            {
                json = File.ReadAllText(jsonPath);
            }
            catch (Exception e)
            {
                return Task.FromResult(new ResultInfo { Error = new ErrorInfo($"Ошибка чтения конфига - {e.Message} -- {e.InnerException}") });
            }


            VmInfo? info;

            try
            {
                info = System.Text.Json.JsonSerializer.Deserialize<VmInfo>(json);
                if (info == null)
                    throw new Exception();
            }
            catch
            {
                return Task.FromResult(new ResultInfo { Error = new ErrorInfo("Ошибка разбора конфига") });
            }

            var vmStarter = new StartVM(info.VBoxManagePath);
            var result = vmStarter.StartVirtualMachine(info.VmName);

            ErrorInfo? error = null;

            if (result != null)
            {
                error = new ErrorInfo(result);
            }

            return Task.FromResult(new ResultInfo { Error = error });
        }
    }

    internal class VmInfo
    {
        /// <summary>
        /// Путь к файлу VBoxManage
        /// </summary>
        public string VBoxManagePath { get; set; }
        /// <summary>
        /// Имя виртуалки
        /// </summary>
        public string VmName { get; set; }
    }
}