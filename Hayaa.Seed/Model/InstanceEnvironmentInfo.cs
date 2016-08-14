using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Hayaa.Seed.Model
{
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
       
    }
}
