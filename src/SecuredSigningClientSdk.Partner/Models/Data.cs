using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecuredSigningClientSdk.Partner.Models
{
    public class MembershipResponse
    {
        public string MembershipCode { get; set; }
        public string Reference { get; set; }
        public string UpdateKey { get; set; }
        public string Result { get; set; }
    }
    public class AddAccountUserResponse
    {
        public string Result { get; set; }
        public string ConnectKey { get; set; }
    }
    public class PlanResponse
    {
        public string PlanName { get; set; }
        public string PlanType { get; set; }
        public int PlanUsers { get; set; }
        public int PlanDocuments { get; set; }
        public string DocumentPrice { get; set; }
        public string PlanPrice { get; set; }
        public string AccountPrice { get; set; }
        public bool Paid { get; set; }
        public string BaseDocumentPrice { get; set; }
        public string BaseUserPrice { get; set; }
        public string PlanExpiryDate { get; set; }
    }
    public class Company
    {
        public string CompanyName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactEmail { get; set; }
        public int GMTOffset { get; set; }
    }
    public class UserDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string LegalName { get; set; }
        public string Website { get; set; }
        public string Industry { get; set; }
        public string Employees { get; set; }
        public string Street { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string PhoneCountry { get; set; }
        public string PhoneArea { get; set; }
        public string PhoneNumber { get; set; }
        public string Title { get; set; }
    }
    public class PlanDetails
    {
        public int PlanUsers { get; set; }
        public int PlanDocuments { get; set; }
    }

}
