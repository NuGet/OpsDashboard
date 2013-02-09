using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuGetGallery.Dashboard.Services
{
    public class PingResult
    {
        public string Name { get; private set; }
        public string Detail { get; private set; }
        public string Target { get; private set; }
        public bool Success { get; private set; }

        public PingResult(string name, string detail, string target, bool success)
        {
            Name = name;
            Detail = detail;
            Target = target;
            Success = success;
        }
    }
}
