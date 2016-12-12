using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hayaa.Seed.Component;
using Hayaa.Seed.Model;
using Hayaa.Seed.Util;
using Newtonsoft.Json;
using Hayaa.Seed.Config;

namespace Hayaa.Seed
{
    public class ProgramSeed
    {
        private static ProgramSeed _instance = new ProgramSeed();
        private ProgramInstanceEnvironment _ProgramInstanceEnvironment;
        private ProgramSentinel _ProgramSentinel;
        private ProgramSeed() {
            _ProgramInstanceEnvironment = new ProgramInstanceEnvironment();
            _ProgramSentinel = new ProgramSentinel();
        }
        public static ProgramSeed Instance
        {
            get
            {
                return _instance;
            }           
        }
         
        private InstanceEnvironmentInfo Environment;
        public string InitProgram()
        {
            string result = "";
            try
            {
                ///探测部署环境
                //操作系统、网络环境、部署类型、程序集信息
                Environment= _ProgramInstanceEnvironment.ScanEnvironment();
                ///创建侦听服务
                //web创建侦听页，非web创建tcp侦听线程
                result=_ProgramSentinel.InitSentinelService(Environment.IsWeb);
                ///检查本地配置是否支持分布式配置系统
                //支持分布式配置系统则获取配置
                ProgramDistributedConfig.Instance.RunInAppStartInit();
                var appConfig = ProgramDistributedConfig.Instance.GetAppConfig();
                ///发送基础环境信息
                SendbaseInfo(Environment, appConfig.SentinelUrl);
                ///发送配置信息
                SendAppConfigInfo(appConfig, appConfig.AppConfigSentinelUrl);
                //检查是否支持服务工厂,支持服务工厂创建所有服务并将所有服务方法测试一遍
                if (ProgramDistributedConfig.Instance.IsFactory())
                {
                    string msg = "";
                    ProgramDoctorService.Instance.Test(ref msg);
                }
                //TODO发送检测与服务工厂的信息
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            return result;
        }

        private void SendAppConfigInfo(AppConfiguration appConfig, string appConfigSentinelUrl)
        {
            Dictionary<string, string> paramters = new Dictionary<string, string>();
            string info = JsonConvert.SerializeObject(appConfig);
            paramters.Add(DefineTable.AppConfig, info);
            paramters.Add(DefineTable.SentinelSign, SecurityProvider.GetMd5Sign(info));
            var r=HttpRequestHelper.Instance.GetNormalRequestResult(appConfigSentinelUrl, paramters);
            // LoggerPool.Instance.DefaultLogger.Info("SendAppConfigInfo结果:{0}", r);
        }

        private void SendbaseInfo(InstanceEnvironmentInfo environment, string sentinelUrl)
        {
            Dictionary<string, string> paramters = new Dictionary<string, string>();
            string info = JsonConvert.SerializeObject(environment);
            paramters.Add(DefineTable.Eveinfo, info);
            paramters.Add(DefineTable.SentinelSign, SecurityProvider.GetMd5Sign(info));
            var r = HttpRequestHelper.Instance.GetNormalRequestResult(sentinelUrl, paramters);
            // LoggerPool.Instance.DefaultLogger.Info("SendAppConfigInfo结果:{0}", r);
        }
    }
}
