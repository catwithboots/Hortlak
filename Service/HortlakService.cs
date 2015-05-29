using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace Service
{
    public partial class HortlakService : ServiceBase
    {
        public HortlakService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string port = "8000";

            string config = ConfigurationManager.AppSettings["HortlakListenerPort"];

            if (!String.IsNullOrEmpty(config))
            {
                port = ConfigurationManager.AppSettings["HortlakListenerPort"];
            }
            
            string baseAddress = String.Format("http://*:{0}/", port);

            // Start OWIN host 
            WebApp.Start<Startup>(url: baseAddress);
                
            Console.WriteLine("Hortlak API Server running at {0}", baseAddress);
            Console.WriteLine("OS: {0}", Environment.OSVersion);

        }

        protected override void OnStop()
        {
        }

        /// <summary>
        /// Starts the service as console application when running interactively.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void StartConsole(string[] args)
        {
            OnStart(args);
        }
        /// <summary>
        /// Stops the service as console application when running interactively.
        /// </summary>
        public void StopConsole()
        {
            OnStop();
        }
    }
}
