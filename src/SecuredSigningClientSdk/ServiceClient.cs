using SecuredSigningClientSdk.Requests;
using SecuredSigningClientSdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack;
using static System.Net.WebRequestMethods;

namespace SecuredSigningClientSdk
{
    public class ServiceClient
    {
        static Version SDKVersion
        {
            get
            {
                return typeof(ServiceClient).Assembly.GetName().Version;
            }
        }
        protected JsonServiceClient _client;
        protected OAuth2Client _oauth2;
        public string APIKey { get; private set; }
        public string APISecret { get; private set; }
        public string ServiceBaseUrl { get; private set; }
        public string APIVersion { get; private set; }
        private string accessToken;
        private string sender;
        public string Sender
        {
            set { sender = value; }
        }
        /// <summary>
        /// Set Access Token
        /// </summary>
        public string AccessToken
        {
            set { accessToken = value; }
        }
        /// <summary>
        /// OAuth 2 Client to deal with authentication.
        /// </summary>        
        public OAuth2Client OAuth2 { get { return _oauth2; } }
        public class AuthHeaders
        {
            public string timestamp { get; internal set; }
            public string signature { get; internal set; }
            public string apiKey { get; internal set; }
            public string nonce { get; internal set; }
        }
        protected string GMT
        {
            get
            {
                return TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalMinutes.ToString("F0");
            }
        }
        public ServiceClient(string serviceUrl, string version, string apiKey, string secret, string accessUrl)
        {
            this.ServiceBaseUrl = serviceUrl;
            this.APIVersion = version;
            this.APIKey = apiKey;
            this.APISecret = secret;
            _client = new JsonServiceClient(serviceUrl + "/" + version);
            _client.UserAgent = "SecuredSigningSDK" + SDKVersion.ToString();
            _oauth2 = new OAuth2Client(new Uri(serviceUrl.Replace("api", "www")).GetLeftPart(UriPartial.Authority), apiKey, secret, accessUrl);
            _client.RequestFilter = httpReq =>
            {
                var headers = GenerateAuthHeaders();
                httpReq.Headers.Add("X-CUSTOM-API-KEY", headers.apiKey);
                httpReq.Headers.Add("X-CUSTOM-DATE", headers.timestamp);
                httpReq.Headers.Add("X-CUSTOM-NONCE", headers.nonce);
                httpReq.Headers.Add("X-CUSTOM-SIGNATURE", headers.signature);
                if (!string.IsNullOrEmpty(accessToken))
                    httpReq.Headers.Add(System.Net.HttpRequestHeader.Authorization, "Bearer " + accessToken);
                if (!string.IsNullOrEmpty(sender))
                    httpReq.Headers.Add("X-CUSTOM-SENDER", sender);
                httpReq.Referer = accessUrl;
            };
        }

        public AuthHeaders GenerateAuthHeaders()
        {
            var timestamp = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString();

            //create nonce

            var nonce = KeyGenerator.GetUniqueKey(32);
            return new AuthHeaders
            {
                timestamp = timestamp,
                signature = AuthHelper.CreateSignature(this.APIKey, this.APISecret, timestamp, nonce),
                apiKey = this.APIKey,
                nonce = nonce
            };
        }

        #region Account
        /// <summary>
        /// Get account information
        /// </summary>
        /// <returns></returns>
        public AccountInfo getAccountInfo()
        {
            return _client.Get(new AccountRequest());
        }
        /// <summary>
        /// Save sender
        /// </summary>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="clientReference"></param>
        /// <param name="details">email and name in details are ignored</param>
        /// <returns></returns>
        public string saveSender(string email, string firstName, string lastName, string clientReference = null, UserDetails details = null)
        {
            if (details == null)
            {
                details = new UserDetails();
            }
            details.Email = email;
            details.FirstName = firstName;
            details.LastName = lastName;
            return _client.Post(new SaveSenderRequest()
            {
                GMT = this.GMT,
                ClientReference = clientReference,
                User = details
            }).Reference;
        }
        #endregion

        #region Document
        /// <summary>
        /// Returns document with its status
        /// </summary>
        /// <param name="documentReference"></param>
        /// <param name="withDocumentLog"></param>
        /// <returns></returns>
        public Document getStatus(string documentReference, bool withDocumentLog = false)
        {
            var result = _client.Get(new StatusRequest
            {
                DocumentReference = documentReference,
                DocumentLog = withDocumentLog
            });
            return result;
        }

