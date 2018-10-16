namespace Sa.Ki.Test.Logging.TreeLogger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LogItem : ILogItem
    {
        public LogLevel Level { get; set; }

        public DateTime Timestamp { get; set; }

        public string Message { get; set; }
    }
}
