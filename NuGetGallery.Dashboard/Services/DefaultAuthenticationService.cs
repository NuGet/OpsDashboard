using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using Newtonsoft.Json;
using NuGetGallery.Dashboard.ViewModel;

namespace NuGetGallery.Dashboard.Services
{
    public class DefaultAuthenticationService : AuthenticationService
    {
        private static readonly string FirstNameField = "http://sts.msft.net/user/FirstName";
        private static readonly string LastNameField = "http://sts.msft.net/user/LastName";
        private static readonly string UserNameField = "http://sts.msft.net/user/UPN";

        private readonly ConfigurationService _configuration;
        private readonly DataProtectionService _dataProtection;

        public DefaultAuthenticationService(ConfigurationService configuration, DataProtectionService dataProtection)
        {
            _configuration = configuration;
            _dataProtection = dataProtection;
        }

        public override UserAccount Login(string acsResult)
        {
            // Pull out the binary token
            Dictionary<string, string> tokenData;
            using (var reader = new StringReader(acsResult))
            {
                // Grab the node
                var doc = new XPathDocument(reader);
                var nav = doc.CreateNavigator();
                XmlNamespaceManager ns = new XmlNamespaceManager(nav.NameTable);
                ns.AddNamespace("t", "http://schemas.xmlsoap.org/ws/2005/02/trust");
                ns.AddNamespace("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
                var secTokenNode = nav.SelectSingleNode("/t:RequestSecurityTokenResponse/t:RequestedSecurityToken/wsse:BinarySecurityToken", ns);

                // Pull the token out and parse it
                string decoded = WebUtility.UrlDecode(
                    Encoding.UTF8.GetString(
                        Convert.FromBase64String(secTokenNode.InnerXml)));
                string decodedJWT = JWT.JsonWebToken.Decode(decoded, _configuration.AuthenticationCertificate);
                tokenData = JsonConvert.DeserializeObject<Dictionary<string, string>>(decodedJWT);
            }

            if (!tokenData.ContainsKey(UserNameField) || 
                !tokenData.ContainsKey(FirstNameField) || 
                !tokenData.ContainsKey(LastNameField))
            {
                throw new InvalidDataException("Token does not contain expected data");
            }
            return new UserAccount(tokenData[UserNameField], tokenData[FirstNameField], tokenData[LastNameField]);
        }
    }
}