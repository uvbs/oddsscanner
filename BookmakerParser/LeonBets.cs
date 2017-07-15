using System;
using System.Collections.Generic;
using System.Linq;
using BetsLibrary;
using CefSharp.OffScreen;
using CefSharp;
using HtmlAgilityPack;
using System.Threading.Tasks;


namespace BookmakerParser
{
    public class LeonBets : BetsLibrary.BookmakerParser
    {
        private string MatchListUrl = "https://mobile.leonbets.net/mobile/#liveEvents";

        private const int MaximumMatches = 50;
        
        private ChromiumWebBrowser matchListBrowser;
        private const Bookmaker Maker = Bookmaker.Leon;
        string JavaSelectCode = "Java";
        string[] type_of_sport = { "Soccer", "Basketball", "Tennis", "Volleyball" };
        public LeonBets()
        {

        }

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

            MatchDict = new Dictionary<MatchName, string>();

            while (MatchDict.Count < MaximumMatches && index < type_of_sport.Length)
            {
                ParseMatchList(index);
                index++;
            }
        }


        public void ParseMatchList(int index)
        {
            string html = matchListBrowser.GetSourceAsync().Result;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection matchList = doc.DocumentNode.SelectNodes("//li[@class='groupedListItem first' or @class='groupedListItem' or @class='groupedListItem last' or @class='groupedListItem first last']");


            if (matchList == null) return;
            foreach (var node in matchList)
            {
                string All = node.InnerHtml;
                HtmlDocument All_Doc = new HtmlDocument();
                All_Doc.LoadHtml(All);
                //
                HtmlNodeCollection h3 = All_Doc.DocumentNode.SelectNodes("//h3");
                string l = h3.First().InnerText;
                if (l == type_of_sport[index])
                {
                    HtmlNodeCollection matchNodes = All_Doc.DocumentNode.SelectNodes("//li[@class='groupedListSubItem first' or @class='groupedListSubItem last' or @class='groupedListSubItem' or @class='groupedListSubItem first last']");
                    if (matchNodes == null) return;
                    foreach (var node2 in matchNodes)
                    {
                        string id = String.Empty;

                        id = node2.Attributes["id"].Value;
                        id = id.Remove(0, 2);
                        MatchName Name = GetMatchName(node2);

                        string url = "https://mobile.leonbets.net/mobile/#eventDetails/:" + id;

                        MatchDict.Add(Name, url);
                        if (MatchDict.Count == MaximumMatches) break;
                    }
                }
            }

        }
        /*
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

            ParseMatchPageHtml(doc, browser.Address);
        }*/

