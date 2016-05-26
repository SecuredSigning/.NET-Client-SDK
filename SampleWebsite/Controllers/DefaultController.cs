using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SampleWebsite.Models;
using System.Web.Mvc;

namespace SampleWebsite.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Sign(Signer signer1, Signer signer2)
        {
            var path = Server.MapPath("../Content/Contracts/doc.pdf");
            var docRef = SDK.client.uploadDocumentFile(new System.IO.FileInfo(path));
            var result = SDK.client.sendSmartTagDocument(new List<string> { docRef }, DateTime.Now.AddDays(7), new SecuredSigningClientSdk.Models.SmartTagInvitee[]
            {
                new SecuredSigningClientSdk.Models.SmartTagInvitee
                {
                    Email=signer1.Email,
                    FirstName=signer1.FirstName,
                    LastName=signer1.LastName,
                    Embedded=true
                },
                new SecuredSigningClientSdk.Models.SmartTagInvitee
                {
                    Email=signer2.Email,
                    FirstName=signer2.FirstName,
                    LastName=signer2.LastName,
                    Embedded =false
                },
            });
            var key = result[0].Signers[0].SigningKey;
            ViewBag.SigningKey = key;
            return View();
        }
    }
}