using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetsLibrary
{
    public abstract class Bookmaker
    {
        protected List<Match> MatchList = new List<Match>();

        public abstract void Parse(); 

    }
}
