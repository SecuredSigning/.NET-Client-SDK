using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Test.Server
{
    public class SampleServer
    {
        public SecuredSigningClientSdk.ServiceClient SDKClient { get; set; }
        public bool KeepSecretInServer { get; set; }
        bool ToStop { get; set; }
        private HttpListener Initialize()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(SampleParameters.CallbackUrl);
            listener.Prefixes.Add(SampleParameters.EmbeddedSigningUrl);
            listener.Prefixes.Add(SampleParameters.GetAuthHeadersUrl);
            listener.Prefixes.Add(SampleParameters.GetClientAccessTokenUrl);
            return listener;
        }
        public string StartOnce()
        {
            string result = null;
            var listener = Initialize();
            listener.Start();

            var context = listener.GetContext();
            var url = context.Request.Url.ToString();
            if (url.StartsWith(SampleParameters.CallbackUrl))
                result = HandleOAuth2Callback(context);
            if (url.StartsWith(SampleParameters.EmbeddedSigningUrl))
                GetSigningPage(context);
            listener.Stop();
            return result;
        }
        public string Listen()
        {
            string result = null;
            var listener = Initialize();
            listener.Start();
            while (!ToStop)
            {
                var context = listener.GetContext();
                var url = context.Request.Url.ToString();
                if (url.StartsWith(SampleParameters.GetAuthHeadersUrl))
                    GetAuthHeaders(context);
                if (url.StartsWith(SampleParameters.GetClientAccessTokenUrl))
                    GetClientAccessToken(context);
            }
            listener.Stop();
            return result;
        }
        public void Stop()
        {
            ToStop = true;
        }
        private void WriteResponse(HttpListenerResponse resp, string content, string contentType = "text/html")
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(content);

            resp.ContentLength64 = buffer.Length;
            resp.ContentType = contentType;
            resp.OutputStream.Write(buffer, 0, buffer.Length);
            resp.OutputStream.Close();

        }
        private string HandleOAuth2Callback(HttpListenerContext context)
        {
            var uri = context.Request.Url;
            string state = string.Empty;
            var code = SecuredSigningClientSdk.OAuth2Client.HandleAuthorizeCallback(uri, out state);

            string responseString = $"<HTML><BODY><p>Authorization Code: <span>{code}</span></p><p>Go to the console application to see what happened.</p></BODY></HTML>";
            WriteResponse(context.Response, responseString);
            System.Threading.Thread.Sleep(2000);
            return code;
        }
        private void GetSigningPage(HttpListenerContext context)
        {
            var signingKey = context.Request.Url.Query.TrimStart('?').Split('&')
                .Select(t => t.Split('=')).ToDictionary(t => t[0], t => t[1]);
            var html = string.Empty;
            if (KeepSecretInServer)
            {
                html = System.IO.File.ReadAllText("EmbeddedSigningSample.secured.html")
                    .Replace("[AuthGenerator]", SampleParameters.GetAuthHeadersUrl)
                    .Replace("[ClientAccessTokenGenerator]", SampleParameters.GetClientAccessTokenUrl);
            }
            else
            {
                html = System.IO.File.ReadAllText("EmbeddedSigningSample.html")
                    .Replace("[APISecret]", SDKClient.APISecret);
            };

            html = html.Replace("[APIKEY]", SDKClient.APIKey)
            .Replace("[APIServiceUrl]", SDKClient.ServiceBaseUrl)
            .Replace("[APIVersion]", SDKClient.APIVersion)
            .Replace("[SigningKey]", signingKey["key"]);

            WriteResponse(context.Response, html);
            System.Threading.Thread.Sleep(2000);
        }
        private void GetAuthHeaders(HttpListenerContext context)
        {
            var result = Newtonsoft.Json.JsonConvert
                .SerializeObject(SDKClient.GenerateAuthHeaders());
            WriteResponse(context.Response, result, "application/json");
            System.Threading.Thread.Sleep(2000);
        }
        private void GetClientAccessToken(HttpListenerContext context)
        {
            #region parse multipart
            var content = string.Empty;
            using (StreamReader reader = new StreamReader(context.Request.InputStream))
            {
                content = reader.ReadToEnd();
            }
            var contentType = context.Request.ContentType.Split(';');
            var parameters = contentType[1].Trim().Split('=');
            var boundary = "--" + parameters[1];
            content = content.Trim().TrimEnd('-');
            var form = content.Split(new string[] { boundary }, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                .ToDictionary(t => t[0].Split(';')[1].Split('=')[1].Trim('"'), t => t[1]);
            #endregion
            var scopes = form["scope"].Split(' ');
            var clientCredentialType = form["client_credential_type"];
            var extraData = new Dictionary<string, string>();
            string[] excluded = { "grant_type", "client_id", "scope", "client_credential_type" };
            foreach (var key in form.Keys)
            {
                if (excluded.Contains(key))
                    continue;
                extraData.Add(key, form[key]);
            }
            var result = SDKClient.OAuth2.GetClientAccessToken(clientCredentialType, extraData, scopes);
            WriteResponse(context.Response, Newtonsoft.Json.JsonConvert.SerializeObject(result), "application/json");
            System.Threading.Thread.Sleep(2000);
        }

    }
}
