namespace Sa.Ki.Test.Logging.TreeLogger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LogRoot : LogAggregation
    {
        public DateTime FinishTimestamp { get; set; }

        public List<LoggedError> Errors { get; set; }
    }
}
