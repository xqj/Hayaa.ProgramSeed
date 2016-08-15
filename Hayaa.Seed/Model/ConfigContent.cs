using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Hayaa.Seed.Model
{
    /// <summary>
    /// 配置内容抽象类
    /// </summary>
    [DataContract]
    public abstract class ConfigContent
    {
        [DataMember]
        public AppSettings appSettings;
        [DataMember]
        public ConnectionStrings connectionStrings;

        [DataMember]
        public ServiceModelClass serviceModel;
    }
    [DataContract]
    public class AppSettings
    {
        [DataMember]
        [XmlElement]
        public List<AppSetAddClass> add;
        [DataContract]
        public class AppSetAddClass
        {
            [DataMember]
            [XmlAttribute]
            public string key;
            [DataMember]
            [XmlAttribute]
            public string value;
        }
    }
    [DataContract]
    public class ConnectionStrings
    {
        [DataMember]
        [XmlElement]
        public List<ConAddClass> add;
        [DataContract]
        public class ConAddClass
        {
            [DataMember]
            [XmlAttribute]
            public string name;
            [DataMember]
            [XmlAttribute]
            public string connectionString;
        }
    }

    [DataContract]
    public class ServiceModelClass
    {
        [DataMember]
        [XmlElement]
        public List<BindingClass> binding;
        [DataMember]
        [XmlElement]
        public List<Endpoint> endpoint;
        [DataContract]
        public class BindingClass
        {
            [DataMember]
            [XmlAttribute]
            public string name;
            [DataMember]
            [XmlAttribute]
            public int maxBufferPoolSize;
            [DataMember]
            [XmlAttribute]
            public int maxReceivedMessageSize;
        }
        [DataContract]
        public class Endpoint
        {
            [DataMember]
            [XmlAttribute]
            public string address;
            [DataMember]
            [XmlAttribute]
            public string binding;
            [DataMember]
            [XmlAttribute]
            public string bindingConfiguration;
            [DataMember]
            [XmlAttribute]
            public string contract;
            [DataMember]
            [XmlAttribute]
            public string name;
        }
    }
}
