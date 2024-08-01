using System;
namespace NaviCoffee.PI.Services
{
    public class LogService
    {
        public LogService()
        {
        }

        public static void LogMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
