using SecuredSigningClientSdk.Requests;
using SecuredSigningClientSdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace SecuredSigningClientSdk
{
    public class ServiceClient
    {
        private JsonServiceClient _client;
        private readonly string apiKey;

        public ServiceClient(string serviceUrl, string version, string apiKey, string secret, string accessUrl)
        {
            this.apiKey = apiKey;

            _client = new JsonServiceClient(serviceUrl + "/" + version);


            _client.RequestFilter = httpReq =>
            {
                //create unix time stamp string
                var requestDate = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString();

                //create nonce

                var nonce = KeyGenerator.GetUniqueKey(32);

                httpReq.Headers.Add("X-CUSTOM-API-KEY", apiKey);
                httpReq.Headers.Add("X-CUSTOM-DATE", requestDate);
                httpReq.Headers.Add("X-CUSTOM-NONCE", nonce);
                httpReq.Headers.Add("X-CUSTOM-SIGNATURE", AuthHelper.CreateToken(apiKey, secret, requestDate, nonce));
                httpReq.Referer = accessUrl;
            };
        }

        #region Document
        /// <summary>
        /// Returns document with its status
        /// </summary>
        /// <param name="documentReference"></param>
        /// <returns></returns>
        public Document getStatus(string documentReference)
        {
            var result = _client.Get<Document>(new StatusRequest { DocumentReference = documentReference });

            return result;
        }

        /// <summary>
        /// Gets log for the document
        /// </summary>
        /// <param name="documentReference"></param>
        /// <returns></returns>
        public List<DocumentLog> getLog(string documentReference)
        {
            var result = _client.Get<List<DocumentLog>>(new LogRequest { DocumentReference = documentReference });

            return result;
        }

        /// <summary>
        /// Extend the document due date
        /// </summary>
        /// <param name="documentReference"></param>
        /// <param name="dueDate"></param>
        /// <param name="gmt"></param>
        /// <returns></returns>
        public Document extendDocument(string documentReference, DateTime dueDate, string gmt)
        {
            var result = _client.Post<Document>(new ExtendRequest()
            {
                DocumentReference = documentReference,
                DueDate = dueDate,
                GMT = gmt
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
            var result = _client.Post<Document>(new UploadRequest
            {
                File = file
            });
            return result.Reference;
        }

        /// <summary>
        /// Uploads a file by mulitpart form
        /// </summary>
        /// <param name="file"></param>
        /// <returns>document reference</returns>
        public string uploadDocumentFile(System.IO.FileInfo file)
        {
            var result = _client.PostFileWithRequest<Document>(file, new UploaderRequest());
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
        public List<Document> getActiveDocuments(string folder)
        {
            var result = _client.Get<List<Document>>(new GetActiveDocumentsRequest()
            {
                Folder = folder
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
            var result = _client.Get<List<FormDirect>>(new FormDirectRequest());

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
        /// Sends forms
        /// </summary>
        /// <param name="formsToSend"></param>
        /// <param name="dueDate"></param>
        /// <returns></returns>
        public List<Document> sendForms(List<FormDirect> formsToSend, DateTime dueDate)
        {
            var result = _client.Post<List<Document>>(new SendFormDirectRequest { Forms = formsToSend, DueDate = dueDate });

            return result;
        }

        /// <summary>
        /// Returns form direct url
        /// </summary>
        /// <param name="formReference"></param>
        /// <param name="fileType"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public string getFormData(string formReference, FormDataFileType fileType, string separator)
        {
            var result = _client.Get<FormDataResponse>(new FormDataRequest
            {
                DocumentReference = formReference,
                FormDataFileType = fileType.ToString(),
                Separator = separator
            });

            return result.Url;
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
        #endregion

        #region Smart Tag
        /// <summary>
        /// Sends smart tag documents
        /// </summary>
        /// <param name="documentReferences"></param>
        /// <param name="embedded"></param>
        /// <param name="dueDate"></param>
        /// <param name="emailTemplateReference"></param>
        /// <param name="workflowReference"></param>
        /// <returns></returns>
        public List<Document> sendSmartTagDocument(List<string> documentReferences, bool embedded, DateTime dueDate, string emailTemplateReference, string workflowReference, Uri returnUrl)
        {
            var result = _client.Post<List<Document>>(new SmartTagRequest
            {
                DocumentReferences = documentReferences,
                DueDate = dueDate,
                EmailTemplateReference = emailTemplateReference,
                Embedded = embedded,
                WorkflowReference = workflowReference,
                ReturnUrl = returnUrl == null ? null : returnUrl.ToString()
            });

            return result;
        }

        /// <summary>
        /// Sends mail merge document along with mail merge list data
        /// </summary>
        /// <param name="documentReference"></param>
        /// <param name="dueDate"></param>
        /// <param name="emailTemplateReference"></param>
        /// <param name="fileType"></param>
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
            var result = _client.Get<ProcessDocument>(new ProcessDocumentRequest
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
        public List<Document> sendFormFillerTemplates(List<FormFillerTemplate> templates, DateTime dueDate)
        {
            var gmt = TimeZoneInfo.Local.GetUtcOffset(dueDate).TotalMinutes.ToString("F0");

            var result = _client.Post(new SendFormFillerRequest
            {
                Templates = templates,
                DueDate = dueDate,
                GMT = gmt
            });
            return result;
        }
        #endregion

    }
}
