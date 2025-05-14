using System.Diagnostics;

namespace VMStarter
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="vBoxManagePath">Путь к exe файлу VBoxManage</param>
    internal class StartVM(string vBoxManagePath)
    {
        /// <summary>
        /// Запустить виртуальную машину
        /// </summary>
        /// <param name="vmName">Имя ВМ</param>
        /// <returns></returns>
        public string? StartVirtualMachine(string vmName)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = vBoxManagePath,
                    Arguments = $"startvm \"{vmName}\" --type gui",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();

            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                return error;
            }

            return null;
        }
    }
}