        /// <summary>
        /// Gets log for the document
        /// </summary>
        /// <param name="documentReference"></param>
        /// <returns></returns>
        public List<DocumentLog> getLog(string documentReference)
        {
            var result = _client.Get(new LogRequest { DocumentReference = documentReference });

            return result;
        }

        /// <summary>
        /// Extend the document due date
        /// </summary>
        /// <param name="documentReference"></param>
        /// <param name="dueDate"></param>
        /// <returns></returns>
        public Document extendDocument(string documentReference, DateTime dueDate)
        {
            var result = _client.Post(new ExtendRequest()
            {
                DocumentReference = documentReference,
                DueDate = dueDate.ToUniversalTime().ToString("o"),
                GMT = GMT
            });
            return result;
        }

        /// <summary>
        /// Update signer profile
        /// </summary>
        /// <param name="documentReference"></param>
        /// <param name="signers"></param>
        /// <returns></returns>
        public Document updateSigners(string documentReference, Signer[] signers)
        {
            var result = _client.Post<Document>(new SignerRequest
            {
                DocumentReference = documentReference,
                Signers = signers
            });
            return result;
        }

        /// <summary>
        /// Uploads a file by Url
        /// </summary>
        /// <param name="file"></param>
        /// <returns>document reference</returns>
        public string uploadDocumentByUrl(FileInfo file)
        {
            var result = _client.Post(new UploadRequest
            {
                File = file
            });
            return result.Reference;
        }

        /// <summary>
        /// Uploads a file by mulitpart form
        /// </summary>
        /// <param name="file"></param>
        /// <param name="clientReference"></param>
        /// <returns>document reference</returns>
        public string uploadDocumentFile(System.IO.FileInfo file, string clientReference)
        {
            var result = _client.PostFileWithRequest<Document>(file, new UploaderRequest()
            {
                ClientReference = clientReference
            });
            return result.Reference;
        }
        /// <summary>
        /// Uploads a file by mulitpart form 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string uploadDocumentFile(System.IO.FileInfo file)
        {
            var result = _client.PostFileWithRequest<Document>(file, new UploaderRequest());
            return result.Reference;
        }
        /// <summary>
        /// Uploads a document from stream
        /// </summary>
        /// <param name="documentName"></param>
        /// <param name="document"></param>
        /// <param name="clientReference"></param>
        /// <returns></returns>
        public string uploadDocument(string documentName, System.IO.Stream document, string clientReference = null)
        {
            var result = _client.PostFileWithRequest<Document>(document, documentName, new UploaderRequest()
            {
                ClientReference = clientReference
            });
            return result.Reference;
        }
        /// <summary>
        /// Returns document url for downloading
        /// </summary>
        /// <param name="documentReference"></param>
        /// <returns></returns>
        public string getDocumentUrl(string documentReference)
        {
            var result = _client.Post<DocumentResponse>(new DocumentRequest { DocumentReference = documentReference });

            return result.Url;
        }

        /// <summary>
        /// validate document
        /// </summary>
        /// <param name="documentReference"></param>
        /// <returns></returns>
        public DocumentValidationResponse validateDocument(string documentReference)
        {
            var result = _client.Post<DocumentValidationResponse>(new DocumentValidationRequest { DocumentReference = documentReference });
            return result;
        }

        /// <summary>
        /// validate document file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public DocumentValidationResponse validateDocumentFile(System.IO.FileInfo file)
        {
            var result = _client.PostFileWithRequest<DocumentValidationResponse>(file, new DocumentFileValidationRequest());
            return result;
        }

        /// <summary>
        /// Returns active documents for account
        /// </summary>
        /// <returns></returns>
        public List<Document> getActiveDocuments(string folder, bool withDocumentLog = false)
        {
            var result = _client.Get(new GetActiveDocumentsRequest()
            {
                Folder = folder,
                DocumentLog = withDocumentLog
            });

            return result;
        }
        /// <summary>
        /// download document file
        /// </summary>
        /// <param name="documentReference"></param>
        /// <returns></returns>
        public byte[] getDocumentData(string documentReference)
        {
            var result = _client.Get<byte[]>(new DownloadDocumentRequest() { DocumentReference = documentReference });
            return result;
        }
        /// <summary>
        /// Send reminder email to invitee
        /// </summary>
        /// <param name="docRef"></param>
        /// <param name="signerRef"></param>
        public void SendReminder(string docRef, string signerRef)
        {
            _client.Post(new SendReminderRequest
            {
                DocumentReference = docRef,
                SignerReference = signerRef
            });
        }
        /// <summary>
        /// Get fields values for smart tag and form filler document after signed
        /// </summary>
        /// <param name="documentReference"></param>
        /// <returns></returns>
        public List<FormFillerField> getFieldData(string documentReference)
        {
            var result = _client.Get(new FieldDataRequest
            {
                DocumentReference = documentReference
            });
            return result;
        }
        /// <summary>
        /// Delete the document
        /// </summary>
        /// <param name="documentReference"></param>
        /// <returns></returns>
        public Document deleteDocument(string documentReference)
        {
            var result = _client.Post(new DeleteRequest
            {
                DocumentReference = documentReference
            });
            return result;
        }
        #endregion

