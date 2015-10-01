using SecuredSigningClientSdk.Models;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SecuredSigningClientSdk.Requests
{
    [Route("/WeSign/UpdateProgress/", Verbs = "POST", Summary = "Update signers's details for in-progress document.", Notes = "Update signers's details for in-progress document.")]
    public class WeSignUpdateRequest : IReturn<Document>
    {
        [ApiMember(Description = "Document reference", ParameterType = "body", AllowMultiple = true, DataType = "string", IsRequired = true)]
        public string DocumentReference { get; set; }

        [ApiMember(Description = "Shows if embedded signing", ParameterType = "body", DataType = "bool", IsRequired = false)]
        public bool Embedded { get; set; }

        [ApiMember(Name = "DueDate", Description = "Due date that document are to be signed by.", ParameterType = "body", DataType = "date", IsRequired = true)]
        public DateTime DueDate { get; set; }

        [ApiMember(Name = "WeSigners", Description = "Signers of the document.", ParameterType = "body", DataType = "Signer", IsRequired = true)]
        public List<Signer> WeSigners { get; set; }
    }
}