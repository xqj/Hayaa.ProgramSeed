using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Hayaa.Seed.Model
{
    [Serializable]
    class InstanceEnvironmentInfo
    {
        /// <summary>
        /// 操作系统信息
        /// </summary>
        public string SystemInfo { get {return Environment.OSVersion.ToString(); } }
        /// <summary>
        /// 机器名
        /// </summary>
        public string MachineName { get { return Environment.MachineName; } } 
        /// <summary>
        /// 处理器个数
        /// </summary>
        public int CpuCount { get { return Environment.ProcessorCount; } }
        /// <summary>
        /// 操作系统ip
        /// </summary>
        public List<IPAddress> SystemIPs { set; get; }
        /// <summary>
        /// 应用程序池或者宿主启动程序名字
        /// </summary>
        public string AppPool { set; get; }
        /// <summary>
        /// 部署目录
        /// </summary>
        public string HostBaseDirectory { get {return AppDomain.CurrentDomain.BaseDirectory; } }
        /// <summary>
        /// 系统类型
        /// </summary>
        public bool IsWeb { set; get; }
        /// <summary>
        /// 服务端口
        /// </summary>
        public int ServicePort { set; get; }
    }
}
