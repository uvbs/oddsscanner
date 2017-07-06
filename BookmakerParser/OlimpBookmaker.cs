using System;
using System.Collections.Generic;
using System.Linq;
using BetsLibrary;
using CefSharp.OffScreen;
using CefSharp;
using HtmlAgilityPack;

namespace BookmakerParser
{
    public class OlimpBookmaker : BetsLibrary.BookmakerParser
    {
        private string MatchListUrl = "https://www.olimp.kz/betting";

        private const int MaximumMatches = 15;

        private Dictionary<string, ChromiumWebBrowser> browserDict = new Dictionary<string, ChromiumWebBrowser>();
        List<string> activeMatchList = new List<string>();
        private ChromiumWebBrowser matchListBrowser;
        private const Bookmaker Maker = Bookmaker.Olimp;
        string JavaSelectCode = "Java";
        string[] type_of_sport = { "1", "5", "3", "10" }; // 1- football, 5 - basketball, 3 - tennis, 10 volleyball

        private void LoadMatchListPages()
        {
            matchListBrowser = new ChromiumWebBrowser(MatchListUrl);
        }


        public override void Parse()
        {
            if (matchListBrowser == null)
                LoadMatchListPages();

            if (!matchListBrowser.IsBrowserInitialized || matchListBrowser.IsLoading) return;

            int index = 0;
            activeMatchList = new List<string>();
            while (activeMatchList.Count < MaximumMatches && index < type_of_sport.Length)
            {
                ParseMatchList(index);
                index++;
            }
            BetList = new List<Bet>();
            DeleteNotActiveMatch();
            foreach (var match in browserDict)
            {
                ParseMatch(match.Value);
            }

        }

        public void DeleteNotActiveMatch()
        {
            var notActiveMatchArray = browserDict.Where(e => !activeMatchList.Contains(e.Key)).Select(e => e.Key).ToArray();

            foreach (var key in notActiveMatchArray)
                browserDict.Remove(key);
        }

        public void ParseMatchList(int index)
        {
            var task = matchListBrowser.GetSourceAsync();
            task.Wait(2000);
            if (!task.IsCompleted) return;
            string html = task.Result;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

        //    Console.WriteLine(html);
           
            HtmlNodeCollection matchList = doc.DocumentNode.SelectNodes(string.Format("//tr[@data-sport='{0}']", type_of_sport[index]));


            if (matchList == null) return;
            foreach (var node in matchList)
            {
                HtmlNodeCollection matchNodes = node.SelectNodes(".//a");
                var node2 = matchNodes.First();
                if (matchNodes == null) return;
                var idNode = node2.Attributes["id"];

                var hrefNode = node2.Attributes["href"];
                if (idNode == null || hrefNode == null) continue;

                string id = idNode.Value;
                if (!id.Contains("match_live_name")) continue;

                activeMatchList.Add(id);

                if (!browserDict.ContainsKey(id))
                {
                    string url = "https://www.olimp.kz/" + hrefNode.Value;
                    url = url.Replace("amp;", "");
                    Console.WriteLine(url);
                    browserDict.Add(id, new ChromiumWebBrowser(url));
                    System.Threading.Thread.Sleep(5000);
                }
                if (activeMatchList.Count == MaximumMatches) break;
            }

        }
        
