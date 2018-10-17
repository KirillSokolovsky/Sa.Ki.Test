namespace Sa.Ki.Test.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class SaKiException : Exception
    {
        public string Type { get; set; }

        public string SubType { get; set; }

        public SaKiException(string type, string message, Exception innerException = null)
            : base(message, innerException)
        {
            Type = type;
        }
    }
}
