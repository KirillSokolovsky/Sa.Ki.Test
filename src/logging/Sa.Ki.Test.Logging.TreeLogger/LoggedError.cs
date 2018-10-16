namespace Sa.Ki.Test.Logging.TreeLogger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LoggedError
    {
        public ILogItem LogItemWithError { get; set; }
        public List<string> PathList { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Error on step: {string.Join(" -> ", PathList)}");
            if (LogItemWithError is LogMessage lm)
            {
                sb.AppendLine($"Error:{Environment.NewLine}\t\t");
                sb.AppendLine($"{lm.Message}");
                if(lm.Error != null)
                    sb.AppendLine($"{Environment.NewLine}\t\t{lm.Error.Message}");
            }
            return sb.ToString();
        }
    }
}
