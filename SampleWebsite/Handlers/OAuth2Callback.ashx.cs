using SampleWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace SampleWebsite.OAuth2Callback
{
    /// <summary>
    /// Summary description for OAuth2Callback
    /// </summary>
    public class OAuth2Callback : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var uri = context.Request.Url;
            string state = string.Empty;
            var code = SecuredSigningClientSdk.OAuth2Client.HandleAuthorizeCallback(uri, out state);
            var tokenResponse = SDK.client.OAuth2.GetToken(code);

            //From Session or other mechanism your website implemented, you can tell which user is login,
            //Or use "state" to pass some validation string (timestamp, userid, etc.) to make sure they're same user.
            //Then save access token and refresh token for this user.            
            //Save tokenResponse.Access_Token;
            //Save tokenResponse.Refresh_Token;
            //Connected
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}