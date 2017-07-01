using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrage_Client
{
    public class BookmakerSettings
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string BetSize { get; set; }
        public bool AutoSelect { get; set; }

        public string IP { get; set; }
        public string Port { get; set; }
        public string ProxyLogin { get; set; }
        public string ProxyPassword { get; set; }
        public bool UseProxy { get; set; }
        
    }
}
