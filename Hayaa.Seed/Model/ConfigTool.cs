using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hayaa.Seed.Component;
using Hayaa.Seed.Model;

namespace Hayaa.Seed.Model
{
    /// <summary>
    /// 配置工具类
    /// 配置管理继承基础类
    /// </summary>
    /// <typeparam name="T"></typeparam>
   public  class ConfigTool<T> where T: ConfigContent,new()
    {
       private ProgramDistributedConfig _configService;
       private  ComponentConfig _compoentConfig;
       private T _config;
       public ConfigTool(int componentID)
       {
           _configService = ConfigClientFactory.Instance.GetConfigClient();
           if (!IsWebConfig)
           {
               _compoentConfig = _configService.GetComponentConfig(componentID);//不进行null检查保证配置初始化出现问题时爆出异常
               _config = XmlConfigSerializer.Instance.FromXml<T>(_compoentConfig.Content);//不进行null检查保证配置初始化出现问题时爆出异常
           }
       }
       public  T GetComponentConfig(){
           return _config;
       }
       public bool IsWebConfig
       {
           get
           {
               bool isWebConfig = true;
               try{
                   isWebConfig=bool.Parse(System.Configuration.ConfigurationManager.AppSettings[DefineTable.ConfigType]);
               }
               catch(Exception ex){
                   isWebConfig = true;
               }
               return isWebConfig;
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
           if(IsWebConfig){
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
           if (IsWebConfig)
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