        #region Form Direct
        /// <summary>
        /// Returns forms for account
        /// </summary>
        /// <returns></returns>
        public List<FormDirect> getFormList()
        {
            var result = _client.Get(new FormDirectRequest());

            return result;
        }

        /// <summary>
        /// Returns form direct object by its reference.
        /// </summary>
        /// <param name="formReference"></param>
        /// <returns></returns>
        public FormDirect getSingleForm(string formReference)
        {
            var result = _client.Get<FormDirect>(new SingleFormDirectRequest { FormReference = formReference });

            return result;
        }
        /// <summary>
        /// Returns predefined form fields for a form
        /// </summary>
        /// <param name="formReference"></param>
        /// <returns></returns>
        public FormFieldResponse getPredefinedFormFields(string formReference)
        {
            var result = _client.Get<FormFieldResponse>(new FormFieldsRequest { FormReference = formReference });
            return result;
        }
        /// <summary>
        /// Sends forms
        /// </summary>
        /// <param name="formsToSend"></param>
        /// <param name="dueDate"></param>
        /// <returns></returns>
        public List<Document> sendForms(List<FormDirect> formsToSend, DateTime dueDate)
        {
            return sendForms(formsToSend, dueDate, string.Empty, null);
        }
        /// <summary>
        /// Send forms with more options
        /// </summary>
        /// <param name="formsToSend"></param>
        /// <param name="dueDate"></param>
        /// <param name="invitationEmailTemplate"></param>
        /// <param name="dropDownListItems"></param>
        /// <returns></returns>
        public List<Document> sendForms(List<FormDirect> formsToSend, DateTime dueDate,string invitationEmailTemplate,List<DropDownListItem> dropDownListItems)
        {
            var result = _client.Post(new SendFormDirectRequest
            {
                Forms = formsToSend,
                DueDate = dueDate.ToUniversalTime().ToString("o"),
                GMT = this.GMT,
                InvitationEmailTemplateReference = invitationEmailTemplate,
                ListItems = dropDownListItems == null ? new List<DropDownListItem>() : dropDownListItems
            });

            return result;
        }
        /// <summary>
        /// Returns document signer link
        /// </summary>
        /// <param name="documentReference"></param>
        /// <param name="signerEmail"></param>
        /// <param name="signerFirstName"></param>
        /// <param name="signerLastName"></param>
        /// <returns></returns>
        public Signer getSignerLink(string documentReference, string signerEmail, string signerFirstName, string signerLastName)
        {
            var result = _client.Post<Signer>(new LinkRequest
            {
                DocumentReference = documentReference,
                Signer = new Signer
                {
                    Email = signerEmail,
                    FirstName = signerFirstName,
                    LastName = signerLastName,
                }
            });

            return result;
        }
        /// <summary>
        /// Get FormData for that specific Document
        /// </summary>
        /// <param name="documentReference"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public byte[] exportFormData(string documentReference, FormDataFileType returnType)
        {
            return _client.Get(new ExportRequest
            {
                DocumentReference = documentReference,
                FormDataFileType = returnType.ToString()
            });
        }
        /// <summary>
        /// Returns employer details for public forms
        /// </summary>
        /// <returns></returns>
        public Employers getEmployers()
        {
            var result = _client.Get(new EmployerListRequest());
            return result;
        }
        /// <summary>
        /// Save employer details for AU public forms
        /// </summary>
        /// <param name="superFundEmployers"></param>
        /// <param name="tfnEmployers"></param>
        /// <returns></returns>
        public Employers saveEmployers(List<SuperFundInfo> superFundEmployers, List<TFNInfo> tfnEmployers)
        {
            return _client.Post(new UpdateEmployerRequest
            {
                SuperFund = superFundEmployers == null ? new List<SuperFundInfo>() : superFundEmployers,
                TFN = tfnEmployers == null ? new List<TFNInfo>() : tfnEmployers
            });
        }
        /// <summary>
        /// Save employer details for NZ public forms
        /// </summary>
        /// <param name="accEmployers"></param>
        /// <returns></returns>
        public Employers saveEmployers(List<AccClaimsHistoryInfo> accEmployers)
        {
            return _client.Post(new UpdateEmployerRequest
            {
                AccClaimsHistory = accEmployers == null ? new List<AccClaimsHistoryInfo>() : accEmployers
            });
        }
        #endregion

