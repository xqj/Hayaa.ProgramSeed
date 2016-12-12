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
        private AppSolution _solutionConfig;
        private AppConfiguration _appConfig;
        private static ProgramDistributedConfig _instance = new ProgramDistributedConfig();

        internal static ProgramDistributedConfig Instance
        {
            get
            {
                return _instance;
            }

          
        }

        private ProgramDistributedConfig()
        {
            _solutionConfig = new AppSolution()
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
            try
            {
                _appConfig = ReadAppConfig();//防止基础配置不存在或者错误导致程序无法启动
            }
            catch (Exception ex)
            {
                _appConfig = new AppConfiguration() { IsFileLoad = true, Version = 0, AppConfigID = Guid.Empty, IsRemote = false };//错误配置下给予最小化配置               
            }
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
                var temp = XmlConfigSerializer.Instance.FromXmlFile<AppSolution>(appConfig.LocalConfigFilePath + "/" + appConfig.ConfigFileName);
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
        private AppSolution ReadLocal(AppConfiguration appConfig)
        {
            if (string.IsNullOrEmpty(appConfig.LocalConfigFilePath) && string.IsNullOrEmpty(appConfig.ConfigFileName))//如果没有默认路径和默认文件配置不读取
            {
                return null;//构造函数里默认数值
            }
            try
            {
                var temp = XmlConfigSerializer.Instance.FromXmlFile<AppSolution>(appConfig.LocalConfigFilePath + "/" + appConfig.ConfigFileName);
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
                AppSolution localconfig = null;
                //判断配置文件是否已经存在
                if (File.Exists(appConfig.LocalConfigFilePath + "/" + appConfig.ConfigFileName))
                {
                    localconfig = ReadLocal(appConfig);
                }
                //远程拉取配置文件
                var remoteConfig = GetRemote(appConfig.RemoteConfigServer, appConfig.AppConfigID);
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
        private AppSolution GetRemote(string url, Guid solutionID)
        {
            var dic = new Dictionary<string, string>();
            dic.Add(DefineTable.SolutionIDParam, solutionID.ToString());
            string str = "";
            AppSolution result = null;
            try
            {
               /// var apiStoreUser = SecurityProvider.GetApiStoreUser();
                str = HttpRequestHelper.Instance.GetNormalRequestResult(url + "/" + DefineTable.GetRmoteConfigAction, dic);
                str = HttpUtility.UrlDecode(str);
                result = XmlConfigSerializer.Instance.FromXml<AppSolution>(str);
            }
            catch (Exception ex)
            {
                result = null;
                return result;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
                r = (AppConfiguration)ConfigurationManager.GetSection("AppConfiguration");
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
        public AppSolution GetLocalConfig()
        {
            return ReadLocal(_appConfig);
        }
        public bool IsEmpty()
        {
            if (_solutionConfig.SolutionID.Equals(Guid.Empty))
            {
                return true;
            }
            return false;
        }
        public bool IsFactory()
        {
            return _solutionConfig.IsFactory;
        }
        /// <summary>
        /// 在程序第一次运行时运行此方法获取配置
        /// </summary>
        /// <returns></returns>
        public InitResult RunInAppStartInit()
        {
            var r = new InitResult() { Result = true };
            if (_appConfig.IsRemote)//判断是否读取远程配置模式
            {
                ReadRemote(_appConfig);//读取远程配置
            }
            ReadLocal(_appConfig, r);//读取本地配置 
            return r;
        }
        public ComponentConfig GetComponentConfig(int componetID)
        {
            //构造函数完成无null初始化设置
            return _solutionConfig.Components.Find(c => c.ComponentID == componetID);
        }
        public List<ServiceWorker> GetServiceWorkers()
        {
            return _solutionConfig.Workers;
        }
        public AppConfiguration GetAppConfig()
        {
            return _appConfig;
        }
    }
    public class InitResult
    {
        public bool Result { set; get; }
        public string Message { set; get; }
    }
}

