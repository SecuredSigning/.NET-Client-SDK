using SecuredSigningClientSdk;
using System;
using System.Collections.Generic;
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
                version: "v1.3",
                apiKey: "YOUR API KEY",
                secret: "YOUR API Secret",
                accessUrl: "YOUR Access URL");
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
            var result = client.sendForms(
                formsToSend: new List<SecuredSigningClientSdk.Models.FormDirect> {
                    tfnForm,
                    superannuationChoiceForm
                },
                dueDate: DateTime.Now.AddDays(5));
        }
    }
}
