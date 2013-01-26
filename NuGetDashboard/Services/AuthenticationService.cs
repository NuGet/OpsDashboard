using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NuGetDashboard.Model;

namespace NuGetDashboard.Services
{
    public abstract class AuthenticationService
    {
        public abstract UserAccount ProcessRecievedToken(string token);
        public abstract SessionToken IssueSessionToken(UserAccount userAccount);
        public abstract SessionToken DecodeSessionToken(string encoded);
        public abstract string EncodeSessionToken(SessionToken token, bool renew);
    }
}