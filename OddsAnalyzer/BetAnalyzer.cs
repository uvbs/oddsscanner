using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetsLibrary;

namespace OddsAnalyzer
{
    class BetAnalyzer
    {
        private List<Bet> bets = new List<Bet>();

        public void AddBet(Bet bet) => bets.Add(bet);

        public List<ArbitrageBet> GetArbitrageBets()
        {
            List<ArbitrageBet> result = new List<ArbitrageBet>();
            double average = bets.Average(item => item.Odds);

            for(int i = 0; i < bets.Count; i++)
                for(int j=0; j < bets.Count; j++)
                {
                    if (i == j) continue;
                    double profitVsAverage = 1 / average - 1 / bets[i].Odds;
                    double profit = 1 / bets[j].Odds - 1 / bets[i].Odds;
                    result.Add(new ArbitrageBet(bets[i], profitVsAverage, profit));
                }

            return result;
        }

    }
}
