using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ArbitrageService;
using System.ServiceModel.Description;

namespace ArbitrageServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost arbitrageServiceHost = null;
            try
            {
                Uri httpBaseAddress = new Uri("http://localhost:49359/ArbitrageService");

                arbitrageServiceHost = new ServiceHost(typeof(ArbitrageBetService), httpBaseAddress);

                BasicHttpBinding binding = new BasicHttpBinding();
                binding.OpenTimeout = new TimeSpan(0, 20, 0);
                binding.CloseTimeout = new TimeSpan(0, 20, 0);
                binding.SendTimeout = new TimeSpan(0, 20, 0);
                binding.ReceiveTimeout = new TimeSpan(0, 20, 0);
                binding.MaxReceivedMessageSize = 2147483647;

                arbitrageServiceHost.Description.Endpoints.Clear();
                arbitrageServiceHost.AddServiceEndpoint(typeof(IArbitrageService), binding, "");

                ServiceMetadataBehavior serviceBehavior = new ServiceMetadataBehavior();
                serviceBehavior.HttpGetEnabled = true;
                arbitrageServiceHost.Description.Behaviors.Add(serviceBehavior);

                arbitrageServiceHost.Open();

                Console.WriteLine("Service is live now at : {0}", httpBaseAddress);
                Console.ReadKey();

            }
            catch (Exception ex)
            {
                arbitrageServiceHost = null;
                Console.WriteLine("There is an issue with ArbitrageService" + ex.Message);
            }

            Console.ReadKey();
        }
    }
}
