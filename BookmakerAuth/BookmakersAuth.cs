using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetsLibrary;

namespace BookmakerAuth
{
    public static class BookmakersAuth
    {
        public static string GetAuthorizedCheckScript(Bookmaker bookmaker)
        {
            switch (bookmaker)
            {
                case Bookmaker.Marathonbet:
                    return "(function() { return document.getElementsByClassName('button btn-login').length; })();";
                case Bookmaker.Titanbet:
                    return string.Empty;
                case Bookmaker.Olimp:
                    return "(function() { return document.getElementsByClassName('enterBtn').length; })();";
                case Bookmaker.Leon:
                    return string.Empty;
            }
            return string.Empty;
        }

        public static string GetAuthorizeScript(Bookmaker bookmaker, string login, string password)
        {
            switch (bookmaker)
            {
                case Bookmaker.Marathonbet:
                    return string.Format("document.getElementById('auth_login').value = '{0}';" +
                                          "document.getElementById('auth_login_password').value = '{1}';" +
                                          "document.getElementsByClassName('button btn-login')[0].click();", login, password);
                case Bookmaker.Titanbet:
                    break;
                case Bookmaker.Olimp:
                  //  return "(function() { return document.getElementsByName('login').length; })();";
                    return string.Format("document.getElementsByName('login')[3].value = '{0}';" +
                                          "document.getElementsByName('passw')[2].value = '{1}';" +
                                          "document.getElementsByClassName('enterBtn')[0].click();", login, password);
                case Bookmaker.Leon:
                    break;
            }

            return string.Empty;
        }

    }
}
