using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using BetsLibrary;
using OddsAnalyzer;
using BookmakerParser;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace ArbitrageService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    public class ArbitrageBetService : IArbitrageService,IDisposable
    {
        ArbitrageFinder finder;
        List<ArbitrageBet> forkList = new List<ArbitrageBet>();
        object lockobj = new object();
        Task task;
        public ArbitrageBetService()
        {/*
            try
            {
                CefSharp.CefSettings settings = new CefSharp.CefSettings();
                settings.LogSeverity = CefSharp.LogSeverity.Error;
               // settings.BrowserSubprocessPath = System.IO.Path.Combine(cefPath, "CefSharp.BrowserSubprocess.exe");
                CefSharp.Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
            }catch(Exception ex) { Console.WriteLine(ex.Message); }
            */

            finder = new ArbitrageFinder();

            Marathonbet marathonbet = new Marathonbet();
        //    LeonBets leon = new LeonBets();
            OlimpBookmaker olimp = new OlimpBookmaker();
            TitanBet titan = new TitanBet();

            finder.AddBookmaker(marathonbet);
           // finder.AddBookmaker(leon);
            finder.AddBookmaker(olimp);
            finder.AddBookmaker(titan);
         //   var newForks = finder.GetForks();

            try
            {
                //Thread thread = new Thread();
                task = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        while (true)
                        {
                           // Console.WriteLine("here");
                            var newForks = finder.GetForks();

                            lock (lockobj)
                            {
                                forkList = newForks;
                            }
                            Thread.Sleep(4000);
                        }
                    } catch(Exception ex) { Console.WriteLine(ex); }
                }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);

              //  task.Start();
            } catch(Exception ex) { Console.WriteLine(ex.Message); }
           
            task.ContinueWith((e) => Console.WriteLine("closed"));
        }
        
        public void Dispose()
        {
            CefSharp.Cef.Shutdown();
        }

        public string GetArbitrageList(string filter)
        {

            JObject o1 = JObject.Parse(filter);

            List<Bookmaker> Bookmakers = o1["bookmakers"].ToObject<List<Bookmaker>>() ?? new List<Bookmaker>();
            List<Sport> Sports = o1["sports"].ToObject<List<Sport>>() ?? new List<Sport>();
            double MinProfit = o1["MinProfit"].ToObject<double>();

            List<ArbitrageBet> forks;
            Console.WriteLine("Current forks count: {0}", forkList.Count);
          //  Console.WriteLine(task);
            lock (lockobj)
            {
                forks = forkList.Where(bet => Bookmakers.Contains(bet.Bookmaker) && Sports.Contains(bet.Sport) && MinProfit <= bet.Profit).ToList();
            }

            return JsonConvert.SerializeObject(forks, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }
        
    }
}
