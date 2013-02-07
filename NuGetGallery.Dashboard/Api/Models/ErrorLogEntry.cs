using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuGetGallery.Dashboard.Api.Models
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ErrorLogEntry
    {

        private ServerVariable[] serverVariablesField;

        private string applicationField;

        private string hostField;

        private string typeField;

        private string messageField;

        private string sourceField;

        private string detailField;

        private System.DateTime timeField;

        private ushort statusCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("item", IsNullable = false)]
        public ServerVariable[] serverVariables
        {
            get
            {
                return this.serverVariablesField;
            }
            set
            {
                this.serverVariablesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string application
        {
            get
            {
                return this.applicationField;
            }
            set
            {
                this.applicationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string host
        {
            get
            {
                return this.hostField;
            }
            set
            {
                this.hostField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string detail
        {
            get
            {
                return this.detailField;
            }
            set
            {
                this.detailField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort statusCode
        {
            get
            {
                return this.statusCodeField;
            }
            set
            {
                this.statusCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ServerVariable
    {

        private ServerVariableValue valueField;

        private string nameField;

        /// <remarks/>
        public ServerVariableValue value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ServerVariableValue
    {

        private string stringField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @string
        {
            get
            {
                return this.stringField;
            }
            set
            {
                this.stringField = value;
            }
        }
    }


}