        private void ParseMatch(ChromiumWebBrowser browser)
        {

            if (!browser.IsBrowserInitialized || browser.IsLoading)
                return;

            var task = browser.GetSourceAsync();
            task.Wait(2000);
            if (!task.IsCompleted) return;
            string html = task.Result;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            MatchName matchName = GetMatchName(doc);
            if (matchName == null) return;
            Sport sport = GetSport(doc);
            if (sport == Sport.NotSupported) return;


            string BetUrl = browser.Address;


            Bet result = null;

            HtmlNodeCollection maindocument = doc.DocumentNode.SelectNodes("//span[@class='bet_sel koefs']");

            if (maindocument == null) return;

            foreach (var node in maindocument)
            {
                result = null;
                try
                {
                    string[] betParams = node.Attributes["data-select"].Value.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                    string coeff = node.Attributes[string.Format("data-v{0}", betParams[1])].Value;
                    double odds = Convert.ToDouble(coeff.Replace(".", ","));
                    Time time = GetTime(betParams);
                    if (time == null) continue;
                    
                    if(betParams[1] == "1") // 1, X, 2, 1X, 12, x2
                    {
                        if(betParams[2] == "1" || betParams[2] == "10" || betParams[2] == "13" || betParams[2] == "16") // 1, X, 2
                        {
                            if (betParams[4] == "1") result = new ResultBet(ResultBetType.First, time, odds, matchName, BetUrl, JavaSelectCode, sport, Maker);
                            if( betParams[4] == "2") result = new ResultBet(ResultBetType.Draw, time, odds, matchName, BetUrl, JavaSelectCode, sport, Maker);
                            if (betParams[4] == "3") result = new ResultBet(ResultBetType.Second, time, odds, matchName, BetUrl, JavaSelectCode, sport, Maker);
                        }

                        if (betParams[2] == "2" || betParams[2] == "9" || betParams[2] == "22") // 1, 2 all game
                        {
                            if (betParams[4] == "1") result = new ResultBet(ResultBetType.First, time, odds, matchName, BetUrl, JavaSelectCode, sport, Maker);
                            if (betParams[4] == "2") result = new ResultBet(ResultBetType.Second, time, odds, matchName, BetUrl, JavaSelectCode, sport, Maker);
                        }

                        if(betParams[2] == "3") // 1x, 12, x2 all game
                        {
                            if (betParams[4] == "1") result = new ResultBet(ResultBetType.FirstOrDraw, time, odds, matchName, BetUrl, JavaSelectCode, sport, Maker);
                            if (betParams[4] == "2") result = new ResultBet(ResultBetType.FirstOrSecond, time, odds, matchName, BetUrl, JavaSelectCode, sport, Maker);
                            if (betParams[4] == "3") result = new ResultBet(ResultBetType.SecondOrDraw, time, odds, matchName, BetUrl, JavaSelectCode, sport, Maker);
                        }
                    }

                    if(betParams[1] == "2") // handicap, under
                    {
                        double param = Convert.ToDouble(betParams[3].Replace(".", ","));

                        if(betParams[2] == "4" || betParams[2] == "168" || betParams[2] == "11" || betParams[2] == "14" || betParams[2] == "17") // f1/f2 
                        {
                            HandicapBetType type = betParams[4] == "1" ? HandicapBetType.F1 : HandicapBetType.F2;
                            result = new HandicapBet(type, param, time, odds, matchName, BetUrl, JavaSelectCode, sport, Maker);
                        }

                        if(betParams[2] == "5" || betParams[2] == "12" || betParams[2] == "15" || betParams[2] == "18") // under
                        {
                            result = new TotalBet(TotalBetType.Under, param, time, Team.All, odds, matchName, BetUrl, JavaSelectCode, sport, Maker);
                        }

                        if(betParams[2] == "7") //first team
                        {
                            result = new TotalBet(TotalBetType.Under, param, time, Team.First, odds, matchName, BetUrl, JavaSelectCode, sport, Maker);
                        }

                        if (betParams[2] == "8") //second team
                        {
                            result = new TotalBet(TotalBetType.Under, param, time, Team.Second, odds, matchName, BetUrl, JavaSelectCode, sport, Maker);
                        }

                    }

                    if (betParams[1] == "3") // over
                    {
                        double param = Convert.ToDouble(betParams[3].Replace(".", ","));

                        if (betParams[2] == "5" || betParams[2] == "12" || betParams[2] == "15" || betParams[2] == "18") // over
                        {
                            result = new TotalBet(TotalBetType.Over, param, time, Team.All, odds, matchName, BetUrl, JavaSelectCode, sport, Maker);
                        }

                        if (betParams[2] == "7") //first team
                        {
                            result = new TotalBet(TotalBetType.Over, param, time, Team.First, odds, matchName, BetUrl, JavaSelectCode, sport, Maker);
                        }

                        if (betParams[2] == "8") //second team
                        {
                            result = new TotalBet(TotalBetType.Over, param, time, Team.Second, odds, matchName, BetUrl, JavaSelectCode, sport, Maker);
                        }

                    }

                    if (result != null)
                    {
                        int index = BetList.IndexOf(result);
                        if (index != -1)
                        {
                            BetList[index].ChangeOdds(result.Odds);
                        }
                        else
                            BetList.Add(result);
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }


            }
            #region code
            foreach (var output in BetList)
            {
                Console.WriteLine();
                Console.Write("{0} vs {1}   ", output.MatchName.FirstTeam, output.MatchName.SecondTeam);
                Console.Write("{0} ", output);
                Console.Write("coef: {0}", output.Odds);
            }

            #endregion
            System.Threading.Thread.Sleep(500);
        }



        MatchName GetMatchName(HtmlDocument doc)
        {
            try
            {
                HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//span");

                foreach(var node in nodes)
                {
                    if (node.Attributes["id"] == null || !node.Attributes["id"].Value.Contains("match_live_name")) continue;
                    string name = node.InnerText;
                    string[] split = name.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
                    return new MatchName(split[0], split[1]);
                }

                return null;
            }
            catch { return null; }
        }

        Sport GetSport(HtmlDocument doc)
        {
            try
            {
                HtmlNodeCollection node = doc.DocumentNode.SelectNodes("//td[@class='smwndcap']");
                string sport = node.First().InnerText;

                if (sport.Contains("Soccer")) return Sport.Football;
                if (sport.Contains("Basketball")) return Sport.Basketball;
                if (sport.Contains("Tennis")) return Sport.Tennis;
                if (sport.Contains("Volleyball")) return Sport.Volleyball;
                
                return Sport.NotSupported;
            }
            catch
            {
                return Sport.NotSupported;
            }
        }

        Time GetTime(string[] data)
        {

            int data2 = Convert.ToInt32(data[2]);
            if (data2 < 9 || data2 == 168) return new Time(TimeType.AllGame);


            TimeType type;
            if (data[7] == "1")
                type = TimeType.Half;
            else
            if (data[7] == "3" || data[7] == "10")
                type = TimeType.Set;
            else
            if (data[7] == "5")
                type = TimeType.Quarter;
            else throw new Exception();

            int value = 0;

            if (data2 == 9 || data2 == 10 || data2 == 11 || data2 == 12) value = 1;
            else
                if (data2 == 13 || data2 == 14 || data2 == 15 || data2 == 22) value = 2;
            else
                if (data2 == 16 || data2 == 17 || data2 == 18) value = 3;
            else return null;

            return new Time(type, value);
        }
        
       
    }



}
