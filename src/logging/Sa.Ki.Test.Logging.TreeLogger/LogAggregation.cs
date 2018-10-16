namespace Sa.Ki.Test.Logging.TreeLogger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LogAggregation : LogItem
    {
        public List<ILogItem> LogItems { get; set; }
    }
}
