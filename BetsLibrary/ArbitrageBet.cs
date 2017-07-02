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
        public Sport Sport => Bet.Sport;

        public ArbitrageBet(Bet Bet, double ProfitVsAverage, double Profit)
        {
            this.Bet = Bet;
            this.ProfitVsAverage = Math.Round(ProfitVsAverage * 100, 2);
            this.Profit = Math.Round(Profit * 100, 2);
        }

        public override int GetHashCode()
        {
            return Bet.GetHashCode() ^ Profit.GetHashCode() ^ ProfitVsAverage.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is ArbitrageBet bet)
            {
                return Equals(bet);
            }
            else return false;
        }

        public bool Equals(ArbitrageBet bet)
        {
            return bet.Bet == Bet && bet.Bookmaker == Bookmaker && bet.Coeff == Coeff && bet.Coeff == Coeff && bet.Profit == Profit && bet.ProfitVsAverage == ProfitVsAverage;
        }
    }
}
