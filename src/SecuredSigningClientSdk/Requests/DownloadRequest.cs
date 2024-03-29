﻿using SecuredSigningClientSdk.Models;
using ServiceStack;
using System.Collections.Generic;
using System.Net;

namespace SecuredSigningClientSdk.Requests
{
    [Route("/Download/FormData/{DocumentReference}/{FormDataFileType}", Verbs = "GET", Summary = "Get FormData for that specific Document", Notes = "choose different export options (csv, xls, xlsx, xml), if it has Xslt set for that Form, it will apply automatically.")]
    public class DownloadFormDataRequest : IReturn<object>
    {
        [ApiMember(Description = "Document reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }

        [ApiMember(Description = "Form data file return type", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        [ApiAllowableValues("FormDataFileType", typeof(FormDataFileType))]
        public string FormDataFileType { get; set; }
    }

    [Route("/Download/GetDocumentData/{DocumentReference}", Verbs = "GET", Summary = "Returns document data", Notes = "Returns the document data as a stream. The document may not be found due to it being removed from Secured Signing according to our data retention policy.")]
    public class DownloadDocumentRequest : IReturn<object>
    {
        [ApiMember(Description = "Document reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }
    }
    [Route("/Download/GetUploadedFileData/{DocumentReference}", Verbs = "GET", Summary = "Returns uploaded file data, always in zip", Notes = "Returns the document data as a stream. The document may not be found due to it being removed from Secured Signing according to our data retention policy.")]
    public class DownloadUploadedFileRequest : IReturn<object>
    {
        [ApiMember(Description = "Document reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }
    }
    [Route("/Download/Attachment/{AttachmentReference}", Verbs = "GET", Summary = "Returns attachment file data", Notes = "Returns attachment file data")]
    public class DownloadAttachmentRequest : IReturn<object>
    {
        [ApiMember(Description = "Document reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string AttachmentReference { get; set; }
    }
    [Route("/Download/Invoice/{InvoiceReference}", Verbs = "GET", Summary = "Returns invoice file data", Notes = "Returns invoice file data")]
    public class DownloadInvoiceRequest : IReturn<object>
    {
        [ApiMember(Description = "Invoice reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string InvoiceReference { get; set; }
    }

    [Route("/Download/ENotaryJournal/{DocumentReference}/{ENotaryJournalDataType}", Verbs = "GET", Summary = "Returns Notary journal data", Notes = "Returns Notary journal data in pdf or json")]
    public class DownloadENotaryJournalRequest : IReturn<object>
    {
        [ApiMember(Description = "Document reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }

        [ApiMember(Description = "ID Verification data return type", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        [ApiAllowableValues("ENotaryJournalDataType", typeof(ENotaryJournalDataType))]
        public string ENotaryJournalDataType { get; set; }
    }

    [Route("/Download/IDVerification/{DocumentReference}/{IDVerificationDataType}", Verbs = "GET", Summary = "Returns ID Verification data", Notes = "Returns ID Verification data in pdf or json")]
    public class DownloadIDVerificationRequest : IReturn<object>
    {
        [ApiMember(Description = "Document reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }

        [ApiMember(Description = "ID Verification data return type", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        [ApiAllowableValues("IDVerificationDataType", typeof(IDVerificationDataType))]
        public string IDVerificationDataType { get; set; }
    }

    [Route("/Download/SigningCompletionCertificate/{CertificateReference}", Verbs = "GET", Summary = "Returns the specified Signing Completion Certificate")]
    public class DownloadSigningCompletionCertificateRequest : IReturn<object>
    {
        [ApiMember(Description = "Signing Completion Certificate reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string CertificateReference { get; set; }
    }

    [Route("/Download/VideoSigningRecording/{DocumentReference}", Verbs = "GET", Summary = "Returns Video Signing recording", Notes = "Returns Video Signing recording")]
    public class DownloadVideoSigningRecordingRequest : IReturn<object>
    {
        [ApiMember(Description = "Document reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }
    }
    [Route("/Download/Snapshot/{DocumentReference}", Verbs = "GET", Summary = "Returns the specified captured images")]
    public class DownloadCapturedImagesRequest : IReturn<object>
    {
        [ApiMember(Description = "Document reference", ParameterType = "path", DataType = SwaggerType.String, IsRequired = true)]
        public string DocumentReference { get; set; }
    }
}