        #region Smart Tag
        /// <summary>
        /// Sends smart tag documents - simple
        /// <see cref="http://www.securedsigning.com/documentation/developer/smarttag-api" />
        /// </summary>
        /// <param name="documentReferences"></param>
        /// <param name="dueDate"></param>
        /// <returns></returns>
        public List<Document> sendSmartTagDocument(List<string> documentReferences, DateTime dueDate)
        {
            var result = _client.Post(new SmartTagRequest
            {
                DocumentReferences = documentReferences,
                DueDate = dueDate.ToUniversalTime().ToString("o"),
                GMT = this.GMT
            });

            return result;
        }

        /// <summary>
        /// <see cref="http://www.securedsigning.com/documentation/developer/smarttag-api#adv1"/>
        /// </summary>
        /// <param name="documentReferences"></param>
        /// <param name="dueDate"></param>
        /// <param name="invitationEmailTemplateReference"></param>
        /// <returns></returns>
        [Obsolete("Use sendSmartTagDocument(List<string>, DateTime, SmartTagOptions) instead.")]
        public List<Document> sendSmartTagDocument(List<string> documentReferences, DateTime dueDate, string invitationEmailTemplateReference)
        {
            var result = _client.Post<List<Document>>(new SmartTagRequest
            {
                DocumentReferences = documentReferences,
                DueDate = dueDate.ToUniversalTime().ToString("o"),
                GMT = this.GMT,
                InvitationEmailTemplateReference = invitationEmailTemplateReference
            });

            return result;
        }
        /// <summary>
        /// <see cref="http://www.securedsigning.com/documentation/developer/smarttag-api#adv2"/>
        /// </summary>
        /// <param name="documentReferences"></param>
        /// <param name="dueDate"></param>
        /// <param name="embedded"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns
        [Obsolete("Use sendSmartTagDocument(List<string>, DateTime, SmartTagOptions) instead.")]
        public List<Document> sendSmartTagDocument(List<string> documentReferences, DateTime dueDate, bool embedded, Uri returnUrl = null)
        {
            var result = _client.Post<List<Document>>(new SmartTagRequest
            {
                DocumentReferences = documentReferences,
                DueDate = dueDate.ToUniversalTime().ToString("o"),
                GMT = this.GMT,
                Embedded = embedded,
                ReturnUrl = returnUrl?.ToString()
            });

            return result;
        }

        /// <summary>
        /// <see cref="http://www.securedsigning.com/documentation/developer/smarttag-api#adv3"/>
        /// </summary>
        /// <param name="documentReferences"></param>
        /// <param name="dueDate"></param>
        /// <param name="signers"></param>
        /// <param name=""></param>
        /// <returns></returns>
        [Obsolete("Use sendSmartTagDocument(List<string>, DateTime, SmartTagOptions) instead.")]
        public List<Document> sendSmartTagDocument(List<string> documentReferences, DateTime dueDate, SmartTagInvitee[] signers)
        {
            var result = _client.Post<List<Document>>(new SmartTagRequest
            {
                DocumentReferences = documentReferences,
                DueDate = dueDate.ToUniversalTime().ToString("o"),
                GMT = this.GMT,
                Signers = signers.ToList()
            });

            return result;
        }

