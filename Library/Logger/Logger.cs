using NLog;

namespace Library
{
    public class Logger : ILogger
    {
        private readonly NLog.Logger _logger;

        public Logger()
        {
            _logger = LogManager.GetLogger("Generic");
        }

        public void LogError(string error)
        {
            _logger.Error(error);
        }

        public void LogInfo(string info)
        {
            _logger.Info(info);
        }

        public void LogWarning(string warning)
        {
            _logger.Warn(warning);
        }
    }

    public interface ILogger
    {
        void LogInfo(string info);
        void LogWarning(string warning);
        void LogError(string error);
    }
}
