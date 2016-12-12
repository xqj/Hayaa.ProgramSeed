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
    [Serializable]
    public class AppConfiguration : IConfigurationSectionHandler
    {
        public AppConfiguration()
        {
            IsAppSettingWebConfig = true;
            IsConnectionStringWebConfig = true;
            IsDistrbutedConfig = false;
        }
        /// <summary>
        /// 是否远程配置
        /// </summary>
        public bool IsDistrbutedConfig
        { set; get; }
        ///
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
        /// <summary>
        /// 程序配置方案ID
        /// </summary>
        public virtual Guid AppConfigID
        {
            get;
            set;
        }
        /// <summary>
        /// 程序配置方案版本
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
        /// 哨兵服务侦听地址
        /// </summary>
        public string SentinelUrl { set; get; }
        /// <summary>
        /// 本地哨兵服务地址
        /// </summary>
        public string LocalSentinelUrl { set; get; }
        /// <summary>
        /// 配置侦听地址
        /// </summary>
        public string AppConfigSentinelUrl { set; get; }
        /// <summary>
        /// 传输密匙
        /// </summary>
        public string TransfersSecurityKey { get; set; }

        public object Create(object parent, object configContext, XmlNode section)
        {
            AppConfiguration configObj = new AppConfiguration();
            configObj.IsDistrbutedConfig = false;
            var baseConfig = section.SelectSingleNode("BaseConfig");
            if (baseConfig != null)
            {
                configObj.IsDistrbutedConfig = Convert.ToBoolean(baseConfig.Attributes["IsDistrbutedConfig"].Value);
                configObj.IsFileLoad = Convert.ToBoolean(baseConfig.Attributes["IsFileLoad"].Value);
                configObj.IsRemote = Convert.ToBoolean(baseConfig.Attributes["IsRemote"].Value);
                configObj.AppConfigID = Guid.Parse(baseConfig.Attributes["SolutionId"].Value);
                configObj.Version = Convert.ToInt32(baseConfig.Attributes["Version"].Value);
                configObj.RemoteConfigServer = baseConfig.Attributes["RemoteConfigServer"].Value;
                configObj.LocalConfigFilePath = baseConfig.Attributes["LocalConfigFilePath"].Value;
                configObj.ConfigFileName = baseConfig.Attributes["ConfigFileName"].Value;
                configObj.AppID = Convert.ToInt32(baseConfig.Attributes["AppID"].Value);
                configObj.IsVirtualPath = Convert.ToBoolean(baseConfig.Attributes["IsVirtualPath"].Value);
                configObj.SentinelUrl = baseConfig.Attributes["SentinelUrl"].Value;
                configObj.LocalSentinelUrl = baseConfig.Attributes["LocalSentinelUrl"].Value;
                configObj.IsAppSettingWebConfig = Convert.ToBoolean(baseConfig.Attributes["IsAppSettingWebConfig"].Value);
                configObj.IsConnectionStringWebConfig = Convert.ToBoolean(baseConfig.Attributes["IsConnectionStringWebConfig"].Value);
                configObj.TransfersSecurityKey = baseConfig.Attributes["TransfersSecurityKey"].Value;
            }
            else
            {
                // LoggerPool.Instance.DefaultLogger.Error("AppConfiguration没有配置");
            }
            return configObj;
        }
    }
}

