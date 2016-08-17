using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
namespace Hayaa.Seed.Model
{
    //Rel_ConfigSolution_AppUser_Component
    public class ServiceWorker
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
        /// 程序用户ID
        /// </summary>	
        [DataMember]
        public int AppUserID
        {
            get;
            set;
        }
        /// <summary>
        /// 组件ID
        /// </summary>		
        [DataMember]
        public int ComponetID
        {
            get;
            set;
        }
        /// <summary>
        /// 服务接口实现类完全限定名
        /// 形式："类名, 程序集名, Version=1.0.0, Culture=neutral, PublicKeyToken=null"
        /// </summary>	
        [DataMember]
        public string ComponentServiceCompeleteName
        {
            get;
            set;
        }
        /// <summary>
        /// 服务接口实现类名
        ///  形式："类名"
        /// </summary>	
        [DataMember]
        public string ComponentServiceName
        {
            get;
            set;
        }
        /// <summary>
        /// 程序集名称
        /// </summary>	
        [DataMember]
        public string ComponentAssemblyName
        {
            get;
            set;
        }
        /// <summary>
        /// 程序集文件名称
        /// </summary>	
        [DataMember]
        public string ComponentAssemblyFileName
        {
            get;
            set;
        }
        /// <summary>
        /// 程序集文件存储路径
        /// </summary>	
        [DataMember]
        public string ComponentAssemblyFileStorePath
        {
            get;
            set;
        }
        /// <summary>
        /// 接口名称
        /// </summary>
        [DataMember]
        public string ComponentInterface
        {
            get;
            set;
        }
        /// <summary>
        /// 程序集版本
        /// </summary>		
        [DataMember]
        public string AssemblyVersion
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
        /// 组建类型1是dll2是wcf
        /// </summary>
        [DataMember]
        public int ComponentType { get; set; }
        /// <summary>
        /// 服务地址
        /// </summary>
        [DataMember]
        public string ServiceUrl { get; set; }
    }
}

