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

namespace Arbitrage_Client
{
    /// <summary>
    /// Interaction logic for BookmakerControl.xaml
    /// </summary>
    public partial class BookmakerControl : UserControl
    {
        public BookmakerControl()
        {
            InitializeComponent();
        }

        public void FillControl(BookmakerSettings settings)
        {
            Login.Text = settings.Login;
            Password.Password = settings.Password;
            ProxyIP.Text = settings.IP;
            ProxyPort.Text = settings.Port;
            ProxyLogin.Text = settings.ProxyLogin;
            ProxyPassword.Password = settings.ProxyPassword;
            UseProxy.IsChecked = settings.UseProxy;
            BetSize.Text = settings.BetSize;
            AutoBet.IsChecked = settings.AutoSelect;
        }

        public void Save(BetsLibrary.Bookmaker bookmaker)
        {
            BookmakerSettings settings = BookmakersSettingsCollection.Get(bookmaker);

            settings.Login = Login.Text;
            settings.Password = Password.Password;
            settings.IP = ProxyIP.Text;
            settings.Port = ProxyPort.Text;
            settings.ProxyLogin = ProxyLogin.Text;
            settings.ProxyPassword = ProxyPassword.Password;
            settings.UseProxy = UseProxy.IsChecked == true;
            settings.BetSize = BetSize.Text;
            settings.AutoSelect = AutoBet.IsChecked == true;

            BookmakersSettingsCollection.Save();
        }
    }
}
