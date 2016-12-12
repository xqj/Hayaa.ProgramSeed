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
        [XmlElement("add")]
        public List<AppSetAddInfo> Adds;
        [DataContract]
        public class AppSetAddInfo
        {
            [DataMember]
            [XmlAttribute("key")]
            public string Key;
            [DataMember]
            [XmlAttribute("value")]
            public string Value;
        }
    }
    [DataContract]
    public class ConnectionStrings
    {
        [DataMember]
        [XmlElement("add")]
        public List<ConAddClass> Adds;
        [DataContract]
        public class ConAddClass
        {
            [DataMember]
            [XmlAttribute("name")]
            public string name;
            [DataMember]
            [XmlAttribute("connectionString")]
            public string connectionString;
            private EnumDatabaseType _databaseType = EnumDatabaseType.SqlServer;
            /// <summary>
            /// 数据库类型
            /// </summary>
            [DataMember]
            [XmlAttribute("databaseType")]
            public EnumDatabaseType DatabaseType { set { _databaseType = value; } get { return _databaseType; } }
            private bool _multipleBase = false;
            /// <summary>
            /// 是否分库
            /// </summary>
            [DataMember]
            [XmlAttribute("multipleBase")]
            public bool MultipleBase { set { _multipleBase = value; } get { return _multipleBase; } }
            private EnumMultipleType _multipleType = EnumMultipleType.MultipleByMode;
            /// <summary>
            /// 分库模式
            /// </summary>
            [DataMember]
            [XmlAttribute("multipleType")]
            public EnumMultipleType MultipleType { set { _multipleType = value; } get { return _multipleType; } }
        }
    }
    public enum EnumDatabaseType
    {
        SqlServer = 1,
        //Oracle = 2,
        MySql = 4,
    }
    public enum EnumMultipleType
    {
        /// <summary>
        /// 没有分库类型
        /// </summary>
        NoMultiple=1,
        /// <summary>
        /// 以id首位数字分配
        /// </summary>
        MultipleByID = 2,
        /// <summary>
        /// 以模选择分库
        /// </summary>
        MultipleByMode = 4
    }
    [DataContract]
    public class ServiceModelClass
    {
        [DataMember]
        [XmlElement("binding")]
        public List<BindingInfo> Binding;
        [DataMember]
        [XmlElement("endpoint")]
        public List<Endpoint> Endpoints;
        [DataContract]
        public class BindingInfo
        {
            [DataMember]
            [XmlAttribute("name")]
            public string Name;
            [DataMember]
            [XmlAttribute("maxBufferPoolSize")]
            public int MaxBufferPoolSize;
            [DataMember]
            [XmlAttribute("maxReceivedMessageSize")]
            public int MaxReceivedMessageSize;
        }
        [DataContract]
        public class Endpoint
        {
            [DataMember]
            [XmlAttribute("address")]
            public string Address;
            [DataMember]
            [XmlAttribute("binding")]
            public string Binding;
            [DataMember]
            [XmlAttribute("bindingConfiguration")]
            public string BindingConfiguration;
            [DataMember]
            [XmlAttribute("contract")]
            public string Contract;
            [DataMember]
            [XmlAttribute("name")]
            public string Name;
        }
    }
}
