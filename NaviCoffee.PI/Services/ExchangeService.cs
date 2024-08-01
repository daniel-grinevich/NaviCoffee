using System;
using System.Runtime.CompilerServices;
using EversysApi.DataObject;
using EversysApi.Services;

namespace NaviCoffee.PI.Services
{
    class MyExchangeService : IExchange
    {
        public event EventHandler ApplicationExit;

        public void Debug(string msg, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string file = null)
        {
            Console.WriteLine("[Debug] " + msg);
        }

        public void Error(string msg, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string file = null)
        {
            Console.WriteLine("[Error] " + msg);
        }

        public void Exception(Exception ex, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string file = null)
        {
            Console.WriteLine("[Exception] " + ex);
        }

        public void Info(string msg, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string file = null)
        {
            Console.WriteLine("[Info] " + msg);
        }

        public void Trace(string msg, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string file = null)
        {
            Console.WriteLine("[Trace] " + msg);
        }

        public void Warning(string msg, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string file = null)
        {
            Console.WriteLine("[Warning] " + msg);
        }

        public void CallApplicationExit()
        {
            ApplicationExit?.Invoke(this, new EventArgs());
        }
    }

}
