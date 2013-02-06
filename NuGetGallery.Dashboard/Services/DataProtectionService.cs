using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NuGetGallery.Dashboard.Services
{
    public abstract class DataProtectionService
    {
        public abstract byte[] Protect(byte[] data, string purpose);
        public abstract byte[] Unprotect(byte[] data, string purpose);
    }
}