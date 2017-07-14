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
using CefSharp.Wpf;
using Newtonsoft.Json;
using System.ServiceModel;
using BookmakerAuth;
using System.Threading;
using CefSharp;

namespace Arbitrage_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            if (!Cef.IsInitialized)
            {
                CefSettings settings = new CefSettings()
                {
                    CachePath = "cache_context",
                    PersistSessionCookies = true
                };
                Cef.Initialize(settings);
            }

        }

        bool AutoBettingStarted = false;

        private void Window_Initialized(object sender, EventArgs e)
        {
        /*    AutoBetting betting = new AutoBetting(this);

            var task = new Thread(() =>
            {
                while (true)
                {
                    betting.GetForks();
                    System.Threading.Thread.Sleep(1000);
                }
            });
            task.SetApartmentState(ApartmentState.STA);
            task.IsBackground = true;
            task.Start();*/
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

        private void OpenPlaceBet()
        {/*
            if (isBrowserOpen) return;
            object selectedItem = BetsList.SelectedItem;
            if (selectedItem == null) return;
            ArbitrageBet arbitrageBet = selectedItem as ArbitrageBet;

            browserWindow = new BrowserWindow();
          //  browserWindow.PlaceBet(arbitrageBet);
            browserWindow.Closed += (i, j) => { isBrowserOpen = false; PlaceBet.IsEnabled = true; };
            browserWindow.Show();

            isBrowserOpen = true;
            PlaceBet.IsEnabled = false;

           */
        }

        private void PlaceBet_Click(object sender, RoutedEventArgs e)
        {
            OpenPlaceBet();
        }

        private void AutoBettingButton_Click(object sender, RoutedEventArgs e)
        {
            AutoBettingStarted = !AutoBettingStarted;
            AutoBettingButton.Content = AutoBettingStarted ? "Остановить АвтоПроставку" : "Запустить АвтоПроставку";
        }



        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private void DeleteBetButtom_Click(object sender, RoutedEventArgs e)
        {
          /*  object selectedItem = BetsList.SelectedItem;
            if (selectedItem == null) return;
            ArbitrageBet arbitrageBet = selectedItem as ArbitrageBet;

            PlacedBets.AddBet(arbitrageBet);
            BetsList.Items.Remove(selectedItem);*/
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

        private void Sync(List<ArbitrageBet> newList)
        {
           /* newList = newList.Where(bet => !PlacedBets.Contains(bet)).ToList();
            
            BetsList.Dispatcher.Invoke(() =>
            {
                for(int i=0; i < BetsList.Items.Count; i++)
                    if (!newList.Contains(BetsList.Items[i])) { BetsList.Items.RemoveAt(i); i--; }

                foreach (var bet in newList)
                    if (!BetsList.Items.Contains(bet)) { BetsList.Items.Add(bet); SystemSounds.Beep.Play(); }
                });*/
        }

        private void BetsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenPlaceBet();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MatchParserModel item = tabControl.SelectedItem as MatchParserModel;
            if(item!=null) item.Color = "Black";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
                Cef.Shutdown();
        }
    }
}
