using System;
using System.Collections.Generic;
using System.Linq;
using BetsLibrary;
using CefSharp.OffScreen;
using CefSharp;
using HtmlAgilityPack;


namespace BookmakerParser
{
    public class TenBet : BetsLibrary.BookmakerParser
    {
        private string MatchListUrl = "https://www.10bet.com/live-betting/";

        private const int MaximumMatches = 10;

        private Dictionary<string, ChromiumWebBrowser> browserDict = new Dictionary<string, ChromiumWebBrowser>();
        List<string> activeMatchList = new List<string>();
        private ChromiumWebBrowser matchListBrowser;
        private const Bookmaker Maker = Bookmaker.Leon; // 10 BET
        string JavaSelectCode = "Java";
        string[] type_of_sport = { "Soccer", "Basketball", "Tennis", "Volleyball" };
        public TenBet()
        {

        }

        private void LoadMatchListPages()
        {
            matchListBrowser = new ChromiumWebBrowser(MatchListUrl);
        }

        public override void Parse()
        {
            if (matchListBrowser == null)
                LoadMatchListPages();

            if (!matchListBrowser.IsBrowserInitialized || matchListBrowser.IsLoading) return;

            int index = 0;
            activeMatchList = new List<string>();
            while (activeMatchList.Count < MaximumMatches && index < type_of_sport.Length)
            {
                ParseMatchList(index);
                index++;

            }

        }
        

        public void ParseMatchList(int index)
        {
            string html = matchListBrowser.GetSourceAsync().Result;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection matchList = doc.DocumentNode.SelectNodes("//a[@class='specials']");

            if (matchList == null) return;
            foreach (var node in matchList)
            {
                string id = node.Attributes["onclick"].Value.Split(new string[] { "('", "')" }, StringSplitOptions.RemoveEmptyEntries)[1];
               // це id кнопки. 
               activeMatchList.Add(id);

                if (!browserDict.ContainsKey(id))
               {
                   string url = "https://mobile.leonbets.net/mobile/#eventDetails/:" + id;
                   Console.WriteLine(url);
                   browserDict.Add(id, new ChromiumWebBrowser(url));
                   System.Threading.Thread.Sleep(5000);
               }
               if (activeMatchList.Count == MaximumMatches) break;
                
            }





        }

   
    }

}
