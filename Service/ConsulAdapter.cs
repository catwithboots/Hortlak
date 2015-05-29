using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Consul;

namespace Service
{
    public class ConsulAdapter
    {
        private readonly string _appname;
        private readonly int _apport;
        private readonly string _appadress;
        private readonly string _spath;
        private readonly int _limit;
        private readonly Client _client;
        private readonly Consul.Semaphore _semaphore;
        private CancellationTokenSource _cts;

        public ConsulAdapter(string ApplicationName, int ApplicationPort, string ApplicationIp, int OwnerLimit)
        {
            _appname = ApplicationName;
            _apport = ApplicationPort;
            _appadress = ApplicationIp;
            _spath = string.Concat(_appname, "/Semaphore"); // What to do with applications with the same name?
            _limit = OwnerLimit;

            try
            {
                _client = new Client();
                _semaphore = _client.Semaphore(_spath, _limit);
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not create a client session to consul: {0}", e);
                Console.ReadLine();
            }
        }

        public void Start()
        {
            _cts = new CancellationTokenSource();
            RegisterService();
            AcquireSemaphore(_cts);
        }

        public void Stop()
        {
            ReleaseSemophore();
            DestroySemaphore();
            DeRegisterService();
        }

        public bool SemaphoreActive()
        {
            return _semaphore.IsHeld;
        }

        public void AcquireSemaphore(CancellationTokenSource ctsTokenSource)
        {
            if (!_semaphore.IsHeld)
            {
                if (ctsTokenSource != null)
                _semaphore.Acquire(ctsTokenSource.Token);
            }
        }

        public void ReleaseSemophore()
        {
            if (_semaphore.IsHeld && _semaphore != null)
                _semaphore.Release();
        }

        public void DestroySemaphore()
        {
            try
            {
                if (!_semaphore.IsHeld && _semaphore != null)
                    _semaphore.Destroy();
            }
            catch (SemaphoreInUseException) {}
        }

        public void DeRegisterService()
        {
            // Only if not in use !!! <-<-<-
            
            _client.Agent.ServiceDeregister(_appname);
        }

        public void RegisterService()
        {
            // Only if not already existing
            
            _client.Agent.ServiceRegister(CreateService());
        }

        public AgentServiceRegistration CreateService()
        {
            AgentServiceRegistration serviceRegistration = new AgentServiceRegistration
            {
                Name = _appname
                //Port = _apport,
                //Address = _appadress
                //Check = CheckTtl()
            };

            return serviceRegistration;
        }

        public AgentServiceCheck CheckTtl()
        {
            var check = new AgentServiceCheck
            {
                TTL = TimeSpan.FromSeconds(5)
            };

            return check;
        }
    }
}
