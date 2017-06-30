using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetsLibrary;
using CefSharp.OffScreen;
using CefSharp;
using HtmlAgilityPack;

namespace BookmakerParser
{
    public class MarathonBookmaker : Bookmaker
    {
        private const string MatchListUrl = "https://www.marathonbet.com/en/live/popular";
        private ChromiumWebBrowser matchListBrowser;
        private Dictionary<string, ChromiumWebBrowser> browserDict = new Dictionary<string, ChromiumWebBrowser>();

        public MarathonBookmaker()
        {
            
        }

        private void LoadMatchListPage()
        {
            matchListBrowser = new ChromiumWebBrowser(MatchListUrl);
        }

        public override void Parse()
        {
            if (matchListBrowser == null) LoadMatchListPage();
            if (!matchListBrowser.IsBrowserInitialized || matchListBrowser.IsLoading) return;
            var activeMatchList = ParseMatchList();
            DeleteNotActiveMatch(activeMatchList);

            foreach (var match in browserDict)
                ParseMatch(match.Value);

        }

        public void DeleteNotActiveMatch(List<string> activeMatchList)
        {
            var notActiveMatchArray = browserDict.Where(e => !activeMatchList.Contains(e.Key)).Select(e => e.Key).ToArray();

            foreach (var key in notActiveMatchArray)
                browserDict.Remove(key);
        }

        public List<string> ParseMatchList()
        {/*
            Cef.UIThreadTaskFactory.StartNew(delegate {
                var rc = this.matchListBrowser.GetBrowser().GetHost().RequestContext;
                var v = new Dictionary<string, object>();
                v["mode"] = "fixed_servers";
                v["server"] = "scheme://host:port";
                string error;
                bool success = rc.SetPreference("proxy", v, out error);
                //success=true,error=""
            });*/

            string html = matchListBrowser.GetSourceAsync().Result;
            List<string> activeMatchList = new List<string>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection matchNodes = doc.DocumentNode.SelectNodes("//div[@class='event-info']");

            int count = 0;
            foreach(var node in matchNodes)
            {
                string id = node.Attributes["data-available-event-treeid"].Value;
                activeMatchList.Add(id);

                if (!browserDict.ContainsKey(id))
                {
                    string url = "https://www.marathonbet.com/en/live/" + id;
                    Console.WriteLine(url);
                    browserDict.Add(id, new ChromiumWebBrowser(url));
                }
                count++;
                if (count > 10) break;
            }

            return activeMatchList;
            
        }
        
        private void ParseMatch(ChromiumWebBrowser browser)
        {
            if (!browser.IsBrowserInitialized ||  browser.IsLoading) return;
            browser.GetSourceAsync().ContinueWith(e =>
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(e.Result);

                Sport sport = GetSport(doc);
                if (sport == Sport.NotSupported) return;

                MatchName matchName = GetMatchName(doc);

                HtmlNodeCollection betsNodes = doc.DocumentNode.SelectNodes("//td");

                foreach (var node in betsNodes)
                {
                    HtmlAttribute attribute = node.Attributes["data-sel"];
                    if (attribute == null) continue;
                    string value = attribute.Value.Replace("&quot;", string.Empty);

                    string coeff = value.Split(new string[] { ",epr:", ",prices:{" }, StringSplitOptions.RemoveEmptyEntries)[1];


                    Console.WriteLine(value + " " + coeff + sport);
                }
            });

            MatchName GetMatchName(HtmlDocument doc)
            {
                string matchName = string.Empty;

                foreach(var node in doc.DocumentNode.SelectNodes("//tbody"))
                    if (node.Attributes["data-event-name"] != null) { matchName = node.Attributes["data-event-name"].Value; break; }

                var matchNameSplit = matchName.Split(new string[] { " vs " }, StringSplitOptions.RemoveEmptyEntries);

                return new MatchName(matchNameSplit[0], matchNameSplit[1]);
            }

            Sport GetSport(HtmlDocument doc)
            {
                string sport = doc.DocumentNode.SelectNodes("//a[@class='sport-category-label']").First().InnerText;

                switch (sport)
                {
                    case "Football": return Sport.Football;
                    case "Basketball": return Sport.Basketball;
                    case "Baseball": return Sport.Baseball;
                    case "Tennis": return Sport.Tennis;
                    case "Ice Hockey": return Sport.IceHockey;
                    case "Volleyball": return Sport.Volleyball;
                }

                return Sport.NotSupported;
            }
        }
        




      
    }
}
