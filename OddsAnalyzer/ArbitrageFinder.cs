using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetsLibrary;

namespace OddsAnalyzer
{
    public class ArbitrageFinder
    {
        private DateTime LastRefresh; 
        private Dictionary<Bet, BetAnalyzer> analyzerDict = new Dictionary<Bet, BetAnalyzer>();
        private List<BookmakerParser> bookmakerList = new List<BookmakerParser>();

        public void AddBookmaker(BookmakerParser bookmaker)
        {
            bookmakerList.Add(bookmaker);
        }

        public void Refresh()
        {
            analyzerDict = new Dictionary<Bet, BetAnalyzer>();
            foreach (var bookmaker in bookmakerList)
            {
                bookmaker.Parse();
                var betList = bookmaker.GetBetList();
                foreach(var bet in betList)
                {
                    if (!analyzerDict.TryGetValue(bet, out BetAnalyzer betAnalyzer))
                    {
                        betAnalyzer = new BetAnalyzer();
                        analyzerDict.Add(bet, betAnalyzer);
                    }
                    betAnalyzer.AddBet(bet);
                }
            }

            LastRefresh = DateTime.Now;
        }

        public List<ArbitrageBet> GetArbitrageBets()
        {
            Console.WriteLine("1");
            Refresh();
            List<ArbitrageBet> result = new List<ArbitrageBet>();
            foreach (var analyzer in analyzerDict)
            {
                var betsList = analyzer.Value.GetArbitrageBets();
                foreach (var bet in betsList)
                    result.Add(bet);
            }

            return result;
        }


    }
}
