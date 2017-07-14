using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace BetsLibrary
{
    public abstract class BookmakerParser
    {
        public List<Bet> BetList = new List<Bet>();
        public Dictionary<MatchName, string> MatchDict = new Dictionary<MatchName, string>();
        

        public abstract void Parse();
        public abstract void ParseBets(List<MatchName> matches);
        public abstract void ParseMatchPageHtml(string html, string url);
        public abstract void ParseMatchPageHtml(HtmlDocument doc, string url);

        public List<MatchName> GetMatchList()
        {
            List<MatchName> result = new List<MatchName>();

            foreach (var bet in BetList)
                if (!result.Contains(bet.MatchName)) result.Add(bet.MatchName);

            return result;
        }

    }
}
