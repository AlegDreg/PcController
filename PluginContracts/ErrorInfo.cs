namespace PluginContracts
{
    public class ErrorInfo
    {
        public string Text { get; private set; }

        public ErrorInfo(string message)
        {
            Text = message;
        }
    }
}