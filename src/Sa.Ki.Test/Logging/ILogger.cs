namespace Sa.Ki.Test.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ILogger
    {
        ILogger CreateChildLogger(string name);

        void INFO(string message);

        void ERROR(string message, Exception exception = null);

        void FILE(string message, byte[] fileBytes, string extension, bool isError);

        void ITEM(ILogItem logItem);
    }
}
