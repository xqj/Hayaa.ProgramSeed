using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Hayaa.Seed.Model
{
    /// <summary>
    /// DataAccess组件配置项目类
    /// </summary>
    [DataContract]
    public class DataAccessConfiguration
    {
        [DataMember]
        public string ConnectionName { set; get; }
         /// <summary>
        /// 数据库连接字符串
        /// </summary>
        [DataMember]
        public string DatabaseConnection { set; get; }
        /// <summary>
        /// 是否分库
        /// </summary>
        [DataMember]
        public bool Multiple { set; get; }
    }
  
}
