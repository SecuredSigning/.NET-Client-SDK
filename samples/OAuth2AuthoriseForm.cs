using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class OAuth2AuthoriseForm : Form
    {
        SecuredSigningClientSdk.ServiceClient client;
        string state;
        SecuredSigningClientSdk.OAuth2Client.OAuth2Scope scope;
        public OAuth2AuthoriseForm(SecuredSigningClientSdk.ServiceClient client,string state,SecuredSigningClientSdk.OAuth2Client.OAuth2Scope scope)
        {
            this.client = client;
            this.state = state;
            this.scope = scope;
            InitializeComponent();
        }

        private void oAuth21_Completed(object sender, SecuredSigningClientSdk.WinForms.OAuth2.OAuth2CompletedEventArgs e)
        {
            if(OnAuthorized!=null)
            {
                OnAuthorized(e.Response);
            }
        }

        public Action<SecuredSigningClientSdk.OAuth2Client.OAuth2TokenResponse> OnAuthorized;

        private void OAuth2AuthoriseForm_Load(object sender, EventArgs e)
        {
            oAuth21.Authorise(this.client, this.state, this.scope);
        }
    }
}
