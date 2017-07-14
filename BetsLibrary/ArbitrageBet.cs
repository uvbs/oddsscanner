using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetsLibrary
{
    public class ArbitrageBet
    {
        public Bet MainBet { get; private set; }
        public Bet SecondBet { get; private set; }


        public double Profit => Math.Round(100 - (100 / MainBet.Odds) - (100 / SecondBet.Odds), 2);
        public MatchName MatchName => MainBet.MatchName;
        public Sport Sport => MainBet.Sport;

        public ArbitrageBet(Bet MainBet, Bet SecondBet)
        {
            this.MainBet = MainBet;
            this.SecondBet = SecondBet;
        }

        public override int GetHashCode()
        {
            return Profit.GetHashCode();
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
            return ((bet.MainBet.Equals(MainBet) && bet.MainBet.Bookmaker == MainBet.Bookmaker && bet.MainBet.Odds == MainBet.Odds && bet.SecondBet.Equals(SecondBet) && bet.SecondBet.Bookmaker == SecondBet.Bookmaker && bet.SecondBet.Odds == SecondBet.Odds))
                && bet.Profit == Profit;
        }
        
    }
}
