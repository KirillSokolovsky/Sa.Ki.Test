namespace Sa.Ki.Test.Logging.TreeLogger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LoggedException
    {
        public static string ErrorNamespaceForStackTrace { get; set; }

        public string Message { get; set; }
        public string FullText { get; set; }

        public LoggedException InnerException { get; set; }

        public string StackTrace { get; set; }
        public string FullStackTrace { get; set; }

        public LoggedException()
        {

        }

        public LoggedException(Exception exception)
        {
            Message = exception.Message;
            FullText = exception.ToString();

            var cur = exception;
            while (cur != null)
            {
                if (cur.StackTrace != null)
                    FullText = FullText.Replace(cur.StackTrace, "");
                cur = cur.InnerException;
            }

            if (exception.InnerException != null)
                InnerException = new LoggedException(exception.InnerException);
            
            FullStackTrace = exception.StackTrace;
            if (FullStackTrace != null)
            {
                var stLines = FullStackTrace.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                var st = stLines.Where(l => l.Contains(ErrorNamespaceForStackTrace))
                    .ToList();
                StackTrace = string.Join(Environment.NewLine, st);
            }
        }
    }
}