        public override void ParseMatchPageHtml(HtmlDocument doc, string url)
        {
            MatchName matchName = GetFullMatchName(doc);
            if (matchName == null) return;
            Sport sport = GetSport(doc);
            if (sport == Sport.NotSupported) return;

            string BetUrl = url;

            Bet result = null;

            HtmlNodeCollection maindocument = doc.DocumentNode.SelectNodes("//li[@class='groupedListItem']");

            if (maindocument == null) return;

            foreach (var node in maindocument)
            {
                result = null;
                try
                {
                    string all_main = node.InnerHtml;

                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(all_main);

                    string Way = document.DocumentNode.SelectNodes("//table[@class]").First().Attributes["class"].Value;
                    string maintype = document.DocumentNode.SelectNodes("//h3").First().InnerText;
                    HtmlNodeCollection betsNodes = document.DocumentNode.SelectNodes("//a[@id]");

                    Team team = GetTeam(maintype);
                    Time time = GetTime(maintype);// зробити час
                    foreach (var node2 in betsNodes)
                    {
                        string value = node2.InnerHtml;
                        if (!value.Contains("class=\"oddHolder\"")) continue;
                        HtmlDocument document2 = new HtmlDocument();
                        document2.LoadHtml(value);
                        HtmlNodeCollection test = document2.DocumentNode.SelectNodes("//div");

                        HtmlAttribute idAttribude = node.Attributes["id"];
                        if (idAttribude == null) continue;
                        JavaSelectCode = "(function() { var element = document.evaluate( '" + node2.XPath + "' ,document, null, XPathResult.ANY_TYPE, null ).singleNodeValue;  element.click(); })();";

                        string type = test.First().InnerText;
                        string coeff = test.Last().InnerText;

                        double Probability = Convert.ToDouble(coeff.Replace(".", ","));
                        if (maintype.Contains("1X2"))
                        {
                            if (Way == "list cell2")
                            {
                                if (type == "1")
                                {
                                    result = new ResultBet(ResultBetType.P1, time, Probability, matchName, BetUrl, JavaSelectCode, sport, Maker);
                                }

                                if (type == "2")
                                {
                                    result = new ResultBet(ResultBetType.P2, time, Probability, matchName, BetUrl, JavaSelectCode, sport, Maker);
                                }
                            }
                            if (Way == "list cell3")
                            {
                                if (type == "1")
                                {
                                    result = new ResultBet(ResultBetType.First, time, Probability, matchName, BetUrl, JavaSelectCode, sport, Maker);
                                }
                                if (type == "x" || type == "X")
                                {
                                    result = new ResultBet(ResultBetType.Draw, time, Probability, matchName, BetUrl, JavaSelectCode, sport, Maker);
                                }
                                if (type == "2")
                                {
                                    result = new ResultBet(ResultBetType.Second, time, Probability, matchName, BetUrl, JavaSelectCode, sport, Maker);
                                }
                            }
                        }
                        if (maintype.Contains("Double Chance") || maintype.Contains("Double chance"))// за весь час чи нормальний час брати? Inc All OT
                        {
                            if (type == "1x" || type == "1X")
                            {
                                result = new ResultBet(ResultBetType.FirstOrDraw, time, Probability, matchName, BetUrl, JavaSelectCode, sport, Maker);
                            }
                            if (type == "12")
                            {
                                result = new ResultBet(ResultBetType.FirstOrSecond, time, Probability, matchName, BetUrl, JavaSelectCode, sport, Maker);
                            }
                            if (type == "x2" || type == "X2")
                            {
                                result = new ResultBet(ResultBetType.SecondOrDraw, time, Probability, matchName, BetUrl, JavaSelectCode, sport, Maker);
                            }
                        }
                        // importantly!!!!
                        // You have to see it.
                        if ((maintype.Contains("Total") || maintype.Contains("total")) && (!maintype.Contains("aggregated") && !maintype.Contains("Totals")))
                        {
                            if (sport == Sport.Basketball &&
                             (!maintype.Contains("including overtime") && !maintype.Contains("Including overtime") && !maintype.Contains("including Overtime")))
                                continue;
                            // перевірити правильність написання including overtime(регістр)

                            if (type.Contains("Under"))
                            {
                                try
                                {
                                    double param = Convert.ToDouble(type.Split(new string[] { "Under (", ")" }, StringSplitOptions.RemoveEmptyEntries)[0].Replace(".", ","));

                                    result = new TotalBet(TotalBetType.Under, param, time, team, Probability, matchName, BetUrl, JavaSelectCode, sport, Maker);

                                }
                                catch { }
                            }
                            if (type.Contains("Over"))
                            {
                                double param = Convert.ToDouble(type.Split(new string[] { "Over (", ")" }, StringSplitOptions.RemoveEmptyEntries)[0].Replace(".", ","));

                                result = new TotalBet(TotalBetType.Over, param, time, team, Probability, matchName, BetUrl, JavaSelectCode, sport, Maker);
                            }
                        }
                        if ((maintype.Contains("Handicap") || maintype.Contains("handicap")) && maintype.Contains("Asian"))
                        {
                            if (sport == Sport.Basketball &&
                             (!maintype.Contains("inc. ОТ") && !maintype.Contains("Inc. OT") && !maintype.Contains("INC. OT") && !maintype.Contains("including overtime")))
                                continue;

                            // перевірити правильність написання including overtime(регістр)
                            string first_or_second_team = type.Split(new string[] { " (" }, StringSplitOptions.RemoveEmptyEntries)[0];
                            if (first_or_second_team == "1")
                            {
                                double param = Convert.ToDouble(type.Split(new string[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries)[1].Replace(".", ","));

                                result = new HandicapBet(HandicapBetType.F1, param, time, Probability, matchName, BetUrl, JavaSelectCode, sport, Maker);
                            }
                            if (first_or_second_team == "2")
                            {
                                double param = Convert.ToDouble(type.Split(new string[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries)[1].Replace(".", ","));

                                result = new HandicapBet(HandicapBetType.F2, param, time, Probability, matchName, BetUrl, JavaSelectCode, sport, Maker);
                            }
                        }
                        else
                        if (maintype.Contains("Handicap") && !maintype.Contains("Asian"))
                        {
                            // коли буде матч переглянути як тут пишуть INC.OT. 

                            string first_or_second_team = type.Split(new string[] { " (" }, StringSplitOptions.RemoveEmptyEntries)[0];
                            if (first_or_second_team == "1")
                            {
                                string initial_score = type.Split(new string[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries)[1];
                                int first_number = Convert.ToInt32(initial_score.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0]);
                                int second_number = Convert.ToInt32(initial_score.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[1]);
                                double param = 0;
                                if (first_number != 0)
                                {
                                    param = first_number - 0.5;
                                }
                                if (second_number != 0)
                                {
                                    param = (-1) * (second_number) - 0.5;
                                }
                                result = new HandicapBet(HandicapBetType.F1, param, time, Probability, matchName, BetUrl, JavaSelectCode, sport, Maker);
                            }
                            else
                            if (first_or_second_team == "2")
                            {
                                string initial_score = type.Split(new string[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries)[1];
                                int first_number = Convert.ToInt32(initial_score.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0]);
                                int second_number = Convert.ToInt32(initial_score.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[1]);
                                double param = 0;
                                if (first_number != 0)
                                {
                                    param = (-1) * first_number - 0.5;
                                }
                                if (second_number != 0)
                                {
                                    param = second_number - 0.5;
                                }
                                result = new HandicapBet(HandicapBetType.F2, param, time, Probability, matchName, BetUrl, JavaSelectCode, sport, Maker);
                            }
                        }
                        else
                        if (maintype.Contains("Draw No Bet"))
                        {
                            if (sport == Sport.Basketball &&
                             (!maintype.Contains("inc. ОТ") && !maintype.Contains("Inc. OT") && !maintype.Contains("INC. OT") && !maintype.Contains("including overtime")))
                                continue;

                            double param = 0;
                            if (type.Contains("1"))
                            {
                                result = new HandicapBet(HandicapBetType.F1, param, time, Probability, matchName, BetUrl, JavaSelectCode, sport, Maker);
                            }
                            else
                            if (type.Contains("2"))
                            {
                                result = new HandicapBet(HandicapBetType.F2, param, time, Probability, matchName, BetUrl, JavaSelectCode, sport, Maker);
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

                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }


            }
            System.Threading.Thread.Sleep(50);
        }


        public override void ParseMatchPageHtml(string html, string url)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            ParseMatchPageHtml(doc, url);
        }
        public override void ParseBets(List<MatchName> matches)
        {
            throw new NotImplementedException();
        }


        MatchName GetMatchName(HtmlNode node)
        {
            HtmlDocument h1_doc = new HtmlDocument();
            h1_doc.LoadHtml(node.InnerHtml);
            HtmlNodeCollection h1_nodes = h1_doc.DocumentNode.SelectNodes("//h1");
            string Name = h1_nodes.First().InnerText;
            var matchNameSplit = Name.Split(new string[] { " vs ", " @ ", " - " }, StringSplitOptions.RemoveEmptyEntries);
            if (matchNameSplit[0].Contains("("))
            {
                matchNameSplit[0] = matchNameSplit[0].Split(new string[] { " (" }, StringSplitOptions.RemoveEmptyEntries)[0];
            }
            if (matchNameSplit[1].Contains("("))
            {
                matchNameSplit[1] = matchNameSplit[1].Split(new string[] { " (" }, StringSplitOptions.RemoveEmptyEntries)[0];
            }
            return new MatchName(matchNameSplit[0], matchNameSplit[1]);

        }

        MatchName GetFullMatchName(HtmlDocument doc)
        {
            string opp1;
            string opp2;
            try
            {
                opp1 = doc.DocumentNode.SelectNodes("//td[@class='nameOne type2']").First().InnerText;
                opp2 = doc.DocumentNode.SelectNodes("//td[@class='nameTwo type2']").First().InnerText;

                opp1 = opp1.Split(new string[] { "<h1>", "</h1>" }, StringSplitOptions.RemoveEmptyEntries)[0];
                opp2 = opp2.Split(new string[] { "<h1>", "</h1>" }, StringSplitOptions.RemoveEmptyEntries)[0];

                return new MatchName(opp1, opp2);
            }
            catch
            {
                return null;
            }
        }
        Sport GetSport(HtmlDocument doc)
        {
            try
            {
                HtmlNodeCollection node = doc.DocumentNode.SelectNodes("//span[@class='preInfo type2']");
                string sport = node.First().InnerText;


                sport = sport.Split(new string[] { " -" }, StringSplitOptions.RemoveEmptyEntries)[0];

                switch (sport)
                {
                    case "Soccer": return Sport.Football;
                    case "Basketball": return Sport.Basketball;
                    case "Tennis": return Sport.Tennis;
                    case "Volleyball": return Sport.Volleyball;
                }

                return Sport.NotSupported;
            }
            catch
            {
                return Sport.NotSupported;
            }
        }
        Team GetTeam(string team)
        {
            if (!team.Contains("Player 1") && !team.Contains("Player 2") && !team.Contains("Hometeam") && !team.Contains("hometeam") && !team.Contains("Awayteam") && !team.Contains("awayteam"))
                return Team.All;
            else
            if ((team.Contains("Player 1") || team.Contains("hometeam") || team.Contains("Hometeam")) && (!team.Contains("Player 2") && !team.Contains("awayteam") && !team.Contains("Awayteam")))
                return Team.First;
            else
            if ((team.Contains("Player 2") || team.Contains("awayteam") || team.Contains("Awayteam")) && (!team.Contains("Player 1") && !team.Contains("hometeam") && !team.Contains("Hometeam")))
                return Team.Second;
            else
                // return Team.NotSupported;
                return Team.All;
        }
        Time GetTime(string TotalorHand)
        {
            TimeType type = TimeType.AllGame;
            int value = 0;
            if ((!TotalorHand.Contains("Sets") && !TotalorHand.Contains("sets")) && (TotalorHand.Contains("Set") || TotalorHand.Contains("set")))
            {
                type = TimeType.Set;
                if (TotalorHand.Contains("1") || TotalorHand.Contains("first"))
                    value = 1;
                else
               if (TotalorHand.Contains("2") || TotalorHand.Contains("second"))
                    value = 2;
                else
               if (TotalorHand.Contains("3") || TotalorHand.Contains("third"))
                    value = 3;
                else
               if (TotalorHand.Contains("4") || TotalorHand.Contains("fourth"))
                    value = 4;
                if (TotalorHand.Contains("5") || TotalorHand.Contains("fifth"))
                    value = 5;
                return new Time(type, value);
            }
            if (!TotalorHand.Contains("1st") && !TotalorHand.Contains("2nd") && !TotalorHand.Contains("3rd") && !TotalorHand.Contains("4th") && !TotalorHand.Contains("first") && !TotalorHand.Contains("second") && !TotalorHand.Contains("third") && !TotalorHand.Contains("fourth") && !TotalorHand.Contains("fifth"))
                return new Time(TimeType.AllGame);
            else
            if (TotalorHand.Contains("1st") || TotalorHand.Contains("first"))
                value = 1;
            else
            if (TotalorHand.Contains("2nd") || TotalorHand.Contains("second"))
                value = 2;
            else
            if (TotalorHand.Contains("3rd") || TotalorHand.Contains("third"))
                value = 3;
            else
            if (TotalorHand.Contains("4th") || TotalorHand.Contains("fourth"))
                value = 4;
            if (TotalorHand.Contains("5th") || TotalorHand.Contains("fifth"))
                value = 5;
            if (TotalorHand.Contains("Half") || TotalorHand.Contains("half"))
                type = TimeType.Half;
            if (TotalorHand.Contains("Quarter") || TotalorHand.Contains("quarter"))
                type = TimeType.Quarter;
            return new Time(type, value);
        }
    }

}
