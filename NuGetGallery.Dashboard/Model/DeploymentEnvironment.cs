using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace NuGetGallery.Dashboard.Model
{
    public class DeploymentEnvironment
    {
        private string _title;

        [JsonIgnore]
        public string Name { get; set; }

        public string Title
        {
            get { return _title ?? Name; }
            set { _title = value; }
        }

        public string Description { get; set; }
        public Uri Url { get; set; }
        public bool PubliclyVisible { get; set; }
        public Uri OperationsStatusBlob { get; set; }
    }
}
