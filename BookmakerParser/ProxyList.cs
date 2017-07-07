using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmakerParser
{
    static class ProxyList
    {
        private static Random rand = new Random();
        static int port = 2831;
        static string login = "mql4coder";
        static string password = "qH0Is2do";

        static string[] IPs = { "185.112.12.2","185.112.12.10","185.112.12.13","185.112.12.16","185.112.13.3","185.112.13.14","185.112.13.17","185.112.13.20",
                                "185.112.14.2","185.112.14.5","185.112.14.9","185.112.14.14","185.112.15.8",
                                "185.112.15.13","185.112.15.17","185.112.15.19","212.86.111.206","212.86.111.209","212.86.111.217","212.86.111.223",};

        public static (string ip, int port, string login, string password) GetRandomProxy()
        {
            int num = 0;
            lock (rand)
            {
                num = rand.Next(IPs.Length);
            }

            return (IPs[num], port, login, password);
        }
    }
}
