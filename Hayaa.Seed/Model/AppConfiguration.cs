using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;

namespace Hayaa.Seed.Model
{
    
    /// <summary>
    /// 程序终节点配置
    /// 对应exe、website对象
    /// 配置加载对象方式和路径的承载对象
    /// 此对象存储在程序对象本地config文件
    /// 不做远程数据配置
    /// </summary>
    public class AppConfiguration : IConfigurationSectionHandler
    {
        public AppConfiguration()
        {
            IsAppSettingWebConfig = true;
            IsConnectionStringWebConfig = true;
        }
        public bool IsAppSettingWebConfig
        { set; get; }
        public bool IsConnectionStringWebConfig
        { set; get; }
        public int AppID { set; get; }
        /// <summary>
        /// 是否文件方式加载配置，不是则以内存方式加载
        /// </summary>
        public virtual bool IsFileLoad
        {
            get;
            set;
        }

        /// <summary>
        /// 配置模式是否远程化
        /// </summary>
        public virtual bool IsRemote
        {
            get;
            set;
        }

        public virtual Guid SolutionId
        {
            get;
            set;
        }
        /// <summary>
        /// 解决方案版本
        /// </summary>
        public virtual int Version
        {
            get;
            set;
        }
        //远程配置服务地址
        public virtual string RemoteConfigServer { set; get; }
        /// <summary>
        /// 本地模式下配置文件存储路径
        /// </summary>
        public virtual string LocalConfigFilePath { set; get; }
        /// <summary>
        /// 配置文件名
        /// </summary>
        public virtual string ConfigFileName { set; get; }
        /// <summary>
        /// 是否相对路径目录
        /// </summary>
        public bool IsVirtualPath { set; get; }
        /// <summary>
        /// 服务侦听地址
        /// </summary>
        public string SentinelUrl { set; get; }

        public object Create(object parent, object configContext, XmlNode section)
        {
            throw new NotImplementedException();
        }
    }
}

