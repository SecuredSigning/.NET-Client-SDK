# Secured Signing .NET Client SDK
.NET Client SDK for Secured Signing

## About Secured Signing

Secured Signing provides a comprehensive and compliant Software as a Service platform that utilizes the secure, personalised, X509 PKI Digital Signature technology and enables the customisation of a full range of eForm and eSignature capabilities.

* [Secured Signing (www.securedsigning.com)](http://www.securedsigning.com/)
* [Developer Page](https://www.securedsigning.com/developer/api-documentation)

## Install our .NET client SDK

### [Install via NuGet](https://www.nuget.org/packages/securedsigning.client/).

## Initialise the library

```csharp
var client = new ServiceClient("https://api.securedsigning.com/web","v1.4", <YOUR API KEY HERE>, <YOUR API SECRET HERE>);
```

## Look at Data Objects
We have provided data objects for the requests e.g.

```csharp
[Schema("EmailTemplate")]
public class EmailTemplate
{
    public string Reference { get; set; }

    public string Name { get; set; }
}
```

## Call APIs
```csharp
An example of a function call
var forms = client.getFormList();
```
