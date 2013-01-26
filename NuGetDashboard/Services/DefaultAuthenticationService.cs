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
using NuGetDashboard.Model;

namespace NuGetDashboard.Services
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

        public override UserAccount ProcessRecievedToken(string token)
        {
            // Pull out the binary token
            Dictionary<string, string> tokenData;
            using (var reader = new StringReader(token))
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
                string decodedJWT = JWT.JsonWebToken.Decode(decoded, _configuration.TokenSigningCert);
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

        public override SessionToken IssueSessionToken(UserAccount userAccount)
        {
            SessionToken token = new SessionToken(
                userAccount,
                DateTime.UtcNow.AddMinutes(30));
            return token;
        }

        public override SessionToken DecodeSessionToken(string encoded)
        {
            byte[] decrypted = _dataProtection.Unprotect(
                Convert.FromBase64String(encoded),
                "sessionToken");
            SessionToken token;
            using (var strm = new MemoryStream(decrypted))
            using(var rdr = new BinaryReader(strm))
            {
                // Discard the prefix nonce
                rdr.ReadBytes(4);

                // Grab the data
                string userName = rdr.ReadString();
                string firstName = rdr.ReadString();
                string lastName = rdr.ReadString();
                DateTime expiresUtc = DateTime.FromBinary(rdr.ReadInt64());
                token = new SessionToken(new UserAccount(userName, firstName, lastName), expiresUtc);

                // Discard the suffix nonce
                // But read it to make sure it's there
                rdr.ReadBytes(4);
            }

            // Check expiry
            if (DateTime.UtcNow >= token.ExpiresUtc)
            {
                throw new SecurityException("Token expired.");
            }
            return token;
        }

        public override string EncodeSessionToken(SessionToken token, bool renew)
        {
            if (renew)
            {
                token.ExpiresUtc = DateTime.UtcNow.AddMinutes(30);
            }
            byte[] data = new byte[8];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(data);
            }

            using(var strm = new MemoryStream())
            using (var writer = new BinaryWriter(strm))
            {
                writer.Write(data, 0, 4);
                writer.Write(token.User.UserName);
                writer.Write(token.User.FirstName);
                writer.Write(token.User.LastName);
                writer.Write(token.ExpiresUtc.ToBinary());
                writer.Write(data, 4, 4);
                writer.Flush();
                strm.Flush();

                return Convert.ToBase64String(_dataProtection.Protect(strm.ToArray(), "sessionToken"));
            }
        }
    }
}