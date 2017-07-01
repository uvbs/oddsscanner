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
using System.Media;
using BetsLibrary;

namespace Arbitrage_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BrowserWindow browserWindow;
        public MainWindow()
        {
            InitializeComponent();
            browserWindow = new BrowserWindow();
            browserWindow.Closed += (i, j) => { isBrowserOpen = false; PlaceBet.IsEnabled = true; };

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*
            browser = new BrowserWindow();
            browser.Show();
            browser.GoTo("https://2ip.ru");
            browser.GetBrowser.LoadingStateChanged += GetBrowser_LoadingStateChanged;*/
            //  browser.Address = "jgkgjg";
            
        }
        /*
        private void GetBrowser_LoadingStateChanged(object sender, CefSharp.LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                browser.GetBrowser.LoadingStateChanged -= GetBrowser_LoadingStateChanged;

                browser.EvaluateScript(BookmakerAuth.MarathonAuth.GetAuthorizedCheckScript()).ContinueWith(task =>
                {
                   if(task.Result.Result == null) return;
                    if((int)task.Result.Result != 0) browser.EvaluateScript(BookmakerAuth.MarathonAuth.GetMarathonAuthScript("illya2342", "12"));
                });
                
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            browser = new BrowserWindow();
            browser.Show();
            browser.GoTo("https://m.titanbet.com/en/inplay/FOOT/Football");
        }*/

        bool AutoBetting = false;

        private void Window_Initialized(object sender, EventArgs e)
        {
         /*   List<ArbitrageBet> list = new List<ArbitrageBet>();
            list.Add(new ArbitrageBet(null, 1, 1));
            BetsList.ItemsSource = list;*/
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        bool isSettingsWindowOpen = false;
        private void BookmakerButton_Click(object sender, RoutedEventArgs e)
        {
            if (isSettingsWindowOpen) return;
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Owner = this;
            settingsWindow.Closed += (i, j) => isSettingsWindowOpen = false;
            settingsWindow.Show();
            isSettingsWindowOpen = true;


        }
    
        bool isBrowserOpen = false;
        
        private void PlaceBet_Click(object sender, RoutedEventArgs e)
        {
         //   SystemSounds.Beep.Play();
            if (isBrowserOpen) return;
            object selectedItem = BetsList.SelectedItem;
            if (selectedItem == null) return;
            ArbitrageBet arbitrageBet = selectedItem as ArbitrageBet;
            browserWindow.GoTo(arbitrageBet.Bet.BetUrl, BookmakersSettingsCollection.Get(arbitrageBet.Bookmaker));
            browserWindow.Title = string.Format("{0} {1} {2} Profit: {3:0.00}/{4:0.00} Coeff: {5}", arbitrageBet.Bookmaker, arbitrageBet.MatchName, arbitrageBet.Bet, arbitrageBet.Profit, arbitrageBet.ProfitVsAverage, arbitrageBet.Coeff);
            browserWindow.Show();
            isBrowserOpen = true;
            PlaceBet.IsEnabled = false;
        }

        private void AutoBettingButton_Click(object sender, RoutedEventArgs e)
        {
            AutoBetting = !AutoBetting;
            AutoBettingButton.Content = AutoBetting ? "Остановить АвтоПроставку" : "Запустить АвтоПроставку";
        }



        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private void DeleteBetButtom_Click(object sender, RoutedEventArgs e)
        {
            object selectedItem = BetsList.SelectedItem;
            if (selectedItem == null) return;
            ArbitrageBet arbitrageBet = selectedItem as ArbitrageBet;

            PlacedBets.AddBet(arbitrageBet.Bet);
            BetsList.Items.Remove(selectedItem);
        }

        bool isFilterWindowOpen = false;
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (isFilterWindowOpen) return;
            FilterWindow filterWindow = new FilterWindow();
            filterWindow.Owner = this;
            filterWindow.Closed += (i, j) => isFilterWindowOpen = false;
            filterWindow.Show();
            isFilterWindowOpen = true;
        }
    }
}
