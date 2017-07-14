using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BetsLibrary;
using BookmakerParser;
using CefSharp;
using System.Collections.ObjectModel;

namespace Arbitrage_Client
{
    /// <summary>
    /// Interaction logic for MatchParserControl.xaml
    /// </summary>
    public partial class MatchParserControl : UserControl
    {
        public Bookmaker Bookmaker;
        BetsLibrary.BookmakerParser parser;
        public string matchUrl;
        public MatchName MatchName { get; private set; }
        private bool locked = false;

        public MatchParserControl(BetsLibrary.BookmakerParser parser, Bookmaker bookmaker, string matchUrl, MatchName MatchName)
        {
            InitializeComponent();
            browserControl.GoTo(matchUrl, bookmaker);
            this.parser = parser;
            this.matchUrl = matchUrl;
            this.MatchName = MatchName;
            Bookmaker = bookmaker;
        }

        public void Parse()
        {
            if (browserControl.Browser.IsLoading || locked) return;

            try
            {
                
                var task = browserControl.Browser.GetSourceAsync();
                GetSource(task, 1000).ContinueWith((html) =>
                {
                    if (html.Result == string.Empty) return;
                    locked = true;
                    parser.ParseMatchPageHtml(html.Result, matchUrl);
                    if (parser.BetList.Count == 0) return;
                    lstwBets.Dispatcher.Invoke(() =>
                    {
                        try
                        {
                            lstwBets.ItemsSource = parser.BetList.ToList().Where((e) => e.BetUrl == matchUrl).ToList();
                            lstwBets.Items.Refresh();
                        }
                        catch { }
                    });
                    locked = false;
                });
                
            }
            catch { }
        }

        private async Task<string> GetSource(Task<string> task, int millisecondsTimeout)
        {
            if (await Task.WhenAny(task, Task.Delay(millisecondsTimeout)) == task)
            {
                return task.Result;
            }
            return string.Empty;
        }

        public void PlaceBet(ArbitrageBet bet)
        {
            /*BetLabel.Content = bet.MainBet.ToString();
            BetLabel.Visibility = Visibility.Visible;
            CoeffLabel.Content = $"Coeff: {bet.MainBet.Odds:0.00}/{bet.SecondBet.Odds:0.00}({bet.SecondBet.Bookmaker})";
            CoeffLabel.Visibility = Visibility.Visible;
            ProfitLabel.Content = $"{bet.Profit}%";
            ProfitLabel.Visibility = Visibility.Visible;*/
        }

        public ObservableCollection<ArbitrageBet> ForksList = new ObservableCollection<ArbitrageBet>();

        public void SetForks(List<ArbitrageBet> bets)
        {
         /*   foreach (var bet in bets)
                if (!ForksList.Contains(bet)) ForksList.Add(bet);

            var itemsToDelete = ForksList.Where((fork) => !bets.Contains(fork)).ToList();
            foreach (var item in itemsToDelete)
                ForksList.Remove(item);*/

            lstwForks.ItemsSource = bets;
            lstwForks.Items.Refresh();
        }
        

        private void lstwForks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ArbitrageBet bet = lstwForks.SelectedItem as ArbitrageBet;
            if (bet == null) return;
            var response = browserControl.EvaluateScript(bet.MainBet.JavaScriptSelectorCode);
            response.ContinueWith((r) => {
                
                });
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Delete)
            {
                ArbitrageBet bet = lstwForks.SelectedItem as ArbitrageBet;
                if (bet == null) return;
                PlacedBets.AddBet(bet);
            }
        }
    }
}
