using SecuredSigningClientSdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //initialise API Client
            var client = new ServiceClient(
                serviceUrl: "https://api.securedsigning.com/web",
                version: "v1.4",
                apiKey: "YOUR API KEY",
                secret: "YOUR API Secret",
                accessUrl: "YOUR Access URL");

            //get OAuth2 access token
            var authorizeUrl = client.OAuth2.CreateAuthorizeRequest("some value",
                OAuth2Client.OAuth2Scope.Basic.ToString(),
                OAuth2Client.OAuth2Scope.FormDirect.ToString(),
                OAuth2Client.OAuth2Scope.FormFiller.ToString(),
                OAuth2Client.OAuth2Scope.SmartTag.ToString());
            //start oauthorize process in a webpage
            System.Diagnostics.Process.Start(authorizeUrl);
            //for real using, need a server to handle the response
            Console.WriteLine("finish OAuth2 authorize process, then input authorization code:");
            string code = ReadLine();
            //get access token
            var accessToken = client.OAuth2.GetToken(code);
            client.AccessToken = accessToken.Access_Token;
            AccountSample(client);
            //FormDirectSample(client);
            //FormFillerSample(client);
            //SmartTagSample(client);
        }
        static void AccountSample(ServiceClient client)
        {
            var account=client.getAccountInfo();
            Console.WriteLine(string.Format("Hello {0}!", account.Name));
            var documents = client.getActiveDocuments("InBox");
        }
        static void FormDirectSample(ServiceClient client)
        {
            //get FormDirect forms
            var forms = client.getFormList();

            var tfnForm = forms.FirstOrDefault(t => t.Name.Contains("TFN"));
            var superannuationChoiceForm = forms.FirstOrDefault(t => t.Name.Contains("Superannuation"));

            //set invitee infomation
            tfnForm.Signers[0].FirstName = "Employee.Firstname";
            tfnForm.Signers[0].LastName = "Employee.Lastname";
            tfnForm.Signers[0].Email = "Employee.Email";
            tfnForm.Signers[0].MobileCountry = "Employee.MobileCountryCode"; //e.g. 61 for Australia
            tfnForm.Signers[0].MobileNumber = "Employee.MobileNumber";

            superannuationChoiceForm.Signers[0].FirstName = "Employee.Firstname";
            superannuationChoiceForm.Signers[0].LastName = "Employee.Lastname";
            superannuationChoiceForm.Signers[0].Email = "Employee.Emai";

            //send forms
            var documents = client.sendForms(
                formsToSend: new List<SecuredSigningClientSdk.Models.FormDirect> {
                    tfnForm,
                    superannuationChoiceForm
                },
                dueDate: DateTime.Now.AddDays(5));
        }
        static void FormFillerSample(ServiceClient client)
        {
            //get FormFiller templates
            var templates = client.getFormFillerTemplates();

            //e.g. There're 3 templates. We select the first and second one.
            var t1 = templates[0];
            var t2 = templates[1];

            //set Signer(s) for template

            //e.g. There're 2 signers in t1
            //[
            //  {
            //    "SignerReference": "186210...025238",
            //    "FirstName": "",
            //    "LastName": "",
            //    "Email": ""
            //  },
            //  {
            //    "SignerReference": "023215...001032",
            //    "FirstName": "",
            //    "LastName": "",
            //    "Email": ""
            //  }
            //]
            //
            t1.Signers = client.getFormFillerSignerTemplate(t1.Reference).Signers;
            t1.Signers[0].FirstName = "t1.s1.FirstName";
            t1.Signers[0].LastName = "t1.s1.LastName";
            t1.Signers[0].Email = "t1.s1.Email";
            t1.Signers[1].FirstName = "t1.s2.FirstName";
            t1.Signers[1].LastName = "t1.s2.LastName";
            t1.Signers[1].Email = "t1.s2.Email";

            //e.g. There's 1 signer in t2
            //[
            //  {
            //    "SignerReference": "048109...086112",
            //    "FirstName": "",
            //    "LastName": "",
            //    "Email": ""
            //  }
            //]
            t2.Signers = client.getFormFillerSignerTemplate(t2.Reference).Signers;
            t2.Signers[0].FirstName = "t2.s1.FirstName";
            t2.Signers[0].LastName = "t2.s1.LastName";
            t2.Signers[0].Email = "t2.s1.Email";

            //set Field(s) value for template (optional)
            //e.g. There's 2 fields in t2
            //[
            //    {
            //      "Label": "Question1",
            //      "Value": "",
            //      "FieldType": "Text",
            //      "IsRequired": false,
            //      "ID": "154107214180236225199245158115055014119106193119",
            //      "ReadOnly": true
            //    },
            //    {
            //      "Label": "Question2",
            //      "Value": "",
            //      "FieldType": "Text",
            //      "IsRequired": false,
            //      "ID": "150138217070090110130118010201151101188219145216",
            //      "ReadOnly": false
            //    }
            //]
            var fields = client.getFormFillerFieldTemplate(t2.Reference).Fields;
            //e.g. We only want to prefill the first field
            t2.Fields = new List<SecuredSigningClientSdk.Models.FormFillerField>();
            var f1 = fields[0];
            f1.Value = "f1.Value";
            t2.Fields.Add(f1);

            //send template(s)
            var documents = client.sendFormFillerTemplates(
                templates: new List<SecuredSigningClientSdk.Models.FormFillerTemplate> {
                    t1,
                    t2
                },
                dueDate: DateTime.Now.AddDays(5));
            //well done.
        }
        static void SmartTagSample(ServiceClient client)
        {
            var file = new System.IO.FileInfo("path to smart tag document");
            var documentReference = client.uploadDocumentFile(file);
            var smartTagResp = client.sendSmartTagDocument(new List<string> {
                documentReference
            }, DateTime.Now.AddDays(7));
        }
        #region to read code in one line more then 256 charactors.
        /// <summary>
        /// see http://stackoverflow.com/questions/5557889/console-readline-max-length
        /// </summary>
        const int READLINE_BUFFER_SIZE = 1024;
        static string ReadLine()
        {
            Stream inputStream = Console.OpenStandardInput(READLINE_BUFFER_SIZE);
            byte[] bytes = new byte[READLINE_BUFFER_SIZE];
            int outputLength = inputStream.Read(bytes, 0, READLINE_BUFFER_SIZE);
            //Console.WriteLine(outputLength);
            char[] chars = Encoding.UTF7.GetChars(bytes, 0, outputLength);
            return new string(chars);
        }
        #endregion
    }
}
