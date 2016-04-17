using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Test.Server
{
    public class OAuth2CallbackHandler
    {
        public SecuredSigningClientSdk.ServiceClient SDKClient { get; set; }
        public string Start()
        {
            string result = null;
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(SampleParameters.CallbackUrl);
            listener.Prefixes.Add(SampleParameters.EmbeddedSigningUrl);
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
        private string HandleOAuth2Callback(HttpListenerContext context)
        {
            var uri = context.Request.Url;
            string state = string.Empty;
            var code = SecuredSigningClientSdk.OAuth2Client.HandleAuthorizeCallback(uri, out state);

            string responseString = $"<HTML><BODY><p>Authorization Code: <span>{code}</span></p><p>Go to the console application to see what happened.</p></BODY></HTML>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
            return code;
        }
        private void GetSigningPage(HttpListenerContext context)
        {
            var signingKey = context.Request.Url.Query.TrimStart('?').Split('&')
                .Select(t => t.Split('=')).ToDictionary(t => t[0], t => t[1]);

            var html = System.IO.File.ReadAllText("EmbeddedSigningSample.html")
                .Replace("[APIKEY]", SDKClient.APIKey)
                .Replace("[APISecret]", SDKClient.APISecret)
                .Replace("[APIServiceUrl]", SDKClient.ServiceBaseUrl)
                .Replace("[APIVersion]", SDKClient.APIVersion)
                .Replace("[SigningKey]", signingKey["key"]);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(html);
            // Get a response stream and write the response to it.
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
            System.Threading.Thread.Sleep(1000);
        }
    }
}
