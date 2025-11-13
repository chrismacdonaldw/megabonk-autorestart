using MelonLoader;

namespace AutoRestart
{
    public static class ModLogger
    {
        private static readonly MelonLogger.Instance _logger = new("AutoRestart");

        public static void Info(string message)
        {
            _logger.Msg(message);
        }

        public static void Debug(string message)
        {
            if (Config.DebugLogging)
            {
                _logger.Msg($"[DEBUG] {message}");
            }
        }

        public static void Error(string message)
        {
            _logger.Error(message);
        }
    }
}