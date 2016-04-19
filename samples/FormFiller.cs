using SecuredSigningClientSdk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    static partial class Sample
    {
        public static void FormFillerSample(ServiceClient client)
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
            //and there's 1 signer in t2
            //[
            //  {
            //    "SignerReference": "048109...086112",
            //    "FirstName": "",
            //    "LastName": "",
            //    "Email": ""
            //  }
            //]
            t1.Signers = client.getFormFillerSignerTemplate(t1.Reference).Signers;
            t2.Signers = client.getFormFillerSignerTemplate(t2.Reference).Signers;
            t1.Signers[0].FirstName = t2.Signers[0].FirstName= SampleParameters.Invitee1_FirstName;
            t1.Signers[0].LastName = t2.Signers[0].LastName = SampleParameters.Invitee1_LastName;
            t1.Signers[0].Email = t2.Signers[0].Email= SampleParameters.Invitee1_Email;

            t1.Signers[1].FirstName = SampleParameters.Invitee2_FirstName;
            t1.Signers[1].LastName = SampleParameters.Invitee2_LastName;
            t1.Signers[1].Email = SampleParameters.Invitee2_Email;
            
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
            f1.Value = SampleParameters.FormFillerSampleFieldValue;
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
        public static void FormFillerSample_EmbeddedSigning(ServiceClient client)
        {
            //get FormFiller templates
            var templates = client.getFormFillerTemplates();

            //e.g. There're 3 templates. We select the second one.
            var t2 = templates[1];

            //set Signer(s) for template

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
            t2.Signers[0].FirstName = SampleParameters.Invitee1_FirstName;
            t2.Signers[0].LastName = SampleParameters.Invitee1_LastName;
            t2.Signers[0].Email = SampleParameters.Invitee1_Email;

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
            f1.Value = SampleParameters.FormFillerSampleFieldValue;
            t2.Fields.Add(f1);

            //send template(s)
            var documents = client.sendFormFillerTemplates(
                templates: new List<SecuredSigningClientSdk.Models.FormFillerTemplate> {
                    t2
                },
                dueDate: DateTime.Now.AddDays(5),
                embedded: true);
            var signingKey = documents.Signers.FirstOrDefault(t => !string.IsNullOrWhiteSpace(t.SigningKey))?.SigningKey;

            //populate signing key into a embedded signing webpage.
            //a sample server hosts the embedded signing webpage, implement your own page instead.
            Console.WriteLine(signingKey);
            var server = new Server.SampleServer()
            {
                SDKClient = client,
                KeepSecretInServer = true
            };

            System.Diagnostics.Process.Start($"{SampleParameters.EmbeddedSigningUrl}?key={signingKey}");
            var runResult = server.StartOnce();
            Console.WriteLine("Go to browser to sign the document");
            server.Listen();
            bool signed = false;
            while (!signed)
            {
                System.Threading.Thread.Sleep(5000);
                Console.WriteLine("Check document status");
                var status = client.getStatus(documents.Documents.First()?.Reference);
                if (status.Status == "Complete")
                {
                    signed = true;
                }
            }
            server.Stop();
            Console.WriteLine("Done");
            Console.Read();

        }
    }
}
