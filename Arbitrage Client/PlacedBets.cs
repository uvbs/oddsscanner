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
        private static Dictionary<ArbitrageBet, DateTime> betsDict = new Dictionary<ArbitrageBet, DateTime>();
        private const string filePath = "placedbets.dat";
        private static TimeSpan timeToDelete = TimeSpan.FromHours(5);

        static PlacedBets()
        {
            if (!File.Exists(filePath)) return;

            JsonSerializer serializer = new JsonSerializer();

            using (StreamReader sw = new StreamReader(filePath))
            using (JsonReader reader = new JsonTextReader(sw))
            {
                betsDict = serializer.Deserialize<Dictionary<ArbitrageBet, DateTime>>(reader);
            }
        }

        public static bool Contains(ArbitrageBet bet)
        {
            var bets = betsDict.Where(placedBet => placedBet.Key.Bet == bet.Bet && placedBet.Key.Bookmaker == bet.Bookmaker);
            return bets.Count() != 0;
        }

        public static void AddBet(ArbitrageBet bet)
        {
            betsDict.Add(bet, DateTime.Now);
            DeleteOldBets();
            Save();
        }

        public static void Save()
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(filePath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, betsDict);
            }
        }

        private static void DeleteOldBets()
        {
            betsDict = betsDict.Where(pair => pair.Value - DateTime.Now < timeToDelete).ToDictionary(p => p.Key, p => p.Value);
        }
    }
}
