﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hayaa.Seed.Component;
using Hayaa.Seed.Config;
using Hayaa.Seed.Model;
using Hayaa.Seed.Util;

namespace Hayaa.Seed.Model
{
    /// <summary>
    /// 配置工具类
    /// 配置管理继承基础类
    /// </summary>
    /// <typeparam name="T"></typeparam>
   public  class ConfigTool<T> where T: ConfigContent,new()
    {
       private  ComponentConfig _compoentConfig;
       private T _config;
       public ConfigTool(int componentID)
       {
            var appConfig = ProgramDistributedConfig.Instance.GetAppConfig();
            _IsWebConfig = !appConfig.IsDistrbutedConfig;          
           if (!IsWebConfig)
           {
                try {
                    _compoentConfig = ProgramDistributedConfig.Instance.GetComponentConfig(componentID);
                    _config = XmlConfigSerializer.Instance.FromXml<T>(_compoentConfig.Content);
                }catch(Exception ex)
                {
                    _IsConfigException = true;
                    _ConfigExceptionMsg = ex.Message;
                }
                _IsAppsetingWebConfig = _compoentConfig.IsAppsetingWebConfig;
                _IsConnectionStringWebConfig = _compoentConfig.IsConnectionStringWebConfig;
            }
       }
       public  T GetComponentConfig(){
           return _config;
       }
        private string _ConfigExceptionMsg = "";
        public string IsConfigExceptionMsg
        {
            get
            {
                return _ConfigExceptionMsg;
            }
        }
        private bool _IsConfigException = false;
        public bool IsConfigException
        {
            get
            {
                return _IsConfigException;
            }
        }
        private bool _IsWebConfig = true;
       public bool IsWebConfig
       {
           get
           {
                return _IsWebConfig;
           }
       }
        private bool _IsAppsetingWebConfig = true;
        public bool IsAppsetingWebConfig
        {
            get
            {
                return _IsAppsetingWebConfig;
            }
        }
        private bool _IsConnectionStringWebConfig = true;
        public bool IsConnectionStringWebConfig
        {
            get
            {
                return _IsConnectionStringWebConfig;
            }
        }
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public string GetConnection(string name,string defaultVal)
       {
           if(IsConnectionStringWebConfig){
           string val=defaultVal;
               try{
                   val=System.Configuration.ConfigurationManager.ConnectionStrings[name].ConnectionString;
               }catch{}
                return val;
           }
           return GetCon(name,defaultVal);
       }
       private string GetCon(string name, string defaultVal)
       {
           var baseConfig = (ConfigContent)_config;
           var con = baseConfig.connectionStrings.add.Find(c => c.name == name);
           if (con != null) return con.connectionString;
           return defaultVal;
       }
       /// <summary>
       /// 获取AppSetting配置
       /// </summary>
       /// <param name="key"></param>
       /// <param name="defaultVal"></param>
       /// <returns></returns>
       public string GetAppsetting(string key, string defaultVal)
       {
           if (IsAppsetingWebConfig)
           {
               string val = defaultVal;
               try
               {
                   val = System.Configuration.ConfigurationManager.AppSettings[key];
               }
               catch { }
               return val;
           }
           return GetAppset(key, defaultVal);
       }
       private string GetAppset(string key, string defaultVal)
       {
           var baseConfig = (ConfigContent)_config;
           var con = baseConfig.appSettings.add.Find(c => c.key == key);
           if (con != null) return con.value;
           return defaultVal;
       }
    }
}
