using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmakerAuth
{
    public static class MarathonAuth
    {
        public static string GetMarathonAuthScript(string login, string password)
        {
            return string.Format("document.getElementById('auth_login').value = '{0}';" +
                                          "document.getElementById('auth_login_password').value = '{1}';" +
                                          "document.getElementsByClassName('button btn-login')[0].click();", login, password);
        }

        public static string GetAuthorizedCheckScript()
        {
            return "(function() { return document.getElementsByClassName('button btn-login').length; })();";
            
        }
    }
}
