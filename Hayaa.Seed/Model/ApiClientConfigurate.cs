using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Runtime.Serialization;
using System.IO;

namespace HMC.ApiSystem.Client.Config
{


    internal class ApiServerConfigure
    {
        public string ServerUrl { set; get; }

        public string ServerName { set; get; }
    }
    /// <summary>
    ///
    /// </summary>
    internal class ApiClientConfigurate : IConfigurationSectionHandler
    {
        /// <summary>
        /// 无任何外部配置时屏蔽日志组件
        /// </summary>
        public ApiClientConfigurate()
        {
            IsUsingLog = false;
        }

        /// <summary>
        /// 客户端moduleid
        /// </summary>
        public Guid ModuleID { set; get; }
        /// <summary>
        /// 验证系统使用的api用户信息
        /// </summary>
        public string ApiUser { set; get; }
        /// <summary>
        /// 客户端ApiID
        /// </summary>
        public int ApiID { set; get; }
        /// <summary>
        /// 是否启用日志组件
        /// </summary>
        public bool IsUsingLog { set; get; }

        /// <summary>
        /// 日志Msmq的队列名
        /// </summary>
        public string LogMsmqPath { set; get; }

        /// <summary>
        /// 日志需要本地序列化的保存位置
        /// </summary>
        public string LogMsmqLocalBasePath { set; get; }
        /// <summary>
        /// 队列间隔秒数[建议不小于10秒]
        /// </summary>
        public int LogQueueSplit { set; get; }
        /// <summary>
        /// 队列占用线程数量[建议不超过cpu总量的一半]
        /// </summary>
        public int LogQueueThreadTotal { set; get; }
        /// <summary>
        /// 队列待处理堆积数[建议位于100至10000区间]
        /// </summary>
        public int LogQueueTotal { set; get; }
        /// <summary>
        /// 队列长度[依据生产者与消费者差距调整,tps2300下为1800000，tps越高需要的数值越大]
        /// </summary>
        public int LogQueueLength { set; get; }
        /// <summary>
        /// 缓存的主机地址
        /// </summary>
        public string ReadWriteHosts { set; get; }
        /// <summary>
        /// 缓存的从机地址集合
        /// </summary>
        public string ReadOnlyHosts { set; get; }
        /// <summary>
        /// 连接池最大读连接数量
        /// </summary>
        public int MaxReadPoolSize { set; get; }
        /// <summary>
        /// 连接池最大写连接数量
        /// </summary>
        public int MaxWritePoolSize { set; get; }
        /// <summary>
        /// 默认redis库
        /// </summary>
        public int CacheDefaultDb { set; get; }

        /// <summary>
        /// Client模式时服务端配置
        /// </summary>
        public Dictionary<string, ApiServerConfigure> ApiServerNodes { set; get; }

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            ApiClientConfigurate configs = new ApiClientConfigurate();
            var clientCache = section.SelectSingleNode("ClientConfigCache");
            if (clientCache != null)
            {
                configs.MaxReadPoolSize = Convert.ToInt32(clientCache.Attributes["MaxReadPoolSize"].Value);
                configs.MaxWritePoolSize = Convert.ToInt32(clientCache.Attributes["MaxWritePoolSize"].Value);
                configs.CacheDefaultDb = Convert.ToInt32(clientCache.Attributes["DefaultDb"].Value);
                configs.ReadWriteHosts = clientCache.Attributes["ReadWriteHosts"].Value;
                configs.ReadOnlyHosts = clientCache.Attributes["ReadWriteHosts"].Value;
            }
            var clientNode = section.SelectSingleNode("ClientConfig");
            if (clientNode != null)
            {
                configs.ModuleID = Guid.Parse(clientNode.Attributes["ModuleID"].Value);
                configs.ApiID = int.Parse(clientNode.Attributes["ApiID"].Value);
                configs.ApiUser = clientNode.Attributes["ApiUser"].Value;
            }
            var logNode = section.SelectSingleNode("LogConfig");
            if (logNode != null)
            {
                var isUsing = logNode.Attributes["IsUsing"].Value;
                bool blnIsUsing;
                configs.IsUsingLog = string.IsNullOrEmpty(isUsing) ? false : bool.TryParse(isUsing, out blnIsUsing);
                var msmqPath = logNode.Attributes["MsmqPath"].Value;
                configs.LogMsmqPath = string.IsNullOrEmpty(msmqPath) ? DefineTable.LogMsmqPath : msmqPath;
                var msmqLocalBasePath = logNode.Attributes["MsmqLocalBasePath"].Value;
                configs.LogMsmqLocalBasePath = string.IsNullOrEmpty(msmqLocalBasePath) ? DefineTable.LogMsmqLocalBasePath : msmqLocalBasePath;
                try {
                    configs.LogQueueSplit = int.Parse(logNode.Attributes["LogQueueSplit"].Value);
                    configs.LogQueueThreadTotal = int.Parse(logNode.Attributes["LogQueueThreadTotal"].Value);
                    configs.LogQueueLength = int.Parse(logNode.Attributes["LogQueueLength"].Value);
                    configs.LogQueueTotal = int.Parse(logNode.Attributes["LogQueueTotal"].Value); 
                }
                catch
                {
                    configs.LogQueueSplit = 15;
                    configs.LogQueueThreadTotal = 4;
                    configs.LogQueueLength = 1800000;
                    configs.LogQueueTotal = 1000;
                }
            }
            else
            {
                configs.LogQueueSplit = 15;
                configs.LogQueueThreadTotal = 4;
                configs.LogQueueLength = 1800000;
                configs.LogQueueTotal = 1000;                
                configs.IsUsingLog = false;
                configs.LogMsmqPath = DefineTable.LogMsmqPath;
                configs.LogMsmqLocalBasePath = DefineTable.LogMsmqLocalBasePath;
            }
            var apiServerNodes = section.SelectSingleNode("ApiServerNodes");
            configs.ApiServerNodes = new Dictionary<string, ApiServerConfigure>();
            if (apiServerNodes != null)
            {
                var serverNodes = apiServerNodes.ChildNodes;
                if (serverNodes != null)
                {
                    for (var i = 0; i < serverNodes.Count; i++)
                    {
                        var key = serverNodes[i].Attributes["ServerName"].Value;
                        if (configs.ApiServerNodes.ContainsKey(key))
                        {
                            configs.ApiServerNodes[key] = new ApiServerConfigure()
                            {
                                ServerName = key,
                                ServerUrl = serverNodes[i].Attributes["ServerUrl"].Value
                            };
                        }
                        else
                            configs.ApiServerNodes.Add(key, new ApiServerConfigure()
                            {
                                ServerName = key,
                                ServerUrl = serverNodes[i].Attributes["ServerUrl"].Value
                            });
                    }
                }
                if (serverNodes == null)
                    throw new Exception("ApiClientConfigurate配置节的子节点ApiServerNodes不能为空");
            }
            return configs;
        }



    }
    internal class DefineTable
    {
        /// <summary>
        /// 客户端运行模式
        /// </summary>
        public readonly static string ClientRunModel = "client";
        /// <summary>
        /// 是否启用日志组件
        /// </summary>
        public readonly static string IsUsingLog = "true";
        /// <summary>
        /// 日志Msmq的队列名
        /// </summary>
        public readonly static string LogMsmqPath = @".\private$\ApiStore_Api_LogQueue";
        /// <summary>
        /// 日志需要本地序列化的保存位置
        /// </summary>
        public readonly static string LogMsmqLocalBasePath = @"C:\msmqLog\ApiStore\";
    }
}
