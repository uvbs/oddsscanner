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
        public string ForkType { get; private set; }
        public double Profit { get; private set; }

        public Bookmaker Bookmaker => Bet.Bookmaker;
        public MatchName MatchName => Bet.MatchName;
        public double Coeff => Bet.Odds;
        public Sport Sport => Bet.Sport;

        public ArbitrageBet(Bet Bet, string ForkType, double Profit)
        {
            this.Bet = Bet;
            this.ForkType = ForkType;
            this.Profit = Profit;
        }

        public override int GetHashCode()
        {
            return Bet.GetHashCode() ^ Profit.GetHashCode() ^ ForkType.GetHashCode();
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
            return bet.Bet.Equals(Bet) && bet.Bookmaker == Bookmaker && bet.Coeff == Coeff && bet.Coeff == Coeff && bet.Profit == Profit && bet.ForkType == ForkType;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Bet, ForkType, Profit);
        }
    }
}
