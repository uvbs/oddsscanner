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
using CefSharp;
using CefSharp.Wpf;
using System.Security.Cryptography.X509Certificates;
using BetsLibrary;
using BookmakerAuth;
using BookmakerParser;

namespace Arbitrage_Client
{
    /// <summary>
    /// Interaction logic for BrowserControl.xaml
    /// </summary>
    public partial class BrowserControl : UserControl
    {
        public BrowserControl()
        {
            InitializeComponent();
            
            var requestContextSettings = new RequestContextSettings { CachePath = "cache_context", PersistSessionCookies = true };
            RequestContext context = new RequestContext(requestContextSettings);
            browserControl.RequestContext = context;

           // GoTo(Address);
            
        }

        BookmakerSettings bookmakerSettings;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GoTo(UrlText.Text);
        }
        

        private void GoTo(string address)
        {
            browserControl.Address = address;
            UrlText.Text = browserControl.Address;
        }

        public void GoTo(string address, Bookmaker bookmaker)
        {
            bookmakerSettings = BookmakersSettingsCollection.Get(bookmaker);
            if (bookmakerSettings.UseProxy) SetProxy();
            GoTo(address);
        }

        private void SetProxy()
        {
            if (bookmakerSettings == null) return;

            Cef.UIThreadTaskFactory.StartNew(delegate
            {
                browserControl.RequestHandler = new MyRequestHandler(bookmakerSettings);
                var rc = this.browserControl.RequestContext;
                var dict = new Dictionary<string, object>();
                dict.Add("mode", "fixed_servers");
                dict.Add("server", "" + bookmakerSettings.IP + ":" + bookmakerSettings.Port + "");
                bool success = rc.SetPreference("proxy", dict, out string error);
            });
        }

        public Task<JavascriptResponse> EvaluateScript(string script)
        {
            Task<JavascriptResponse> result = null;
            try
            {
                result = browserControl.GetBrowser().MainFrame.EvaluateScriptAsync(script);
            }
            catch { }
            return result;
        }

        public ChromiumWebBrowser Browser => browserControl;

        #region proxyauth
        class MyRequestHandler : IRequestHandler
        {
            BookmakerSettings bookmakerSettings;
            public MyRequestHandler(BookmakerSettings settings)
            {
                this.bookmakerSettings = settings;
            }

            public IResponseFilter GetResourceResponseFilter(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
            {
                return null;
            }

            public bool OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool isRedirect)
            {
                return false;
            }

            public CefReturnValue OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
            {
                return CefReturnValue.Continue;
            }

            public bool OnCertificateError(IWebBrowser browserControl, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
            {
                return false;
            }

            public bool OnOpenUrlFromTab(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
            {
                return false;
            }

            public void OnPluginCrashed(IWebBrowser browserControl, IBrowser browser, string pluginPath)
            {

            }

            public bool OnProtocolExecution(IWebBrowser browserControl, IBrowser browser, string url)
            {
                return false;
            }

            public bool OnQuotaRequest(IWebBrowser browserControl, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
            {
                return false;
            }

            public void OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status)
            {

            }

            public void OnRenderViewReady(IWebBrowser browserControl, IBrowser browser)
            {

            }

            public void OnResourceLoadComplete(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
            {

            }

            public void OnResourceRedirect(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, ref string newUrl)
            {

            }

            public bool OnResourceResponse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
            {
                return false;
            }

            public bool OnSelectClientCertificate(IWebBrowser browserControl, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
            {
                return false;
            }

            bool IRequestHandler.GetAuthCredentials(IWebBrowser browserControl, IBrowser browser, IFrame frame, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
            {

                if (isProxy == true)
                {
                    callback.Continue(bookmakerSettings.ProxyLogin, bookmakerSettings.ProxyPassword);

                    return true;
                }

                return false;

            }
        }
        #endregion

        private void UrlText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return) GoTo(UrlText.Text);
        }
    }
}
