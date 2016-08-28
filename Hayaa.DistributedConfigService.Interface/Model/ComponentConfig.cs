using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Hayaa.DistributedConfigService.Interface.Model
{
    [DataContract]
    public class ComponentConfig
    {

        /// <summary>
        /// ID
        /// </summary>		
        [DataMember]
        public int ID
        {
            get;
            set;
        }
        /// <summary>
        /// 组件配置ID
        /// </summary>	
        [DataMember]
        public int ComponentConfigID
        {
            get;
            set;
        }
        /// <summary>
        /// 组件ID
        /// </summary>		
        [DataMember]
        public int ComponentID
        {
            get;
            set;
        }
        /// <summary>
        /// 组件配置内容
        /// </summary>		
        [DataMember]
        public string Content
        {
            get;
            set;
        }
        /// <summary>
        /// 配置内容版本
        /// </summary>	
        [DataMember]
        public int Version
        {
            get;
            set;
        }
        /// <summary>
        /// CreateBy
        /// </summary>	
        [DataMember]
        public int CreateBy
        {
            get;
            set;
        }
        /// <summary>
        /// CreateTime
        /// </summary>		
        [DataMember]
        public DateTime CreateTime
        {
            get;
            set;
        }
        /// <summary>
        /// IsActive
        /// </summary>	
        [DataMember]
        public bool IsActive
        {
            get;
            set;
        }
        /// <summary>
        /// 配置说明
        /// </summary>
        [DataMember]
        public string ComponentConfigTitle { get; set; }

        private bool _IsAppsetingWebConfig = true;
        [DataMember]
        public bool IsAppsetingWebConfig
        {
            get
            {
                return _IsAppsetingWebConfig;
            }
            set
            {
                _IsAppsetingWebConfig = value;
            }
        }
        private bool _IsConnectionStringWebConfig = true;
        [DataMember]
        public bool IsConnectionStringWebConfig
        {
            get
            {
                return _IsConnectionStringWebConfig;
            }
            set
            {
                _IsConnectionStringWebConfig = value;
            }
        }

    }
}
