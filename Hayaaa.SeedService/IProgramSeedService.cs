using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hayaaa.SeedService
{
  public  interface IProgramSeedService
    {
        /// <summary>
        /// 哨兵服务，种子端发送的哨兵信息
        /// </summary>
        /// <param name="info">信息</param>
        /// <param name="sign">md5后的签名</param>
        /// <returns></returns>
        string Sentinel(string info, string sign);
        /// <summary>
        /// 配置哨兵服务，种子端发送的本地配置信息
        /// </summary>
        /// <param name="info">信息</param>
        /// <param name="sign">md5后的签名</param>
        /// <returns></returns>
        string AppConfigSentinel(string info, string sign);
    }
}
