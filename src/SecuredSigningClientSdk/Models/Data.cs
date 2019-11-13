using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace SecuredSigningClientSdk.Models
{

    [Schema("OAuth2AccessToken")]
    public class OAuth2AccessToken
    {
        [ApiMember]
        public string Access_Token { get; set; }
        [ApiMember]
        public int Expires_In { get; set; }
        [ApiMember]
        public string Scope { get; set; }
        [ApiMember]
        public string Token_Type { get; set; }

        public string ToAuthorizationString()
        {
            return string.Join(" ", this.Token_Type, this.Token_Type);
        }
    }

    [Schema("FormDirect")]
    public class FormDirect
    {
        [ApiMember(Description = "Form name", DataType = SwaggerType.String, IsRequired = true)]
        public string Name { get; set; }
        [ApiMember(Description = "Form type", DataType = SwaggerType.Int, IsRequired = true)]
        public int FormType { get; set; }

        [ApiMember(Description = "Identifier", DataType = SwaggerType.String, IsRequired = true)]
        public string Reference { get; set; }

        [ApiMember(Description = "List of signers required for the form", AllowMultiple = true, IsRequired = true, DataType = "Signer")]
        public List<FormDirectInvitee> Signers { get; set; }

        [ApiMember(Description = "If true, the signing links will be used in an iFrame to access the forms", DataType = "boolean", IsRequired = true)]
        public bool EmbedForm { get; set; }

        [ApiMember(Description = "After signing a form, the page will redirect to the specified url", DataType = SwaggerType.String, IsRequired = false)]
        public string ReturnUrl { get; set; }
        /// <summary>
        /// *BETA, Only available in DSX
        /// </summary>
        [ApiMember(Description = "Notify Url.", DataType = SwaggerType.String, IsRequired = false)]
        public string NotifyUrl { get; set; }

        [ApiMember(Description = "Auto fill data for the form. It is an XML document converted to a string. Secured Signing creates the template for the data.", DataType = SwaggerType.String, IsRequired = false)]
        public string XMLData { get; set; }
        [ApiMember(Description = "Auto fill employer data for the form. It can be returned by FormDirect/Employers api. Secured Signing creates the template for the data.", DataType = SwaggerType.String, IsRequired = false)]
        public string EmployerReference { get; set; }

    }
    [Schema("UserInfo")]
    public class UserInfo
    {
        [ApiMember(Description = "First name of user", DataType = SwaggerType.String, IsRequired = true)]
        public string FirstName { get; set; }

        [ApiMember(Description = "Last name of user", DataType = SwaggerType.String, IsRequired = true)]
        public string LastName { get; set; }

        [ApiMember(Description = "Email address of user", DataType = SwaggerType.String, IsRequired = true)]
        public string Email { get; set; }
    }
    [Schema("Invitee")]
    public class Invitee : UserInfo
    {
        [ApiMember(Description = "Mobile number of signer, for SMS secured forms. Must include the mobile carrier code e.g. Australia 04, New Zealand 027 or 021 etc", DataType = SwaggerType.String, IsRequired = false)]
        public string MobileNumber { get; set; }

        [ApiMember(Description = "Mobile Country code for phone number e.g. Australia 61, New Zealand 64 etc", DataType = SwaggerType.String, IsRequired = false)]
        public string MobileCountry { get; set; }
        [ApiMember(Description = "Whether the signer using F2F signing", DataType = SwaggerType.Boolean, IsRequired = false)]
        public bool IsFaceToFaceSigning { get; set; }
        [ApiMember(Description = "ShareUsers for this user", DataType = SwaggerType.String, IsRequired = false)]
        public List<ShareUser> ShareUsers { get; set; }
    }
    [Schema("FormDirectInvitee")]
    public class FormDirectInvitee : Invitee
    {
        [ApiMember(Description = "Role of signer in signing process", DataType = SwaggerType.String, IsRequired = false)]
        public string SignerType { get; set; }
        [ApiMember(Description = "When the use of a mobile phone is required for authencation but is not available, enable this option to use known data about the signer they have to use to ID themselves", IsRequired = false, DataType = SwaggerType.Boolean, ExcludeInSchema = true)]
        public bool UseIDData { get; set; }

        [ApiMember(Description = "Data required for identifying signers with no access to mobile phones", IsRequired = false, DataType = "AuthInfo", ExcludeInSchema = true)]
        public List<AuthInfo> IDData { get; set; }
        [ApiMember(Description = "When the signer can be optional, tell API this signer is ignored or not.", IsRequired = false, DataType = SwaggerType.Boolean, ExcludeInSchema = true)]
        public bool Optional { get; set; }
        [ApiMember(Description = "If enable face to face signing for this signer or not", IsRequired = false, DataType = SwaggerType.Boolean, ExcludeInSchema = true)]
        public bool EnabledFaceToFaceSigning { get; set; }
        [ApiMember(Description = "If enable video confirmation for this signer or not", IsRequired = false, DataType = SwaggerType.Boolean, ExcludeInSchema = true)]
        public bool VideoConfirmation { get; set; }
    }
    [Schema("AuthInfo")]
    public class AuthInfo
    {
        [ApiMember(Description = "Description of the information required", DataType = SwaggerType.String, IsRequired = true)]
        public string Label { get; set; }

        [ApiMember(Description = "Value to be matched by invitee", DataType = SwaggerType.String, IsRequired = true)]
        public string Value { get; set; }

        [ApiMember(Description = "Data type", DataType = SwaggerType.String, IsRequired = true)]
        [ApiAllowableValues("DataType", typeof(DataType))]
        public string DataType { get; set; }
    }
    [Schema("FormDirectInvitee")]
    public class SmartTagInvitee : Invitee
    {
        [ApiMember(Description = "The reference of attachments")]
        public List<string> Attachments { get; set; }

        [ApiMember(Description = "Shows if embedded signing only for this invitee", DataType = SwaggerType.Boolean, IsRequired = false)]
        public bool? Embedded { get; set; }
        [ApiMember(Description = "Return Url only for this invitee if embedded", DataType = SwaggerType.String, IsRequired = false)]
        public string ReturnUrl { get; set; }

        [ApiMember(Description = "Invitation email template reference only for this invitee; Obsolete, use InvitationEmailTemplateReference instead;", DataType = SwaggerType.String, IsRequired = false)]
        [Obsolete]
        public string EmailTemplateReference
        {
            get
            {
                return InvitationEmailTemplateReference;
            }
            set
            {
                this.InvitationEmailTemplateReference = value;
            }
        }
        [ApiMember(Description = "Invitation email template reference only for this invitee", DataType = SwaggerType.String, IsRequired = false)]
        public string InvitationEmailTemplateReference { get; set; }
        [ApiMember(Description = "Completion email template reference only for this invitee", DataType = SwaggerType.String, IsRequired = false)]
        public string CompletionEmailTemplateReference { get; set; }
        [ApiMember(Description = "Tell if the InvitationText is for personal message or customized email template")]
        public bool IsPersonalMessage { get; set; }
        [ApiMember(Description = "Customized invitation email text or personal message")]
        public string InvitationText { get; set; }
        [ApiMember(Description = "Customized invitation email subject")]
        public string EmailSubject { get; set; }
    }
    [Schema("SmartTagOptions")]
    public class SmartTagOptions
    {
        [ApiMember(Description = "Shows if embedded signing", DataType = SwaggerType.Boolean, IsRequired = false)]
        public bool Embedded { get; set; }

        [ApiMember(Description = "Invitation Email template reference; Obsoleted, use InvitationEmailTemplateReference instead.", DataType = SwaggerType.String, IsRequired = false, ExcludeInSchema = true)]
        [Obsolete("use InvitationEmailTemplateReference instead.")]
        public string EmailTemplateReference
        {
            get { return this.InvitationEmailTemplateReference; }
            set { this.InvitationEmailTemplateReference = value; }
        }
        [ApiMember(Description = "Invitation Email template reference", DataType = SwaggerType.String, IsRequired = false)]
        public string InvitationEmailTemplateReference { get; set; }

        [ApiMember(Description = "Completion Email template reference", DataType = SwaggerType.String, IsRequired = false)]
        public string CompletionEmailTemplateReference { get; set; }

        [ApiMember(Description = "Return Url", DataType = SwaggerType.String, IsRequired = false)]
        public string ReturnUrl { get; set; }
        [ApiMember(Description = "Signer details, overwrite details populated in document", DataType = SwaggerType.Array, IsRequired = false)]
        public List<SmartTagInvitee> Signers { get; set; }

        [ApiMember(Description = "The list options for drop down list field smart tag; only work with client field", DataType = SwaggerType.Array, IsRequired = false)]
        public List<DropDownListItem> ListItems { get; set; }

        [ApiMember(Description = "Whether all documents are in a package (by default) or sent separately", DataType = SwaggerType.Boolean, IsRequired = false)]
        public bool NoPackage { get; set; }
        /// <summary>
        /// *BETA, Only available in DSX
        /// </summary>
        [ApiMember(Description = "Notify Url.", DataType = SwaggerType.String, IsRequired = false)]
        public string NotifyUrl { get; set; }
        [ApiMember(Description = "The name of the package; if empty and only one document in package, the name will be document name.", DataType = SwaggerType.String, IsRequired = false)]
        public string PackageName { get; set; }
        [ApiMember(Description = "Share user details, if no share user specified in document", DataType = SwaggerType.Array, IsRequired = false)]
        public List<ShareUser> ShareUsers { get; set; }

    }
    [Schema("DropDownListItem")]
    public class DropDownListItem
    {
        [ApiMember(Description = "The field name on client side", DataType = SwaggerType.String, IsRequired = true)]
        public string ClientField { get; set; }
        [ApiMember(Description = "Item name", DataType = SwaggerType.String, IsRequired = true)]
        public string Item { get; set; }
        [ApiMember(Description = "Item value", DataType = SwaggerType.String, IsRequired = true)]
        public string Value { get; set; }
    }
    [Schema("Signer")]
    public class Signer : Invitee
    {
        [ApiMember(Description = "Signer reference", DataType = SwaggerType.String, IsRequired = false)]
        public string SignerReference { get; set; }

        [ApiMember(Description = "Url for access to signing", DataType = SwaggerType.String)]
        public string SigningKey { get; set; }

        [ApiMember(Description = "User signing status", DataType = "boolean", IsRequired = false)]
        public bool HasSigned { get; set; }
        [ApiMember(Description = "Signer Status", IsRequired = false, DataType = SwaggerType.Boolean, ExcludeInSchema = true)]
        [ApiAllowableValues("SignedStatus", typeof(SignedStatus))]
        public SignedStatus SignedStatus { get; set; }

        [ApiMember(Description = "Declined reason. Return only when invitee declined to sign.", DataType = SwaggerType.String, IsRequired = false)]
        public string DeclinedReason { get; set; }

    }
    public class FormDirectSigner : Signer
    {
        [ApiMember(Description = "Role of signer in signing process", DataType = SwaggerType.String, IsRequired = false)]
        public string SignerType { get; set; }
    }
    [Schema("Document")]
    public class Document
    {
        [ApiMember(Description = "File Name", DataType = SwaggerType.String, IsRequired = true)]
        public string Name { get; set; }

        [ApiMember(Description = "Document reference, used for document access", DataType = SwaggerType.String, IsRequired = true)]
        public string Reference { get; set; }

        [ApiMember(Description = "Form reference", DataType = SwaggerType.String, IsRequired = false)]
        public string FormDirectReference { get; set; }

        [ApiMember(Description = "List of signers", AllowMultiple = true, IsRequired = true, DataType = "Signer")]
        public List<Signer> Signers { get; set; }

        [ApiMember(Description = "Document signing status", DataType = SwaggerType.String, IsRequired = false)]
        public string Status { get; set; }

        [ApiMember(Description = "Url to download document data", DataType = SwaggerType.String, IsRequired = false)]
        public string DocumentUrl { get; set; }

        [ApiMember(Description = "Due Date of document", DataType = SwaggerType.Date, IsRequired = false)]
        public string DueDate { get; set; }

        [ApiMember(Description = "Due Date of document in .NET DateTime format", DataType = SwaggerType.Date, IsRequired = false)]
        public DateTime? DueDate2
        {
            get
            {
                if (string.IsNullOrEmpty(DueDate))
                    return null;
                return DateTime.Parse(DueDate);
            }
        }

        [ApiMember(Description = "GMT Offset", DataType = SwaggerType.String, IsRequired = false)]
        public string GMT { get; set; }

        [ApiMember(Description = "Date of latest signature", DataType = SwaggerType.Date, IsRequired = false)]
        public string LastSignedDate { get; set; }
        [ApiMember(Description = "Date of latest signature in .NET DateTime format", DataType = SwaggerType.Date, IsRequired = false)]
        public DateTime? LastSignedDate2
        {
            get
            {
                if (string.IsNullOrEmpty(LastSignedDate))
                    return null;
                return DateTime.Parse(LastSignedDate);
            }
        }
        [ApiMember(Description = "List of document logs", IsRequired = false, DataType = SwaggerType.Array)]
        public List<DocumentLog> Logs { get; set; }
        [ApiMember(Description = "Whether invitee uploaded any files during signing process.", DataType = SwaggerType.Boolean, IsRequired = false)]
        public bool HasFileUploaded { get; set; }
    }
    public class FileInfoBase
    {
        [ApiMember(Description = "File Name", DataType = SwaggerType.String, IsRequired = true)]
        public string Name { get; set; }

        [ApiMember(Description = "File type of file", DataType = SwaggerType.String, IsRequired = true)]
        [ApiAllowableValues("FileType", typeof(FileType))]
        public string FileType { get; set; }

        [ApiMember(Description = "Url to download retrieve file data", DataType = SwaggerType.String, IsRequired = true)]
        public string FileUrl { get; set; }
    }
    [Schema("DocumentFileInfo")]
    public class FileInfo : FileInfoBase
    {
        [ApiMember(Description = "The reference of the document on client side", DataType = SwaggerType.String, IsRequired = false)]
        public string ClientReference { get; set; }
    }
    [Schema("AttachmentFileInfo")]

    public class AttachmentFileInfo : FileInfoBase
    {
        [ApiMember(Description = "Attachment Number, can only be digitals", DataType = SwaggerType.String, IsRequired = false)]
        public string Number { get; set; }
        [ApiMember(Description = "Attachment Category", DataType = SwaggerType.String, IsRequired = false)]
        public string Category { get; set; }

    }
    [Schema("DocumentLog")]
    public class DocumentLog
    {
        [ApiMember(Description = "Name of user responsible for action", DataType = SwaggerType.String, IsRequired = false)]
        public string Name { get; set; }

        [ApiMember(Description = "Email of user responsible for action", DataType = SwaggerType.String, IsRequired = false)]
        public string Email { get; set; }

        [ApiMember(Description = "Log entry", DataType = SwaggerType.String, IsRequired = false)]
        public string Action { get; set; }

        [ApiMember(Description = "Date of log entry", DataType = "date", IsRequired = false)]
        public DateTime Date { get; set; }

        [ApiMember(Description = "GMT Offset", DataType = SwaggerType.String, IsRequired = false)]
        public string GMT { get; set; }
    }

    [Schema("ProcessDocument")]
    public class ProcessDocument
    {
        [ApiMember(Description = "Document reference, used for document access", DataType = SwaggerType.String, IsRequired = true)]
        public string Reference { get; set; }

        [ApiMember(Description = "Processing status of mail merge", DataType = SwaggerType.String, IsRequired = true)]
        public string ProcessingStatus { get; set; }

        [ApiMember(Description = "List of documents for mail merge process", AllowMultiple = true, IsRequired = true, DataType = "Document")]
        public List<Document> Documents { get; set; }
    }

    [Schema("EmailTemplate")]
    public class EmailTemplate
    {
        [ApiMember(Description = "Email template's reference.", DataType = SwaggerType.String, IsRequired = true)]
        public string Reference { get; set; }

        [ApiMember(Description = "Email template's name.", DataType = SwaggerType.String, IsRequired = true)]
        public string Name { get; set; }
        [ApiMember(Description = "If the email template is default one or not", DataType = SwaggerType.Boolean, IsRequired = false)]
        public bool IsDefault { get; set; }
        [ApiMember(Description = "Email template's subject.", DataType = SwaggerType.String, IsRequired = true)]
        public string Subject { get; set; }
        [ApiMember(Description = "Email template's text.", DataType = SwaggerType.String, IsRequired = true)]
        public string Template { get; set; }
    }

    [Schema("Workflow")]
    public class Workflow
    {
        [ApiMember(Description = "Workflow's reference.", DataType = SwaggerType.String, IsRequired = true)]
        public string Reference { get; set; }

        [ApiMember(Description = "Workflow's name.", DataType = SwaggerType.String, IsRequired = true)]
        public string Name { get; set; }
    }
    [Schema("DocumentResponse")]
    public class DocumentResponse
    {
        [ApiMember(Description = "Url which file content will be downloaded", DataType = SwaggerType.String)]
        public string Url { get; set; }
        [ApiMember(Description = "Document or multiple documents objects, with data for signing", ParameterType = "path", DataType = "Document", AllowMultiple = false)]
        public List<Document> Documents { get; set; }
        public List<Signer> Signers { get; set; }

    }
    [Schema("DocumentValidationResponse")]
    public class DocumentValidationResponse
    {
        public List<VerifySignature> Signatures { get; set; }
        public string DocumentURL { get; set; }
        public string DocumentName { get; set; }
    }
    [Schema("VerifySignature")]
    public class VerifySignature
    {
        public bool isValid { get; set; }
        public string SignatureTime { get; set; }
        public string User { get; set; }
        public SignerResponse Signer { get; set; }
    }
    [Schema("SignerResponse")]
    public class SignerResponse
    {
        [ApiMember(Description = "First name of user", DataType = SwaggerType.String, IsRequired = true)]
        public string FirstName { get; set; }

        [ApiMember(Description = "Last name of user", DataType = SwaggerType.String, IsRequired = true)]
        public string LastName { get; set; }

        [ApiMember(Description = "Email address of user", DataType = SwaggerType.String, IsRequired = true)]
        public string Email { get; set; }

        [ApiMember(Description = "Role of signer in signing process", DataType = SwaggerType.String, IsRequired = false)]
        public string SignerType { get; set; }

        [ApiMember(Description = "Mobile number of signer, for SMS secured forms. Must include the mobile carrier code e.g. Australia 04, New Zealand 027 or 021 etc", DataType = SwaggerType.String, IsRequired = false)]
        public string MobileNumber { get; set; }

        [ApiMember(Description = "Mobile Country code for phone number e.g. Australia 61, New Zealand 64 etc", DataType = SwaggerType.String, IsRequired = false)]
        public string MobileCountry { get; set; }

        [ApiMember(Description = "User signing status", DataType = SwaggerType.Boolean, IsRequired = false)]
        public bool HasSigned { get; set; }

        public string Title { get; set; }
        public string Reason { get; set; }

        public string Company { get; set; }
    }
    [Schema("FormFillerField")]
    public class FormFillerField
    {
        public string Label { get; set; }
        public string Value { get; set; }
        public HTMLElementType FieldType { get; set; }
        public bool IsRequired { get; set; }
        public string ID { get; set; }
        public bool ReadOnly { get; set; }
    }
    [Schema("FormFillerTemplate")]
    public class FormFillerTemplate
    {
        [ApiMember(Description = "Template name", DataType = SwaggerType.String, IsRequired = true)]
        public string Name { get; set; }

        [ApiMember(Description = "Identifier", DataType = SwaggerType.String, IsRequired = true)]
        public string Reference { get; set; }

        [ApiMember(Description = "List of signers required for the template", AllowMultiple = true, IsRequired = true, DataType = "Signer")]
        public List<Signer> Signers { get; set; }

        public List<FormFillerField> Fields { get; set; }
    }
    [Schema("AccountInfo")]
    public class AccountInfo
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public int DocumentRemain { get; set; }
        public string PlanName { get; set; }
        public int DocumentUsed { get; set; }
        public int DocumentLeft { get; set; }
        public string PlanType { get; set; }
        public int DefaultDueDate { get; set; }
        public int MaxDueDate { get; set; }
        public string DateFormat { get; set; }
        public string Upgrade { get; set; }
        public string UserID { get; set; }
        public long MaxUploadSize { get; set; }
        public bool Actived { get; set; }
        public bool Locked { get; set; }
        public string AccountStatus { get; set; }
        public bool EnabledFaceToFaceSigning { get; set; }
        public bool EnabledVideoConfirmation { get; set; }
        public bool EnabledDocNegotiation { get; set; }
        public bool EnabledReviewBeforeSigning { get; set; }
    }
    public class UserReferenceResponse
    {
        public string Reference { get; set; }
    }
    [Schema("User Details")]
    public class UserDetails : UserInfo
    {
        [ApiMember(Description = "User's middle name", DataType = SwaggerType.String, IsRequired = false)]
        public string MiddleName { get; set; }
        [ApiMember(Description = "User's job title", DataType = SwaggerType.String, IsRequired = false)]
        public string JobTitle { get; set; }
        [ApiMember(Description = "User's company name", DataType = SwaggerType.String, IsRequired = false)]
        public string CompanyName { get; set; }
        [ApiMember(Description = "User's legal name", DataType = SwaggerType.String, IsRequired = false)]
        public string LegalName { get; set; }
        [ApiMember(Description = "User's website", DataType = SwaggerType.String, IsRequired = false)]
        public string Website { get; set; }
        [ApiMember(Description = "User's industry", DataType = SwaggerType.String, IsRequired = false)]
        public string Industry { get; set; }
        [ApiMember(Description = "User's employee number", DataType = SwaggerType.String, IsRequired = false)]
        public string Employees { get; set; }
        [ApiMember(Description = "User's street", DataType = SwaggerType.String, IsRequired = false)]
        public string Street { get; set; }
        [ApiMember(Description = "User's suburb", DataType = SwaggerType.String, IsRequired = false)]
        public string Suburb { get; set; }
        [ApiMember(Description = "User's city", DataType = SwaggerType.String, IsRequired = false)]
        public string City { get; set; }
        [ApiMember(Description = "User's post code / ZIP", DataType = SwaggerType.String, IsRequired = false)]
        public string Postcode { get; set; }
        [ApiMember(Description = "User's country", DataType = SwaggerType.String, IsRequired = false)]
        public string Country { get; set; }
        [ApiMember(Description = "User's state", DataType = SwaggerType.String, IsRequired = false)]
        public string State { get; set; }
        [ApiMember(Description = "User's phone number country part, e.g. +64", DataType = SwaggerType.String, IsRequired = false)]
        public string PhoneCountry { get; set; }
        [ApiMember(Description = "User's phone number area part", DataType = SwaggerType.String, IsRequired = false)]
        public string PhoneArea { get; set; }
        [ApiMember(Description = "User's phone number", DataType = SwaggerType.String, IsRequired = false)]
        public string PhoneNumber { get; set; }
        [ApiMember(Description = "User's title", DataType = SwaggerType.String, IsRequired = false)]
        public string Title { get; set; }
    }
    [Schema("FormField")]
    public class FormField
    {
        public string FieldName { get; set; }
        public string DisplayName { get; set; }
        public HTMLElementType FieldType { get; set; }
        public string FiledValue { get; set; }
        public List<string> ValueOptions { get; set; }
        public string ClientField { get; set; }
        public int ClientMapping { get; set; }
    }
    [Schema("Employers")]
    public class Employers
    {
        public List<SuperFundInfo> SuperFund { get; set; }
        public List<TFNInfo> TFN { get; set; }
        public List<AccClaimsHistoryInfo> AccClaimsHistory { get; set; }
    }
    [Schema("SuperFundInfo")]
    public class SuperFundInfo
    {
        public string ABN { get; set; }
        public string BusinessName { get; set; }
        public string SuperFundName { get; set; }
        public string USI { get; set; }
        public string Phone { get; set; }
        public string SuperFundWebsite { get; set; }
        [ApiMember(Description = "EmployerReference", DataType = SwaggerType.String, IsRequired = true)]
        public string Reference { get; set; }
        [ApiMember(Description = "Name for identify", DataType = SwaggerType.String, IsRequired = true)]
        public string Name { get; set; }
    }
    [Schema("TFNInfo")]
    public class TFNInfo
    {
        public string ABN { get; set; }
        public string BusinessName { get; set; }
        public string BusinessAddress { get; set; }
        public string BusinessSuburb { get; set; }
        public string BusinessState { get; set; }
        public string BusinessPostcode { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPhone { get; set; }
        [ApiMember(Description = "EmployerReference", DataType = SwaggerType.String, IsRequired = true)]
        public string Reference { get; set; }
        [ApiMember(Description = "Name for identify", DataType = SwaggerType.String, IsRequired = true)]
        public string Name { get; set; }
    }
    [Schema("AccClaimsHistoryInfo")]
    public class AccClaimsHistoryInfo
    {
        public string OrganisationName { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonPhone { get; set; }
        public string ContactPersonEmail { get; set; }
        [ApiMember(Description = "EmployerReference", DataType = SwaggerType.String, IsRequired = true)]
        public string Reference { get; set; }
        [ApiMember(Description = "Name for identify", DataType = SwaggerType.String, IsRequired = true)]
        public string Name { get; set; }
        [ApiMember(Description = "If it's the default employer", DataType = SwaggerType.Boolean, IsRequired = false)]
        public bool IsDefault { get; set; }
    }
    [Schema("Attachment Response")]
    public class AttachmentResponse : AttachmentFileInfo
    {
        [ApiMember(Description = "Attachment reference", DataType = SwaggerType.String, IsRequired = true)]
        public string Reference { get; set; }
    }
    public class ShareUser : UserInfo
    {
        public bool IsDefault { get; set; }
        public bool IsOwner { get; set; }
    }
    public class FormFieldResponse
    {
        public List<FormField> Fields { get; set; }
        [ApiMember(Description = "Date of last update", DataType = SwaggerType.Date, IsRequired = false)]
        public string LastUpdateTime { get; set; }
    }
    [Schema("Recipient")]
    public class Recipient : UserInfo
    {
        public string RecipientReference { get; set; }
        public bool IsDefault { get; set; }
    }
    [Schema("CompletionRecipient")]
    public class CompletionRecipient : Recipient
    {
    }
    [Schema("NotificationRecipient")]
    public class NotificationRecipient : Recipient
    {
        public bool IsReviewer { get; set; }
        public bool IsDefaultReviewer { get; set; }
    }
    [Schema("Recipients")]
    public class RecipientsResponse
    {
        public List<NotificationRecipient> NotificationRecipients { get; set; }
        public List<CompletionRecipient> CompletionRecipients { get; set; }
    }
    [Schema("ResultItem")]
    public class ResultItem
    {
        public string Reference { get; set; }
        public string Result { get; set; }
        public int ResultCode { get; set; }
    }
    [Schema("ResultResponse")]
    public class ResultResponse
    {
        public string Result { get; set; }
        public List<ResultItem> Results { get; set; }
    }
    [Schema("DeleteRecipientsResultResponse")]
    public class DeleteRecipientsResultResponse
    {
        public List<ResultItem> NotificationRecipientResults { get; set; }
        public List<ResultItem> CompletionRecipienResults { get; set; }

    }
    #region billing
    [Schema("Invoice")]
    public class InvoiceInfo
    {
        public string InvoiceReference { get; set; }
        public string InvoiceNO { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceTimezone { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public InvoiceType InvoiceType { get; set; }
    }
    [Schema("InvoiceDetail")]
    public class InvoiceDetail
    {
        public string InvoiceNO { get; set; }
        public string Description { get; set; }
        public InvoiceType InvoiceType { get; set; }

        public string StartDate { get; set; }
        public string StartDateTimezone { get; set; }
        public string EndDate { get; set; }
        public string EndDateTimezone { get; set; }

        public string InvoiceUser { get; set; }
        public string InvoiceMembership { get; set; }
        public string InvoicePartner { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Devices { get; set; }
        public int TotalDocuments { get; set; }
        public int NumberOfSMS { get; set; }
        public decimal IDCheckCost { get; set; }
        public int NumberOfNegotiation { get; set; }
        public List<KeyValuePair<string, int>> IDCheckDetails { get; set; }
    }
    #endregion
    public enum FileType
    {
        pdf,
        doc,
        docx,
        rtf

    }
    public enum FormDataFileType
    {
        xml,
        csv,
        xlsx,
        xls

    }
    public enum ServiceType
    {
        smarttag,
        mailmergelist

    }
    public enum MailMergeListFileType
    {
        csv,
        xlsx,
        xls
    }
    public enum Folder
    {
        InBox,
        Progress,
        Signed
    }
    public enum HTMLElementType
    {
        Text = 0,
        MultiLineText = 1,
        CheckBox = 3,
        DropDownList = 5,
        DateInput = 6
    }
    public enum SignedStatus
    {
        Initialized, //0
        Invitation, //1
        Reminder1, //2
        Reminder2, //3
        AutoExtend, //4
        Expired, //5
        Signed, //6
        Complete, //7
        Archived, //8
        Pending, //9
        Deleted, //10
        Rejected, //11
        Declined, //12
        Reminder, //13
    }
    public enum InvoiceType
    {
        None = 0,
        BuySMS = 1,
        BuyIdCheck = 2,
        Account = 3,
        MembershipAccount = 4,
        Membership = 5,
        Partner = 6,
        BuyDocumentNegotiation = 7,
        Sender = 8
    }
    public enum DataType
    {
        text,
        date,
        email
    }
}
