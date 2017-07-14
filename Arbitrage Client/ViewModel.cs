using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using CefSharp;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BetsLibrary;

namespace Arbitrage_Client
{
    public sealed class ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<MatchParserModel> Tabs { get; set; }
        public AutoBetting betting;
        private Task task;
        public ViewModel()
        {
            Tabs = new ObservableCollection<MatchParserModel>();

            betting = new AutoBetting(Tabs);
            betting.TabsChanged += (e, a) => OnPropertyChanged("Tabs");

            task = new Task(() =>
            {
                while (true)
                {
                    betting.GetForks();
                    System.Threading.Thread.Sleep(1000);
                }
            });
            task.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
    public sealed class MatchParserModel : INotifyPropertyChanged
    {
        public DateTime LastRefresh { get; set; }
        public string Header { get; set; }
        public MatchParserControl Control { get; set; }
        private string color;
        public string Color
        {
            get { return color; }
            set
            {
                color = value;
                OnPropertyChanged("Color");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
