using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using Newtonsoft.Json;
using NuGetDashboard.Model;
using NuGetDashboard.Services;

namespace NuGetDashboard.Modules
{
    public class MainModule : Nancy.NancyModule
    {
        public MainModule(PackageService packages, JobStatusService jobStatus, ConfigurationService configuration)
        {
            Get["/api/v1/packages"] = _ =>
                // Get a list of packages from blob storage
                packages.GetAll();

            Get["/api/v1/jobs"] = _ =>
                // Get jobs status json
                jobStatus.GetJobStatusJson();

            Post["/"] = _ =>
            {
                // Get the token
                string token = Request.Form["wresult"];
                if (String.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException("No token response!");
                }

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
                    string decodedJWT = JWT.JsonWebToken.Decode(decoded, configuration.TokenSigningCert);
                    tokenData = JsonConvert.DeserializeObject<Dictionary<string, string>>(decodedJWT);
                }

                return View["Default"];
            };

            Get["/(.*)"] = _ =>
                // Just show the app
                View["Default", new DefaultViewModel(configuration)];
        }
    }
}