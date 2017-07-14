using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetsLibrary;
using BookmakerParser;
using OddsAnalyzer;
using System.Collections.ObjectModel;
using System.Windows;

namespace Arbitrage_Client
{
    public class AutoBetting
    {
        public ObservableCollection<MatchParserModel> Tabs { get; set; }
        private Dictionary<Bookmaker, BetsLibrary.BookmakerParser> parsersDict = new Dictionary<Bookmaker, BetsLibrary.BookmakerParser>();
        private ArbitrageFinder arbitrageFinder = new ArbitrageFinder();
        public event EventHandler TabsChanged;

        public AutoBetting(ObservableCollection<MatchParserModel> Tabs)
        {
            parsersDict.Add(Bookmaker.Marathonbet, new Marathonbet());
            parsersDict.Add(Bookmaker.Leon, new LeonBets());
            parsersDict.Add(Bookmaker.Olimp, new OlimpBookmaker());

            foreach (var parser in parsersDict.Values)
                arbitrageFinder.AddBookmaker(parser);

            this.Tabs = Tabs;
        }

        bool locked = false;

        public void GetForks()
        {
            if (locked) return;
            foreach(var pair in parsersDict)
            {
                pair.Value.BetList = new List<Bet>();
            }
            SyncMatchBrowsers();
            ParseBets();

            locked = true;
            Task.Delay(3000).ContinueWith((n) =>
            {

                var forks = arbitrageFinder.GetForks();
                forks = forks.Where((e) => FilterSettings.Sports.Contains(e.Sport) && e.Profit >= FilterSettings.MinProfit && !PlacedBets.Contains(e)).ToList();


                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var tab in Tabs)
                    {
                        var tabForks = forks.Where((fork) => fork.MainBet.BetUrl == tab.Control.matchUrl).ToList();
                        tab.Control.SetForks(tabForks);
                        if (tabForks.Count > 0)
                        {
                            tab.Color = "Blue";
                            System.Media.SystemSounds.Beep.Play();
                        }
                        else if (tab.Color != "Red") tab.Color = "Black";
                    }
                });
                locked = false;
            });

        }

        public void SyncMatchBrowsers()
        {
            foreach(var pair in parsersDict)
            {
                pair.Value.Parse();
            }

            List<MatchName> matches = new List<MatchName>();

            foreach (var pair in parsersDict)
                matches.AddRange(pair.Value.MatchDict.Keys);

            filteredMatches = matches.Where(name =>
            {
                int count = matches.Where(match => match.Equals(name)).Count();
                return count > 2;
            }).ToList();

            filteredMatches.AddRange(matches.Where(name =>
            {
                int count = matches.Where(match => match.Equals(name)).Count();
                return count == 2;
            }).ToList().Take(Math.Max(15 - filteredMatches.Count, 1)));

            CloseNotActiveWindows();
            OpenNewWindows();
        }

        List<MatchName> filteredMatches;

        private void OpenNewWindows()
        {
            filteredMatches = filteredMatches.ToList();

            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var pair in parsersDict)
                    foreach (var match in pair.Value.MatchDict)
                    {
                        if (filteredMatches.Contains(match.Key))
                        {
                            var tabs = Tabs.Where((e) => e.Control.matchUrl == match.Value);
                            if (tabs.Count() > 0) tabs.First().LastRefresh = DateTime.Now;
                            else
                            {
                                var control = new MatchParserControl(pair.Value, pair.Key, match.Value, match.Key);
                                Tabs.Add(new MatchParserModel() { Control = control, Header = string.Format("{0}: ", pair.Key), Color = "Red", LastRefresh = DateTime.Now });
                                TabsChanged?.Invoke(this, null);
                            }
                            //System.Threading.Thread.Sleep(1000);
                        }

                    }
            });
        }

        private void ParseBets()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var tab in Tabs)
                    tab.Control.Parse();
            });
        }

        private void CloseNotActiveWindows()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var notActiveMatchArray = Tabs.Where(e => DateTime.Now - e.LastRefresh > TimeSpan.FromMinutes(1))
                .ToArray();

                foreach (var key in notActiveMatchArray)
                {
                    Tabs.Remove(key);
                    TabsChanged?.Invoke(this, null);
                }
            });
        }

        
    }
}
