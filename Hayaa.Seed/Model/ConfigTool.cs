using System;
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
        public EnumDatabaseType GetDatabaseType(string name, EnumDatabaseType databaseType)
        {
            if (IsConnectionStringWebConfig)
            {
                return databaseType;
            }
            else
            {
                var baseConfig = (ConfigContent)_config;
                var con = baseConfig.connectionStrings.Adds.Find(c => c.name == name);
                if (con != null)
                {
                    return con.DatabaseType;
                }
                return databaseType;
            }
        }
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public string GetConnection(string name,string defaultVal,long? multipleFiled=null)
       {
           if(IsConnectionStringWebConfig){
           string val=defaultVal;
               try{
                   val=System.Configuration.ConfigurationManager.ConnectionStrings[name].ConnectionString;
               }catch{}
                return val;
           }
           return GetCon(name,defaultVal, multipleFiled.HasValue? multipleFiled.Value:0);
       }
       private string GetCon(string name, string defaultVal, long multipleFiled)
       {
           var baseConfig = (ConfigContent)_config;
           var con = baseConfig.connectionStrings.Adds.Find(c => c.name == name);
            if (con != null) {
               
                return ParseCon(con.connectionString, multipleFiled, con.MultipleType); }
           return defaultVal;
        }  /// <summary>
           /// 分库的链接字符串形式:数据库名_{0}的模板化配置
           /// 而且主键必须是整数和长整数
           /// </summary>
           /// <param name="connectionString"></param>
           /// <returns></returns>
        private static string ParseCon(string connectionString, long multipleFiled, EnumMultipleType multipleType= EnumMultipleType.NoMultiple)
        {
            switch (multipleType)
            {
                case EnumMultipleType.NoMultiple:
                    return connectionString;
                case EnumMultipleType.MultipleByID:
                    if (multipleFiled == 0) return string.Format(connectionString, "1"); ;
                    return string.Format(connectionString, multipleFiled.ToString().Substring(0, 1));
                case EnumMultipleType.MultipleByMode:
                    return string.Format(connectionString, multipleFiled % 10);
            }
            return connectionString;
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
           var con = baseConfig.appSettings.Adds.Find(c => c.Key == key);
           if (con != null) return con.Value;
           return defaultVal;
       }
    }
}
