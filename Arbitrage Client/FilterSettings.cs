using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetsLibrary;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

namespace Arbitrage_Client
{
    public static class FilterSettings
    {
        private const string filepath = "filter.set";
        public static List<Bookmaker> Bookmakers = new List<Bookmaker>();

        public static List<Sport> Sports = new List<Sport>();

        public static double MinProfit = 0;


        static FilterSettings()
        {
            if (!File.Exists(filepath)) return;
            // read JSON directly from a file
            JObject o1 = JObject.Parse(File.ReadAllText(filepath));
            

            Bookmakers = o1["bookmakers"].ToObject<List<Bookmaker>>() ?? new List<Bookmaker>();
            Sports = o1["sports"].ToObject<List<Sport>>() ?? new List<Sport>();
            MinProfit = o1["MinProfit"].ToObject<double>();
        }
        

        public static void Save()
        {
            JObject filter = new JObject(
                            new JProperty("bookmakers", Bookmakers),
                            new JProperty("sports", Sports),
                            new JProperty("MinProfit", MinProfit));

            // write JSON directly to a file
            using (StreamWriter file = File.CreateText(filepath))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                filter.WriteTo(writer);
            }
            
        }

        public static string GetJsonString()
        {
            return new JObject(
                          new JProperty("bookmakers", Bookmakers),
                          new JProperty("sports", Sports),
                          new JProperty("MinProfit", MinProfit)).ToString();
        }



    }
}
