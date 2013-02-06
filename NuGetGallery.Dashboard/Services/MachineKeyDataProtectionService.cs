using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace NuGetGallery.Dashboard.Services
{
    public class MachineKeyDataProtectionService : DataProtectionService
    {
        public override byte[] Protect(byte[] data, string purpose)
        {
            return MachineKey.Protect(data, purpose);
        }

        public override byte[] Unprotect(byte[] data, string purpose)
        {
            return MachineKey.Unprotect(data, purpose);
        }
    }
}