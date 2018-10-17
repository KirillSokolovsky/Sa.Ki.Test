namespace Sa.Ki.Test.Logging.TreeLogger
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TreeLogger : ILogger
    {
        private readonly TreeLogger parentLogger = null;

        public TreeLogger(string message, TreeLogger parentLogger)
        {
            this.parentLogger = parentLogger;
            this.RootItem = new LogAggregation
            {
                Message = message,
                LogItems = new List<ILogItem>(),
                Timestamp = DateTime.UtcNow
            };
        }

        public event EventHandler<ILogItem> OnLogItemAdded;

        public event EventHandler<ILogItem> OnAnyLogItemAdded;

        public event EventHandler<LoggedError> OnErrorLogged;

        public event EventHandler<LoggedError> OnAnyErrorLogged;

        public LogAggregation RootItem { get; set; }

        public string Name => this.RootItem.Message;

        public ILogger CreateChildLogger(string message)
        {
            var innerLogger = new TreeLogger(message, this);

            innerLogger.OnAnyLogItemAdded += this.OnAnyLogItemAdded;
            innerLogger.OnAnyErrorLogged += this.OnAnyErrorLogged;

            this.AddItem(innerLogger.RootItem);
            return innerLogger;
        }

        public void ERROR(string message, Exception exception = null)
        {
            this.AddMessage(LogLevel.ERROR, message, exception);
        }

        public void INFO(string message)
        {
            this.AddMessage(LogLevel.INFO, message);
        }

        public void FILE(string message, byte[] fileBytes, string extension, bool isError)
        {
            var fileItem = new LogFile
            {
                Level = isError ? LogLevel.ERROR : LogLevel.INFO,
                Message = message,
                Timestamp = DateTime.UtcNow,
                FileBytes = fileBytes,
                Extension = extension
            };
            this.AddItem(fileItem);
        }

        public void ITEM(ILogItem logItem)
        {
            AddItem(logItem);
        }

        private void AddMessage(LogLevel level, string message, Exception exception = null)
        {
            var messageItem = new LogMessage
            {
                Level = level,
                Message = message,
                Error = exception == null
                    ? null
                    : new LoggedException(exception),
                Timestamp = DateTime.UtcNow
            };
            this.AddItem(messageItem);
        }

        private void AddItem(ILogItem logItem)
        {
            this.RootItem.LogItems.Add(logItem);

            if (logItem.Level == LogLevel.ERROR)
            {
                var loggedError = new LoggedError
                {
                    LogItemWithError = logItem,
                    PathList = new List<string> { Name }
                };

                BubleLoggedError(loggedError);

                OnErrorLogged?.Invoke(this, loggedError);
                OnAnyErrorLogged?.Invoke(this, loggedError);
            }

            this.OnAnyLogItemAdded?.Invoke(this, logItem);
            this.OnLogItemAdded?.Invoke(this, logItem);
        }

        private void BubleLoggedError(LoggedError loggedError)
        {
            if (parentLogger != null)
            {
                loggedError.PathList.Insert(0, parentLogger.Name);
                parentLogger.BubleLoggedError(loggedError);
                return;
            }

            if (RootItem is LogRoot logRoot)
            {
                if (logRoot.Errors == null)
                    logRoot.Errors = new List<LoggedError>();

                logRoot.Errors.Add(loggedError);
            }
        }
    }
}
