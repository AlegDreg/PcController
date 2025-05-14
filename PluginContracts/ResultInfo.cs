using System.Diagnostics.CodeAnalysis;

namespace PluginContracts
{
    public class ResultInfo
    {
        /// <summary>
        /// Успешно ли выполнение
        /// </summary>
        [MemberNotNullWhen(false, nameof(Error))]
        public bool IsSuccess => Error == null;
        /// <summary>
        /// Объект с ошибкой выполнения модуля
        /// </summary>
        public ErrorInfo? Error { get; set; }
    }
}