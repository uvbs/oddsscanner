using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetsLibrary;

namespace OddsAnalyzer
{
    public class BetAnalyzer
    {
        private List<Bet> bets = new List<Bet>();

        public void AddBet(Bet bet) => bets.Add(bet);

        public List<ArbitrageBet> GetForks(ArbitrageFinder finder)
        {
            List<ArbitrageBet> result = new List<ArbitrageBet>();
            foreach(var bet in bets)
            {
                foreach(var forkBet in bet.GetForkBets())
                {
                    var possiblesBets = finder.GetBetAnalyzer(forkBet);
                    if (possiblesBets == null) continue;
                    foreach(var possibleBet in possiblesBets.bets)
                    {
                        double profit = 1 - ((1 / bet.Odds) + (1 / possibleBet.Odds));
                        if (profit < -0.03) continue;
                        string type = string.Format("{0} - {1}", bet, possibleBet);
                        
                        var arbitrage = new ArbitrageBet(bet, possibleBet);
                        result.Add(arbitrage);
                    }
                }
            }

            return result;
        }
      
    }
}
