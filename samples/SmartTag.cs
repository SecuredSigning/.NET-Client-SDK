using SecuredSigningClientSdk;
using SecuredSigningClientSdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    static partial class Sample
    {
        /// <summary>
        /// Basic Usage. For more details about Smart Tag API, see:
        /// <see cref="http://www.securedsigning.com/documentation/developer/smarttag-api" />
        /// </summary>
        /// <param name="client"></param>
        public static void SmartTagSample(ServiceClient client)
        {
            var file = new System.IO.FileInfo(SampleParameters.Path2SmartTagDocument);
            var documentReference = client.uploadDocumentFile(file);
            var smartTagResp = client.sendSmartTagDocument(new List<string> {
                documentReference
            }, DateTime.Now.AddDays(7));
        }

        public static void SmartTagAdvancedUsage1_EmailTemplate(ServiceClient client)
        {
            var invitationTemplates = client.getInvitationEmailTemplates();
            //Let's say there're 2 templates, take the first one
            var templateReference = invitationTemplates[0].Reference;

            var file = new System.IO.FileInfo(SampleParameters.Path2SmartTagDocument);
            var documentReference = client.uploadDocumentFile(file);
            var smartTagResp = client.sendSmartTagDocument(new List<string> {
                documentReference
            }, DateTime.Now.AddDays(7), new SmartTagOptions
            {
                InvitationEmailTemplateReference = templateReference
            });
        }

        public static void SmartTagAdvancedUsage2_Embedded(ServiceClient client)
        {
            var file = new System.IO.FileInfo(SampleParameters.Path2SmartTagDocument);
            var documentReference = client.uploadDocumentFile(file);

            var smartTagResp = client.sendSmartTagDocument(new List<string> {
                documentReference
            }, DateTime.Now.AddDays(7), new SecuredSigningClientSdk.Models.SmartTagOptions { Embedded = true });

            var signingKey = smartTagResp[0].Signers[0].SigningKey;
            //populate signing key into a embedded signing webpage.
        }

        public static void SmartTagAdvancedUsage3_InviteeDetails(ServiceClient client)
        {
            var file = new System.IO.FileInfo(SampleParameters.Path2SmartTagDocument);
            var documentReference = client.uploadDocumentFile(file);
            var signers = new SmartTagInvitee[]
            {
                new SmartTagInvitee
                {
                    FirstName=SampleParameters.Invitee1_FirstName,
                    LastName=SampleParameters.Invitee1_LastName,
                    Email=SampleParameters.Invitee1_Email,
                    //if sms needed
                    //MobileCountry=SampleParameters.Invitee1_MobileCountry,
                    //MobileNumber=SampleParameters.Invitee1_MobileNumber                   
                }
            };
            var smartTagResp = client.sendSmartTagDocument(new List<string> {
                documentReference
            }, DateTime.Now.AddDays(7), new SmartTagOptions
            {
                Signers = signers.ToList()
            });
        }

        public static void SmartTagAdvancedUsage4_Attachment(ServiceClient client)
        {
            var attachment = new System.IO.FileInfo(SampleParameters.Path2AttachmentFile);
            var attachmentReference = client.uploadAttachmentFile(attachment);

            var file = new System.IO.FileInfo(SampleParameters.Path2SmartTagDocument);
            var documentReference = client.uploadDocumentFile(file);

            var signers = new SmartTagInvitee[]
            {
                new SmartTagInvitee
                {
                    FirstName=SampleParameters.Invitee1_FirstName,
                    LastName=SampleParameters.Invitee1_LastName,
                    Email=SampleParameters.Invitee1_Email,
                    //if sms needed
                    //MobileCountry=SampleParameters.Invitee1_MobileCountry,
                    //MobileNumber=SampleParameters.Invitee1_MobileNumber,
                    Attachments=new List<string>
                    {
                        attachmentReference
                    }
                }
            };

            var smartTagResp = client.sendSmartTagDocument(new List<string> {
                documentReference
            }, DateTime.Now.AddDays(7), new SmartTagOptions { Signers = signers.ToList() });
        }
    }
}
