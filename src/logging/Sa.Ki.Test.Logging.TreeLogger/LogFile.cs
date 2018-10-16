namespace Sa.Ki.Test.Logging.TreeLogger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LogFile : LogItem
    {
        public byte[] FileBytes { get; set; }
        public string Extension { get; set; }
    }
}
