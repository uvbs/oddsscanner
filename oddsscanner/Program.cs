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

namespace oddsscanner
{
    public class Program
    {

        public static void Main(string[] args)
        {
            ArbitrageFinder finder = new ArbitrageFinder();

            //Marathonbet marathon = new Marathonbet();
            LeonBets leon = new LeonBets();

            //finder.AddBookmaker(marathon);
           finder.AddBookmaker(leon);

            
            var result = finder.GetArbitrageBets();

            Console.WriteLine("here");

            for (int i = 0; i < 5; i++)
            {
                  result =   finder.GetArbitrageBets();
             //   marathon.Parse();
                Thread.Sleep(2000);
            }

            List<BetsLibrary.Bet> list = new List<BetsLibrary.Bet>();

            foreach(var bet in leon.GetBetList())
            {
                if (!list.Contains(bet)) list.Add(bet);
                else throw new Exception();
            }
            

     //       result = finder.GetArbitrageBets();

            Console.WriteLine(finder);

        }

    }
}
