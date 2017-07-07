using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using CefSharp.OffScreen;
using CefSharp;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using HtmlAgilityPack;
using OddsAnalyzer;
using BookmakerParser;
using BetsLibrary;
using System.Linq;

namespace oddsscanner
{
    public class Program
    {

        public static void Main(string[] args)
        {/*
            ArbitrageFinder finder = new ArbitrageFinder();

            OlimpBookmaker olimp = new OlimpBookmaker();
            Marathonbet marathon = new Marathonbet();
            LeonBets leon = new LeonBets();
            TitanBet titan = new TitanBet();

            finder.AddBookmaker(marathon);
            finder.AddBookmaker(leon);
            finder.AddBookmaker(olimp);
         //   finder.AddBookmaker(titan);
            
            var result = finder.GetForks();

         //   Console.WriteLine("here");

            for (int i = 0; i < 10; i++)
            {
               result = finder.GetForks();

               var m_matches = marathon.GetMatchList();
               var l_matches = leon.GetMatchList();
                var o_matches = olimp.GetMatchList();
                var t_matches = titan.GetMatchList();
                var forks = result.Where(e => e.Profit > 0);
                Thread.Sleep(10000);
            }

                List<Bet> list = new List<Bet>();
            /*
            foreach(var bet in leon.GetBetList())
            {
                if (!list.Contains(bet)) list.Add(bet);
                else
                {
                    Console.WriteLine("error");
                    Console.WriteLine();
                    Console.Write("{0} vs {1}   ", bet.MatchName.FirstTeam, bet.MatchName.SecondTeam);
                    Console.Write("Sport : {0}  ", bet.Sport);
                    try
                    {
                        var T = bet as ResultBet;
                        if (T != null)
                        {
                            Console.Write("{0}", ((ResultBet)bet).ToString());
                            Console.Write("coef: {0}", T.Odds);
                        }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                    try
                    {
                        var T = bet as TotalBet;
                        if (T != null)
                        {
                            Console.Write("{0}", ((TotalBet)bet).ToString());
                            Console.Write("coef: {0}", T.Odds);
                        }
                    }
                    catch { }
                    try
                    {
                        var h = bet as HandicapBet;
                        if (h != null)
                        {
                            Console.Write("{0}", ((HandicapBet)bet).ToString());
                            Console.Write("coef: {0}", h.Odds);
                        }
                    }
                    catch { }
                }
            }
            */

            //   result = finder.GetArbitrageBets();

            //     Console.WriteLine(finder);

            //  MatchName name1 = new MatchName("Ukraine Dinamo (U-21)", "Russia");
            //  MatchName name2 = new MatchName("D/Ukr U21", "Russia A");

            //  Console.WriteLine(name1.Equals(name2));

            /*var web = new HtmlWeb();
            web.UserAgent = "Mozilla/5.0 (Linux; U; Android 4.0.3; ko-kr; LG-L160L Build/IML74K) AppleWebkit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30";
           // var doc = web.Load("http://sports.titanbet.com/web_nr?key=do_switch_platform&amp;platform=mobile");
            var doc = web.Load("https://m.10bet.com/?#home");

            Console.WriteLine(doc.DocumentNode.OuterHtml);*/

            Marathonbet marathon = new Marathonbet();
            OlimpBookmaker olimp = new OlimpBookmaker();
            TitanBet titan = new TitanBet();


            for (int i = 0; i < 100; i++) {
                titan.Parse();
                Console.WriteLine(titan.GetBetList().Count);
                Thread.Sleep(2000);
            }

            Console.ReadLine();

        }

    }
}
