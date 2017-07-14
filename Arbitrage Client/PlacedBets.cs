using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetsLibrary;
using System.IO;
using Newtonsoft.Json;

namespace Arbitrage_Client
{
    public static class PlacedBets
    {
        private static Dictionary<string, DateTime> betsDict = new Dictionary<string, DateTime>();
        private const string filePath = "placedbets.dat";
        private static TimeSpan timeToDelete = TimeSpan.FromHours(5);

        static PlacedBets()
        {
            if (!File.Exists(filePath)) return;

            int counter = 0;
            string line;

            // Read the file and display it line by line.
            StreamReader file = new StreamReader(filePath);
            while ((line = file.ReadLine()) != null)
            {
                string[] split = line.Split(new string[] { " @ " }, StringSplitOptions.RemoveEmptyEntries);
                betsDict.Add(split[0], Convert.ToDateTime(split[1]));
                counter++;
            }

            file.Close();
        }

        public static bool Contains(ArbitrageBet bet)
        {
            return betsDict.ContainsKey(BetToString(bet));
        }

        public static void AddBet(ArbitrageBet bet)
        {
            if (betsDict.ContainsKey(BetToString(bet))) return;
            betsDict.Add(BetToString(bet), DateTime.Now);
            DeleteOldBets();
            Save();
        }

        public static void Save()
        {
            StreamWriter file = new StreamWriter(filePath);

            foreach(var pair in betsDict)
                file.WriteLine(pair.Key + " @ " + pair.Value);

            file.Close();
        }

        private static void DeleteOldBets()
        {
            betsDict = betsDict.Where(pair => pair.Value - DateTime.Now < timeToDelete).ToDictionary(p => p.Key, p => p.Value);
        }

        private static string BetToString(ArbitrageBet bet)
        {
            return string.Format("{0} {1} {2}", bet.MainBet.Bookmaker, bet.MainBet, bet.MatchName);
        }
    }
}
