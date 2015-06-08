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
        private ConsulAdapter consulAdapter;

        public HortlakService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string applicationName = "Hortlak";
            var port = GetListenerPort();
            var ip = "+";
            var limit = 2;

            // Are we using consul
            if (UseConsul())
            {
                DoConsulMagic(applicationName, Convert.ToInt16(port), ip, limit);
            }
            
            // Build the address
            string baseAddress = String.Format("http://*:{0}/", port);

            // Start OWIN host 
            StartOwin(baseAddress);

        }

        protected override void OnStop()
        {
            if (UseConsul())
            {
                UndoConsulMagic();
            }
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

        protected bool UseConsul()
        {
            if (ConfigurationManager.AppSettings["UseConsul"] == "true")
            {
                return true;
            }

            return false;
        }

        protected string GetListenerPort()
        {
            // If noting is configured, port 8000 will be used
            string port = "8000";

            string config = ConfigurationManager.AppSettings["HortlakListenerPort"];

            if (!String.IsNullOrEmpty(config))
            {
                port = ConfigurationManager.AppSettings["HortlakListenerPort"];
            }

            return port;
        }

        protected void StartOwin(string baseaddress)
        {
            WebApp.Start<Startup>(url: baseaddress);

            Console.WriteLine("Hortlak API Server running at {0}", baseaddress);
            Console.WriteLine("OS: {0}", Environment.OSVersion);
        }

        protected void DoConsulMagic(string ApplicationName, int ApplicationPort, string ApplicationIp, int OwnerLimit)
        {
            consulAdapter = new ConsulAdapter(ApplicationName, ApplicationPort, ApplicationIp, OwnerLimit);

            try
            {
                consulAdapter.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to register via the Consul agent. {0}", e);                    
            }
            
        }

        protected void UndoConsulMagic()
        {
            consulAdapter.Stop();
        }
    }
}
