using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var service = new HortlakService();
            if (Environment.UserInteractive)
            {
                service.StartConsole(args);
                Console.WriteLine("Press any key to stop program");
                Console.Read();
                service.StopConsole();
            }
            else
            {
                ServiceBase.Run(service);    
            }

        }
    }
}
