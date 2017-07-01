using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetsLibrary
{
    public abstract class BookmakerParser
    {
        protected List<Bet> BetList = new List<Bet>();

        public IReadOnlyList<Bet> GetBetList() => BetList.AsReadOnly();

        public abstract void Parse(); 

    }
}
