namespace Test
{
    partial class OAuth2AuthoriseForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.oAuth21 = new SecuredSigningClientSdk.WinForms.OAuth2();
            this.SuspendLayout();
            // 
            // oAuth21
            // 
            this.oAuth21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.oAuth21.Location = new System.Drawing.Point(0, 0);
            this.oAuth21.MinimumSize = new System.Drawing.Size(20, 20);
            this.oAuth21.Name = "oAuth21";
            this.oAuth21.Size = new System.Drawing.Size(643, 518);
            this.oAuth21.TabIndex = 0;
            this.oAuth21.Completed += new System.EventHandler<SecuredSigningClientSdk.WinForms.OAuth2.OAuth2CompletedEventArgs>(this.oAuth21_Completed);
            // 
            // OAuth2AuthoriseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 518);
            this.Controls.Add(this.oAuth21);
            this.Name = "OAuth2AuthoriseForm";
            this.Text = "Authorise to Secured Signing";
            this.Load += new System.EventHandler(this.OAuth2AuthoriseForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private SecuredSigningClientSdk.WinForms.OAuth2 oAuth21;
    }
}