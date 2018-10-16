namespace Sa.Ki.Test.Logging.TreeLogger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LogImagesDiff : LogItem
    {
        public LogFile WhatImage { get; set; }
        public LogFile WithImage { get; set; }
        public LogFile DiffMaskImage { get; set; }
        public double DiffRatio { get; set; }
    }
}
