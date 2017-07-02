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

namespace oddsscanner
{
    public class Program
    {

        public static void Main(string[] args)
        {
            ArbitrageFinder finder = new ArbitrageFinder();

            //Marathonbet marathon = new Marathonbet();
            LeonBets leon = new LeonBets();

           // finder.AddBookmaker(marathon);
            finder.AddBookmaker(leon);

            
            var result = finder.GetArbitrageBets();

            Console.WriteLine("here");

            for (int i = 0; i < 20; i++)
            {
                  result = finder.GetArbitrageBets();
                //   marathon.Parse();
                Thread.Sleep(2000);
            }

                List<BetsLibrary.Bet> list = new List<BetsLibrary.Bet>();

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
            

     //       result = finder.GetArbitrageBets();

            Console.WriteLine(finder);

        }

    }
}
