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

            //set invitee infomation for employee
            tfnForm.Signers[0].FirstName = SampleParameters.Invitee1_FirstName;
            tfnForm.Signers[0].LastName = SampleParameters.Invitee1_LastName;
            tfnForm.Signers[0].Email = SampleParameters.Invitee1_Email;
            tfnForm.Signers[0].MobileCountry = SampleParameters.Invitee1_MobileCountry; //e.g. 61 for Australia
            tfnForm.Signers[0].MobileNumber = SampleParameters.Invitee1_MobileNumber;
            //set invitee infomation for employer
            superannuationChoiceForm.Signers[0].FirstName = SampleParameters.Invitee2_FirstName;
            superannuationChoiceForm.Signers[0].LastName = SampleParameters.Invitee2_LastName;
            superannuationChoiceForm.Signers[0].Email = SampleParameters.Invitee2_Email;

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
