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

namespace Arbitrage_Client
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            InitializeMarathon();
            InitializeOlimp();
            InitializeLeon();
            InitializeTitan();
        }

        private void MarathonSave_Click(object sender, RoutedEventArgs e)
        {
            MarathonControl.Save(BetsLibrary.Bookmaker.Marathonbet);
            this.Close();
        }

        private void InitializeMarathon()
        {
            BookmakerSettings settings = BookmakersSettingsCollection.Get(BetsLibrary.Bookmaker.Marathonbet);
            MarathonControl.FillControl(settings);
        }

        private void OlimpSave_Click(object sender, RoutedEventArgs e)
        {
            OlimpControl.Save(BetsLibrary.Bookmaker.Olimp);
            this.Close();
        }

        private void InitializeOlimp()
        {
            BookmakerSettings settings = BookmakersSettingsCollection.Get(BetsLibrary.Bookmaker.Olimp);
            OlimpControl.FillControl(settings);
        }

        private void LeonSave_Click(object sender, RoutedEventArgs e)
        {
            LeonControl.Save(BetsLibrary.Bookmaker.Leon);
            this.Close();
        }

        private void InitializeLeon()
        {
            BookmakerSettings settings = BookmakersSettingsCollection.Get(BetsLibrary.Bookmaker.Leon);
            LeonControl.FillControl(settings);
        }

        private void TitanbetSave_Click(object sender, RoutedEventArgs e)
        {
            TitanbetControl.Save(BetsLibrary.Bookmaker.Titanbet);
            this.Close();
        }

        private void InitializeTitan()
        {
            BookmakerSettings settings = BookmakersSettingsCollection.Get(BetsLibrary.Bookmaker.Titanbet);
            TitanbetControl.FillControl(settings);
        }
    }
}
