using System.Linq;

namespace SecuredSigningClientSdk.Helpers
{
    public static class ScopeHelper
    {
        public static string[] ToStringArray(this OAuth2Client.OAuth2Scope scopes)
        {
            return scopes.ToString("F").Split(',').Select(t => t.Trim()).ToArray();
        }
    }
}
