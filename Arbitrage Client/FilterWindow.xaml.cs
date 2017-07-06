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
using System.Windows.Shapes;
using BetsLibrary;

namespace Arbitrage_Client
{
    /// <summary>
    /// Interaction logic for FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : Window
    {
        public FilterWindow()
        {
            InitializeComponent();

            UseMarathonbet.IsChecked = FilterSettings.Bookmakers.Contains(Bookmaker.Marathonbet);
            UseLeon.IsChecked = FilterSettings.Bookmakers.Contains(Bookmaker.Leon);
            UseOlimp.IsChecked = FilterSettings.Bookmakers.Contains(Bookmaker.Olimp);
            UseTitanbet.IsChecked = FilterSettings.Bookmakers.Contains(Bookmaker.Titanbet);

            UseTennis.IsChecked = FilterSettings.Sports.Contains(Sport.Tennis);
            UseVolleyball.IsChecked = FilterSettings.Sports.Contains(Sport.Volleyball);
            UseFootball.IsChecked = FilterSettings.Sports.Contains(Sport.Football);
            UseBasketball.IsChecked = FilterSettings.Sports.Contains(Sport.Basketball);

            MinProfit.Text = FilterSettings.MinProfit.ToString();
        }

        private void ContentButton_Click(object sender, RoutedEventArgs e)
        {
            List<Bookmaker> bookmakers = new List<Bookmaker>();
            if (UseMarathonbet.IsChecked == true) bookmakers.Add(Bookmaker.Marathonbet);
            if (UseLeon.IsChecked == true) bookmakers.Add(Bookmaker.Leon);
            if (UseOlimp.IsChecked == true) bookmakers.Add(Bookmaker.Olimp);
            if (UseTitanbet.IsChecked == true) bookmakers.Add(Bookmaker.Titanbet);

            List<Sport> sports = new List<Sport>();

            if (UseTennis.IsChecked == true) sports.Add(Sport.Tennis);
            if (UseVolleyball.IsChecked == true) sports.Add(Sport.Volleyball);
            if (UseFootball.IsChecked == true) sports.Add(Sport.Football);
            if (UseBasketball.IsChecked == true) sports.Add(Sport.Basketball);

            FilterSettings.Bookmakers = bookmakers;
            FilterSettings.Sports = sports;

            try
            {
                FilterSettings.MinProfit = Convert.ToDouble(MinProfit.Text);
            }
            catch { }

            FilterSettings.Save();

            this.Close();
        }
    }
}
