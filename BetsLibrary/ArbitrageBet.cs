using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetsLibrary
{
    public class ArbitrageBet
    {
        public Bet Bet { get; private set; }
        public double ProfitVsAverage { get; private set; }
        public double Profit { get; private set; }

        public Bookmaker Bookmaker => Bet.Bookmaker;
        public MatchName MatchName => Bet.MatchName;
        public double Coeff => Bet.Odds;

        public ArbitrageBet(Bet Bet, double ProfitVsAverage, double Profit)
        {
            this.Bet = Bet;
            this.ProfitVsAverage = ProfitVsAverage;
            this.Profit = Profit;
        }
    }
}