        /// <summary>
        /// <see cref="http://www.securedsigning.com/documentation/developer/smarttag-api#adv3"/>
        /// </summary>
        /// <param name="documentReferences"></param>
        /// <param name="dueDate"></param>
        /// <param name="signers"></param>
        /// <param name="embedded"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [Obsolete("Use sendSmartTagDocument(List<string>, DateTime, SmartTagOptions) instead.")]
        public List<Document> sendSmartTagDocument(List<string> documentReferences, DateTime dueDate, SmartTagInvitee[] signers, bool embedded, Uri returnUrl)
        {
            var result = _client.Post<List<Document>>(new SmartTagRequest
            {
                DocumentReferences = documentReferences,
                DueDate = dueDate.ToUniversalTime().ToString("o"),
                GMT = this.GMT,
                Embedded = embedded,
                ReturnUrl = returnUrl?.ToString(),
                Signers = signers.ToList()
            });

            return result;
        }
        /// <summary>
        /// <see cref="http://www.securedsigning.com/documentation/developer/smarttag-api#adv3"/>
        /// </summary>
        /// <param name="documentReferences"></param>
        /// <param name="dueDate"></param>
        /// <param name="signers"></param>
        /// <param name="invitationEmailTemplateReference"></param>
        /// <returns></returns>
        [Obsolete("Use sendSmartTagDocument(List<string>, DateTime, SmartTagOptions) instead.")]
        public List<Document> sendSmartTagDocument(List<string> documentReferences, DateTime dueDate, SmartTagInvitee[] signers, string invitationEmailTemplateReference)
        {
            var result = _client.Post<List<Document>>(new SmartTagRequest
            {
                DocumentReferences = documentReferences,
                DueDate = dueDate.ToUniversalTime().ToString("o"),
                GMT = this.GMT,
                InvitationEmailTemplateReference = invitationEmailTemplateReference,
                Signers = signers.ToList()
            });

            return result;
        }
        /// <summary>
        /// Sends smart tag documents - with custom options
        /// <see cref="http://www.securedsigning.com/documentation/developer/smarttag-api"/>
        /// </summary>
        /// <param name="documentReferences"></param>
        /// <param name="dueDate"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public List<Document> sendSmartTagDocument(List<string> documentReferences, DateTime dueDate, SmartTagOptions options)
        {
            var result = _client.Post(new SmartTagRequest
            {
                DocumentReferences = documentReferences,
                DueDate = dueDate.ToUniversalTime().ToString("o"),
                GMT = this.GMT,
                InvitationEmailTemplateReference = options.InvitationEmailTemplateReference,
                CompletionEmailTemplateReference = options.CompletionEmailTemplateReference,
                Embedded = options.Embedded,
                ListItems = options.ListItems,
                NoPackage = options.NoPackage,
                ReturnUrl = options.ReturnUrl,
                Signers = options.Signers,
                NotifyUrl = options.NotifyUrl,
                PackageName = options.PackageName,
                ShareUsers = options.ShareUsers
            });

            return result;
        }

        /// <summary>
        /// Sends mail merge document along with mail merge list data
        /// </summary>
        /// <param name="documentReference"></param>
        /// <param name="dueDate"></param>
        /// <param name="emailTemplateReference"></param>
        /// <param name="mailMergeFileType"></param>
        /// <param name="mailMergeListData"></param>
        /// <returns></returns>
        public ProcessDocument sendMailMerge(string documentReference, DateTime dueDate, string emailTemplateReference, string mailMergeFileType, byte[] mailMergeListData, bool embedded, Uri returnUrl)
        {
            var result = _client.Post<ProcessDocument>(new MailMergeRequest
            {
                DocumentReference = documentReference,
                DueDate = dueDate,
                EmailTemplateReference = emailTemplateReference,
                MailMergeListFileData = Convert.ToBase64String(mailMergeListData),
                MailMergeListFileType = mailMergeFileType,
                Embedded = embedded,
                ReturnUrl = returnUrl.ToString()
            });

            return result;
        }

        /// <summary>
        /// Gets mail merge documents for process documents
        /// </summary>
        /// <param name="processDocumentReference"></param>
        /// <returns></returns>
        public ProcessDocument getMailMergeDocuments(string processDocumentReference)
        {
            var result = _client.Get(new ProcessDocumentRequest
            {
                ProcessDocumentReference = processDocumentReference
            });

            return result;
        }
        #endregion

        #region Email Template
        /// <summary>
        /// Gets invitation email templates for the account
        /// </summary>
        /// <returns></returns>
        public List<EmailTemplate> getInvitationEmailTemplates()
        {
            var result = _client.Get<List<EmailTemplate>>(new EmailTemplateRequest());

            return result;
        }
        /// <summary>
        /// Gets completion email templates for the account
        /// </summary>
        /// <returns></returns>
        public List<EmailTemplate> getCompletionEmailTemplates()
        {
            var result = _client.Get<List<EmailTemplate>>(new CompletionEmailTemplateRequest());

            return result;
        }
        #endregion

        #region Workflow
        /// <summary>
        /// Get smart tag's workflow for the account
        /// </summary>
        /// <returns></returns>
        public List<Workflow> getSmartTagWorkflows()
        {
            var result = _client.Get<List<Workflow>>(new WorkflowRequest());

            return result;
        }
        #endregion

