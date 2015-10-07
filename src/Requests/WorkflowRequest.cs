using SecuredSigningClientSdk.Models;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SecuredSigningClientSdk.Requests
{
    [Route("/Workflow/GetSmartTagWorkflows", Verbs = "GET", Summary = "Returns account user's smarttag's workflows.", Notes = "Returns account user's smarttag's workflows.")]
    public class WorkflowRequest : IReturn<List<Workflow>>
    {
    }
}