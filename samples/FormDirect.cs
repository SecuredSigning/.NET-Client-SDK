using SecuredSigningClientSdk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    static partial class Sample
    {
        public static void FormDirectSample(ServiceClient client)
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
    }
}