        #region FormFiller
        public List<FormFillerTemplate> getFormFillerTemplates()
        {
            var result = _client.Get(new FormFillerRequest());
            return result;
        }
        public FormFillerTemplate getFormFillerSignerTemplate(string templateRef)
        {
            var result = _client.Get(new FormFillerSignerRequest() { TemplateReference = templateRef });
            return result;
        }
        public FormFillerTemplate getFormFillerFieldTemplate(string templateRef)
        {
            var result = _client.Get(new FormFillerFieldRequest { TemplateReference = templateRef });
            return result;
        }
        public DocumentResponse sendFormFillerTemplates(List<FormFillerTemplate> templates, DateTime dueDate, bool embedded = false, Uri returnUrl = null)
        {
            var result = _client.Post(new SendFormFillerRequest
            {
                Templates = templates,
                DueDate = dueDate.ToUniversalTime().ToString("o"),
                GMT = this.GMT,
                Embedded = embedded,
                ReturnUrl = returnUrl?.ToString()
            });
            return result;
        }
        #endregion

        #region Attachment
        /// <summary>
        /// Uploads a file as attachment by URL
        /// </summary>
        /// <param name="file"></param>
        /// <returns>document reference</returns>
        public string uploadAttachmentByUrl(AttachmentFileInfo file)
        {
            var result = _client.Post(new UploadAttachmentRequest
            {
                File = file
            });
            return result.Reference;
        }

        /// <summary>
        /// Uploads a file as attachment by mulitpart form
        /// </summary>
        /// <param name="file"></param>
        /// <returns>document reference</returns>
        public string uploadAttachmentFile(System.IO.FileInfo file, string number = "", string category = "")
        {
            var result = _client.PostFileWithRequest<AttachmentResponse>(file, new AttachmentUploaderRequest()
            {
                Number = number,
                Category = category
            });
            return result.Reference;
        }

        /// <summary>
        /// Uploads binary data as attachment by mulitpart form
        /// </summary>
        /// <param name="attachmentName"></param>
        /// <param name="attachment"></param>
        /// <param name="number"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public string uploadAttachment(string attachmentName, System.IO.Stream attachment, string number = "", string category = "")
        {
            var result = _client.PostFileWithRequest<AttachmentResponse>(attachment, attachmentName, new AttachmentUploaderRequest()
            {
                Number = number,
                Category = category
            });
            return result.Reference;
        }

        /// <summary>
        /// Returns account user's available attachments
        /// </summary>
        /// <returns></returns>
        public List<AttachmentResponse> getAttachments()
        {
            return _client.Get(new GetAttachmentsRequest());
        }
        /// <summary>
        /// Delete attachment with POST method.
        /// </summary>
        /// <param name="attachmentRef"></param>
        public void deleteAttachment2(string attachmentRef)
        {
            _client.Post(new DeleteAttachmentRequest { AttachmentReference = attachmentRef });
        }
        /// <summary>
        /// Delete attachment
        /// </summary>
        /// <param name="attachmentRef"></param>
        public void deleteAttachment(string attachmentRef)
        {
            _client.Delete(new DeleteAttachmentRequest { AttachmentReference = attachmentRef });
        }
        /// <summary>
        /// get attachment data
        /// </summary>
        /// <param name="attachmentReference"></param>
        /// <returns></returns>
        public byte[] getAttachmentData(string attachmentReference)
        {
            var result = _client.Get<byte[]>(new DownloadAttachmentRequest() { AttachmentReference = attachmentReference });
            return result;
        }
        #endregion

        #region Recipient
        /// <summary>
        /// get recipient
        /// </summary>
        /// <returns></returns>
        public List<RecipientsResponse> getRecipients()
        {
            return _client.Get(new GetRecipientsRequest());
        }
        #endregion

        #region Billing
        public List<InvoiceInfo> getInvoices()
        {
            return _client.Get(new InvoiceRequest());
        }
        public byte[] getInvoiceFile(string invoiceReference)
        {
            return _client.Get<byte[]>(new DownloadInvoiceRequest()
            {
                InvoiceReference = invoiceReference
            });
        }
        public List<InvoiceDetail> getInvoiceDetails(string invoiceReference)
        {
            return _client.Get(new InvoiceDetailsRequest()
            {
                InvoiceReference = invoiceReference
            });
        }
        #endregion
    }
}
