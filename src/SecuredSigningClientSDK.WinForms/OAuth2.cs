using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SecuredSigningClientSdk.WinForms
{
    public class OAuth2 : System.Windows.Forms.WebBrowser
    {
        public class OAuth2CompletedEventArgs: EventArgs
        {
            public OAuth2CompletedEventArgs(OAuth2Client.OAuth2TokenResponse response,string state)
            {
                this.Response = response;
                this.State = state;
            }
            public OAuth2Client.OAuth2TokenResponse Response { get; private set; }
            public string State { get; private set; }
        }
        public event EventHandler<OAuth2CompletedEventArgs> Completed;
        static void SetWebBrowserDocumentMode()
        {
            var browser = new System.Windows.Forms.WebBrowser();
            var version = browser.Version;
            var keys = new string[] { @"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION",
                @"HKEY_CURRENT_USER\SOFTWARE\Wow6432Node\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION" };
            var processName = Process.GetCurrentProcess().ProcessName + ".exe";

            var value = Registry.GetValue(keys[0], processName, null);
            if (value == null)
            {
                int? v = null;
                switch (version.Major)
                {
                    case 11:
                        v = 11001;
                        break;
                    case 10:
                        v = 10001;
                        break;
                    case 9:
                        v = 9999;
                        break;
                    case 8:
                        v = 8888;
                        break;
                }
                if (v.HasValue)
                {
                    foreach (var key in keys)
                        Registry.SetValue(key, processName, v.Value, RegistryValueKind.DWord);
                }
            }
        }

        [ComVisible(true)]
        public class ScriptManager
        {
            readonly OAuth2 oAuth2;
            public ScriptManager(OAuth2 oAuth2)
            {
                this.oAuth2 = oAuth2;
            }
            public void HandleOAuth2AuthorizedResult(string result)
            {
                this.oAuth2.HandleResult(result);
            }
        }

        public OAuth2()
        {
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AllowNavigation = true;
            this.IsWebBrowserContextMenuEnabled = false;
            this.ObjectForScripting = new ScriptManager(this);
        }
        ServiceClient SdkClient;
        public void Authorise(ServiceClient client, string state, SecuredSigningClientSdk.OAuth2Client.OAuth2Scope scopes)
        {
            this.SdkClient = client;
            var url = this.SdkClient.OAuth2.CreateAuthorizeRequest(state,scopes);
            Start(client, url);
        }
        public void Authorise(ServiceClient client, string state, params string[] scopes)
        {
            this.SdkClient = client;
            var url = this.SdkClient.OAuth2.CreateAuthorizeRequest(state, scopes);
            Start(client, url);
        }
        void Start(ServiceClient client,string url)
        {
            this.Navigate(url);

        }
        protected void HandleResult(string returnUrl)
        {
            var uri = new Uri(returnUrl);
            string state = string.Empty;
            var code = OAuth2Client.HandleAuthorizeCallback(uri, out state);
            var response = this.SdkClient.OAuth2.GetToken(code);            
            OAuth2CompletedEventArgs args = new OAuth2CompletedEventArgs(response,state);
            Completed?.Invoke(this, args);
        }
    }
}
