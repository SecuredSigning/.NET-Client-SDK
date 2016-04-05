using SecuredSigningClientSdk;
using System;
using System.Collections.Generic;

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
    }
}
