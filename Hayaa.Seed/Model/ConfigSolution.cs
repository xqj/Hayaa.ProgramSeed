using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

using System.Runtime.Serialization;
namespace Hayaa.Seed.Model
{
    //ConfigSolution
    [DataContract]
    public class ConfigSolution
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
        /// 解决方案ID
        /// </summary>	
        [DataMember]
        public Guid SolutionID
        {
            get;
            set;
        }
        /// <summary>
        /// 解决方案名称
        /// </summary>	
        [DataMember]
        public string SolutionName
        {
            get;
            set;
        }
        /// <summary>
        /// 方案配置
        /// </summary>	
        [DataMember]
        public string ConfigContent
        {
            get;
            set;
        }
        /// <summary>
        /// 方案版本号
        /// </summary>	
        [DataMember]
        public int Version
        {
            get;
            set;
        }
        /// <summary>
        /// 是否文件存储
        /// </summary>	
        [DataMember]
        public bool IsFile
        {
            get;
            set;
        }
        /// <summary>
        /// 是否远程获取
        /// </summary>		
        [DataMember]
        public bool IsRemote
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

        [DataMember]
        public List<ComponentConfig> Components { get; set; }
        [DataMember]
        public List<Rel_ConfigSolution_AppUser_Component> AppUserServices { get; set; }

    }
}

