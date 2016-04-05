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

        [ApiMember(Description = "Identifier", DataType = SwaggerType.String, IsRequired = true)]
        public string Reference { get; set; }

        [ApiMember(Description = "List of signers required for the form", AllowMultiple = true, IsRequired = true, DataType = "Signer")]
        public List<FormDirectInvitee> Signers { get; set; }

        [ApiMember(Description = "If true, the signing links will be used in an iFrame to access the forms", DataType = "boolean", IsRequired = true)]
        public bool EmbedForm { get; set; }

        [ApiMember(Description = "After signing a form, the page will redirect to the specified url", DataType = SwaggerType.String, IsRequired = false)]
        public string ReturnUrl { get; set; }

        [ApiMember(Description = "Auto fill data for the form. It is an XML document converted to a string. Secured Signing creates the template for the data.", DataType = SwaggerType.String, IsRequired = false)]
        public string XMLData { get; set; }
    }
    [Schema("Invitee")]
    public class Invitee
    {
        [ApiMember(Description = "First name of user", DataType = SwaggerType.String, IsRequired = true)]
        public string FirstName { get; set; }

        [ApiMember(Description = "Last name of user", DataType = SwaggerType.String, IsRequired = true)]
        public string LastName { get; set; }

        [ApiMember(Description = "Email address of user", DataType = SwaggerType.String, IsRequired = true)]
        public string Email { get; set; }
        [ApiMember(Description = "Mobile number of signer, for SMS secured forms. Must include the mobile carrier code e.g. Australia 04, New Zealand 027 or 021 etc", DataType = SwaggerType.String, IsRequired = false)]
        public string MobileNumber { get; set; }

        [ApiMember(Description = "Mobile Country code for phone number e.g. Australia 61, New Zealand 64 etc", DataType = SwaggerType.String, IsRequired = false)]
        public string MobileCountry { get; set; }
    }
    [Schema("FormDirectInvitee")]
    public class FormDirectInvitee: Invitee
    {
        [ApiMember(Description = "Role of signer in signing process", DataType = SwaggerType.String, IsRequired = false)]
        public string SignerType { get; set; }
    }
    [Schema("FormDirectInvitee")]
    public class SmartTagInvitee : Invitee
    {
        public List<string> Attachments { get; set; }
    }

    [Schema("Signer")]
    public class Signer:Invitee
    {
        [ApiMember(Description = "Signer reference", DataType = SwaggerType.String, IsRequired = false)]
        public string SignerReference { get; set; }

        [ApiMember(Description = "Url for access to signing", DataType = SwaggerType.String)]
        public string SigningKey { get; set; }

        [ApiMember(Description = "User signing status", DataType = "boolean", IsRequired = false)]
        public bool HasSigned { get; set; }
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

        [ApiMember(Description = "File type of Smart tag", DataType = SwaggerType.String, IsRequired = false)]
        [ApiAllowableValues("FileType", typeof(FileType))]
        public string FileType { get; set; }

        [ApiMember(Description = "Form reference", DataType = SwaggerType.String, IsRequired = false)]
        public string FormDirectReference { get; set; }

        [ApiMember(Description = "List of signers", AllowMultiple = true, IsRequired = true, DataType = "Signer")]
        public List<Signer> Signers { get; set; }

        [ApiMember(Description = "Document signing status", DataType = SwaggerType.String, IsRequired = false)]
        public string Status { get; set; }

        [ApiMember(Description = "Purpose for document e.g. use for Smart tags", DataType = SwaggerType.String, IsRequired = false)]
        [ApiAllowableValues("ServiceType", typeof(ServiceType))]
        public string ServiceType { get; set; }

        [ApiMember(Description = "Url to download document data", DataType = SwaggerType.String, IsRequired = false)]
        public string DocumentUrl { get; set; }

        [ApiMember(Description = "Due Date of document", DataType = SwaggerType.Date, IsRequired = false)]
        public string DueDate { get; set; }

        [ApiMember(Description = "GMT Offset", DataType = SwaggerType.String, IsRequired = false)]
        public string GMT { get; set; }

        [ApiMember(Description = "Date of latest signature", DataType = SwaggerType.Date, IsRequired = false)]
        public string LastSignedDate { get; set; }
    }
    [Schema("FileInfo")]
    public class FileInfo
    {
        [ApiMember(Description = "File Name", DataType = SwaggerType.String, IsRequired = true)]
        public string Name { get; set; }

        [ApiMember(Description = "File type of file", DataType = SwaggerType.String, IsRequired = true)]
        [ApiAllowableValues("FileType", typeof(FileType))]
        public string FileType { get; set; }

        [ApiMember(Description = "Url to download retrieve file data", DataType = SwaggerType.String, IsRequired = true)]
        public string FileUrl { get; set; }


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
    }
    [Schema("Employers")]
    public class Employers
    {
        public List<SuperFundInfo> SuperFund { get; set; }
        public List<TFNInfo> TFN { get; set; }
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
        CheckBox = 3
    }
}
