using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace NuGetGallery.Dashboard.Model
{
    public class ElmahError
    {
        public string Application { get; set; }
        public string Host { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string Detail { get; set; }
        public DateTime Time { get; set; }
        public ushort StatusCode { get; set; }

        public IDictionary<string, string> ServerVariables { get; private set; }

        public ElmahError() {
            ServerVariables = new Dictionary<string, string>();
        }

        public ElmahError(XmlReader serializedError) : this()
        {
            XmlSerializer ser = new XmlSerializer(typeof(Error));
            Error err = (Error)ser.Deserialize(serializedError);

            Application = err.Application;
            Host = err.Host;
            Type = err.Type;
            Message = err.Message;
            Source = err.Source;
            Detail = err.Detail;
            Time = err.Time;
            StatusCode = err.StatusCode;

            foreach (var serVar in err.ServerVariables)
            {
                ServerVariables[serVar.Name] = serVar.Value.Value;
            }
        }

        #region XmlSerializer Types
        /// <remarks/>
        [XmlType(AnonymousType = true)]
        [XmlRoot(Namespace = "", IsNullable = false)]
        private partial class Error
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
            [XmlElement("serverVariables")]
            [XmlArrayItem("item", IsNullable = false)]
            public ServerVariable[] ServerVariables
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
            [XmlAttribute("application")]
            public string Application
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
            [XmlAttribute("host")]
            public string Host
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
            [XmlAttribute("type")]
            public string Type
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
            [XmlAttribute("message")]
            public string Message
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
            [XmlAttribute("source")]
            public string Source
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
            [XmlAttributeAttribute("detail")]
            public string Detail
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
            [XmlAttribute("time")]
            public DateTime Time
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
            [XmlAttribute("statusCode")]
            public ushort StatusCode
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
        [XmlType(AnonymousType = true)]
        private partial class ServerVariable
        {
            private ServerVariableValue valueField;
            private string nameField;

            /// <remarks/>
            [XmlElement("value")]
            public ServerVariableValue Value
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
            [XmlAttribute("name")]
            public string Name
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
        [XmlType(AnonymousType = true)]
        private partial class ServerVariableValue
        {
            private string stringField;

            /// <remarks/>
            [XmlAttributeAttribute("string")]
            public string Value
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
        #endregion
    }
}