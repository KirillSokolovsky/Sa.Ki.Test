using Sa.Ki.Test.Logging.TreeLogger;
using System;

namespace CoreTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new TreeLogger("test", null);
            Console.WriteLine("Hello World!");
        }
    }
}
