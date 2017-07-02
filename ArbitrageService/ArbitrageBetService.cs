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

namespace ArbitrageService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    public class ArbitrageBetService : IArbitrageService
    {
        ArbitrageFinder finder;
        public ArbitrageBetService()
        {
          /*  finder = new ArbitrageFinder();

            Marathonbet marathonbet = new Marathonbet();
            LeonBets leon = new LeonBets();


            finder.AddBookmaker(marathonbet);
            finder.AddBookmaker(leon);*/
            
        }

        public List<ArbitrageBet> GetArbitrageList()
        {
            // return finder.GetArbitrageBets();
            return null;
        }
        
    }
}
