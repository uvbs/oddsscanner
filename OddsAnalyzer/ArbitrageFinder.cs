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
        private Dictionary<Bet, BetAnalyzer> analyzerDict = new Dictionary<Bet, BetAnalyzer>();
        private List<BookmakerParser> bookmakerList = new List<BookmakerParser>();
        public List<ArbitrageBet> ArbitrageBets = new List<ArbitrageBet>();

        public void AddBookmaker(BookmakerParser bookmaker)
        {
            bookmakerList.Add(bookmaker);
        }
        /*
        public void Refresh()
        {
            analyzerDict = new Dictionary<Bet, BetAnalyzer>();
            var tasks = new List<Task>();

            List<MatchName> matches = new List<MatchName>();

            foreach (var bookmaker in bookmakerList)
            {
                bookmaker.Parse();
                matches.AddRange(bookmaker.MatchDict.Keys);
            }

            var filteredMatches = matches.Where(name =>
            {
                int count = matches.Where(match => match.Equals(name)).Count();
                return count > 1;
            }).ToList();

            Console.WriteLine("filtered match: " + filteredMatches.Count);

            foreach (var bookmaker in bookmakerList)
            {
               tasks.Add(Task.Factory.StartNew(() => bookmaker.ParseBets(filteredMatches)));
            }
            Task.WaitAll(tasks.ToArray());
            

    }*/

        public List<ArbitrageBet> GetForks()
        {
            List<ArbitrageBet> result = new List<ArbitrageBet>();
            analyzerDict = new Dictionary<Bet, BetAnalyzer>();
            foreach (var bookmaker in bookmakerList)
            {
                foreach (var bet in bookmaker.BetList.ToList())
                {
                    if (!analyzerDict.TryGetValue(bet, out BetAnalyzer betAnalyzer))
                    {
                        betAnalyzer = new BetAnalyzer();
                        analyzerDict.Add(bet, betAnalyzer);
                    }
                    betAnalyzer.AddBet(bet);
                }
            }



            foreach (var analyzer in analyzerDict)
                result.AddRange(analyzer.Value.GetForks(this));

            return result;
        }

        public BetAnalyzer GetBetAnalyzer(Bet bet)
        {
            analyzerDict.TryGetValue(bet, out BetAnalyzer result);
            return result;
        }


    }
}
