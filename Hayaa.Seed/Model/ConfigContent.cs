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
            private EnumDatabaseType _databaseType = EnumDatabaseType.SqlServer;
            /// <summary>
            /// 数据库类型
            /// </summary>
            [DataMember]
            [XmlAttribute]
            public EnumDatabaseType databaseType { set { _databaseType = value; } get { return _databaseType; } }
            private bool _multipleBase = false;
            /// <summary>
            /// 是否分库
            /// </summary>
            [DataMember]
            [XmlAttribute]
            public bool multipleBase { set { _multipleBase = value; } get { return _multipleBase; } }
            private EnumMultipleType _multipleType = EnumMultipleType.MultipleByMode;
            /// <summary>
            /// 分库模式
            /// </summary>
            [DataMember]
            [XmlAttribute]
            public EnumMultipleType multipleType { set { _multipleType = value; } get { return _multipleType; } }
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
