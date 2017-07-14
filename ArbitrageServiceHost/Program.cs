using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ArbitrageService;
using System.ServiceModel.Description;
using System.Runtime.Serialization;

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

                BasicHttpBinding binding = new BasicHttpBinding()
                {
                    OpenTimeout = new TimeSpan(0, 20, 0),
                    CloseTimeout = new TimeSpan(0, 20, 0),
                    SendTimeout = new TimeSpan(0, 20, 0),
                    ReceiveTimeout = new TimeSpan(0, 20, 0),
                    MaxBufferPoolSize = 2147483647,
                    MaxBufferSize = 2147483647,
                    MaxReceivedMessageSize = 2147483647
                };
                var quotas = new System.Xml.XmlDictionaryReaderQuotas()
                {
                    MaxDepth = 32,
                    MaxArrayLength = 2147483647,
                    MaxStringContentLength = 2147483647
                };
                binding.ReaderQuotas = quotas;

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
