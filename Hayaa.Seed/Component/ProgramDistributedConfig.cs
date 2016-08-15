using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Hayaa.Seed.Config;
using Hayaa.Seed.Model;
using Hayaa.Seed.Util;

namespace Hayaa.Seed.Component
{
   
    class ProgramDistributedConfig
    {
        private ConfigSolution _solutionConfig;
        private AppConfiguration _appConfig;
        public ProgramDistributedConfig()
        {
            _solutionConfig = new ConfigSolution()
            {
                Components = new List<ComponentConfig>(),
                ConfigContent = "",
                IsRemote = false,
                SolutionID = Guid.Empty,
                IsActive = true,
                IsFile = true,
                SolutionName = "本地配置",
                Version = 0,


            };//构造函数默认对象内属性数值，默认为本地模式参数
        }
        internal bool IsEmpty()
        {
            if (_solutionConfig.SolutionID.Equals(Guid.Empty))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 在程序第一次运行时运行此方法获取配置
        /// </summary>
        /// <returns></returns>
        public InitResult RunInAppStartInit()
        {
            var r = new InitResult() { Result = true };
            AppConfiguration appConfig = null;
            try
            {
                appConfig = ReadAppConfig();//防止基础配置不存在或者错误导致程序无法启动
                _appConfig = appConfig;
            }
            catch (Exception ex)
            {
                appConfig = new AppConfiguration() { IsFileLoad = true, Version = 0, SolutionId = Guid.Empty, IsRemote = false };//错误配置下给予最小化配置
                r.Result = false;
                r.Message = ex.Message;
            }
            if (appConfig.IsRemote)//判断是否读取远程配置模式
            {
                ReadRemote(appConfig);//读取远程配置
            }
            ReadLocal(appConfig, r);//读取本地配置 
            return r;
        }
        public ComponentConfig GetComponentConfig(int componetID)
        {
            //构造函数完成无null初始化设置
            return _solutionConfig.Components.Find(c => c.ComponentID == componetID);
        }
        public List<Rel_ConfigSolution_AppUser_Component> GetAppUserServiceConfigs()
        {
            return _solutionConfig.AppUserServices;
        }
        public AppConfiguration GetAppConfig()
        {
            return _appConfig;
        }
        /// <summary>
        /// 本地配置模式下只有一个方案序列化文件
        /// </summary>
        /// <param name="appConfig"></param>
        private void ReadLocal(AppConfiguration appConfig, InitResult result)
        {
            if (string.IsNullOrEmpty(appConfig.LocalConfigFilePath))//如果没有默认路径不读取
            {
                result.Result = false;
                result.Message = "本地配置文件路径为空";
                return;//构造函数里默认数值
            }
            try
            {
                var temp = XmlConfigSerializer.Instance.FromXmlFile<ConfigSolution>(appConfig.LocalConfigFilePath + "/" + appConfig.ConfigFileName);
                if (temp != null)//使用构造函数里的数值，避免多位置同效代码赋值
                {
                    _solutionConfig = temp;
                }
            }
            catch (Exception ex)//预期异常：格式错误，错误内容
            {
                result.Result = false;
                result.Message = ex.Message;
                return;
            }

        }
        private ConfigSolution ReadLocal(AppConfiguration appConfig)
        {
            if (string.IsNullOrEmpty(appConfig.LocalConfigFilePath) && string.IsNullOrEmpty(appConfig.ConfigFileName))//如果没有默认路径和默认文件配置不读取
            {
                return null;//构造函数里默认数值
            }
            try
            {
                var temp = XmlConfigSerializer.Instance.FromXmlFile<ConfigSolution>(appConfig.LocalConfigFilePath + "/" + appConfig.ConfigFileName);
                return temp;
            }
            catch (Exception ex)//预期异常：格式错误，错误内容
            {

            }
            return null;
        }
        private void ReadRemote(AppConfiguration appConfig)
        {

            if (appConfig.IsFileLoad)
            {
                ConfigSolution localconfig = null;
                //判断配置文件是否已经存在
                if (File.Exists(appConfig.LocalConfigFilePath + "/" + appConfig.ConfigFileName))
                {
                    localconfig = ReadLocal(appConfig);
                }
                //远程拉取配置文件
                var remoteConfig = GetRemote(appConfig.RemoteConfigServer, appConfig.SolutionId);
                //判断配置文件的新鲜程度
                if (remoteConfig != null)//无法获取远程配置时不更新本地
                {
                    if (appConfig.Version == 0)//永远最新
                    {
                        File.Delete(appConfig.LocalConfigFilePath + "/" + appConfig.ConfigFileName);
                        //固化指定目录下制定的文件
                        XmlConfigSerializer.Instance.Serializer(appConfig.LocalConfigFilePath + "/" + appConfig.ConfigFileName, remoteConfig);
                    }
                    if ((appConfig.Version > 0) && (localconfig == null))//本地没有配置文件并且不是永远更新
                    {
                        XmlConfigSerializer.Instance.Serializer(appConfig.LocalConfigFilePath + "/" + appConfig.ConfigFileName, remoteConfig);
                    }
                }
            }
        }
        private ConfigSolution GetRemote(string url, Guid solutionID)
        {
            var dic = new Dictionary<string, string>();
            dic.Add(DefineTable.SolutionIDParam, solutionID.ToString());
            string str = "";
            ConfigSolution result = null;
            try
            {
                var apiStoreUser = SecurityProvider.GetApiStoreUser();
                dic.Add(DefineTable.ApiStore_UserNameParam, apiStoreUser.UserName);
                dic.Add(DefineTable.ApiStore_PasswordParam, apiStoreUser.Password);
                dic.Add(DefineTable.ApiStore_TokenParam, apiStoreUser.Token);
                str = HttpRequestHelper.Instance.GetNormalRequestResult(url + "/" + DefineTable.GetRmoteConfigAction, dic);
                str = HttpUtility.UrlDecode(str);
                result = XmlConfigSerializer.Instance.FromXml<ConfigSolution>(str);
            }
            catch (Exception ex)
            {
                result = null;
                return result;
            }
            return result;
        }
        private AppConfiguration ReadAppConfig()
        {
            AppConfiguration r = new AppConfiguration()
            {
                IsFileLoad = true,
                IsRemote = true,
                IsVirtualPath = true,
                ConfigFileName = "RemoteConfig.Config",
                LocalConfigFilePath = "~/Config",
                Version = 0
            };
            try
            {
                r.IsFileLoad = bool.Parse(ConfigurationManager.AppSettings[DefineTable.IsFileLoad]);
                r.IsRemote = bool.Parse(ConfigurationManager.AppSettings[DefineTable.IsRemote]);
                r.SolutionId = Guid.Parse(ConfigurationManager.AppSettings[DefineTable.SolutionId]);
                r.Version = int.Parse(ConfigurationManager.AppSettings[DefineTable.SolutionVersion]);
                r.RemoteConfigServer = ConfigurationManager.AppSettings[DefineTable.RemoteConfigServerUrl];
                r.LocalConfigFilePath = ConfigurationManager.AppSettings[DefineTable.LocalConfigFilePath];
                r.ConfigFileName = ConfigurationManager.AppSettings[DefineTable.ConfigFileName];
                r.AppID = int.Parse(ConfigurationManager.AppSettings[DefineTable.AppID]);
                r.IsVirtualPath = bool.Parse(ConfigurationManager.AppSettings[DefineTable.IsVirtualPath]);
                r.SentinelUrl = ConfigurationManager.AppSettings[DefineTable.SentinelUrl];

            }
            catch (Exception ex)
            {
                r = new AppConfiguration() { IsVirtualPath = false };
            }
            if (r.IsVirtualPath)
            {
                r.LocalConfigFilePath = System.Web.HttpContext.Current.Server.MapPath(r.LocalConfigFilePath);
            }
            return r;
        }

        internal ConfigSolution GetLocalConfig()
        {
            return ReadLocal(_appConfig);
        }
    }
    public class InitResult
    {
        public bool Result { set; get; }
        public string Message { set; get; }
    }
}

