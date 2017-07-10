using ServiceStack;
using System.Collections.Generic;

namespace SecuredSigningClientSdk.Requests
{
    [Route("/Billing/Invoices", Verbs = "GET", Summary = "Get account information", Notes = "Get account information.")]

    public class InvoiceRequest : IReturn<List<Models.InvoiceInfo>>
    {

    }
    [Route("/Billing/InvoiceDetail/{InvoiceReference}", Verbs = "GET", Summary = "Get invoice details", Notes = "Get invoice details")]

    public class InvoiceDetailsRequest : IReturn<List<Models.InvoiceDetail>>
    {
        public string InvoiceReference { get; set; }
    }
}