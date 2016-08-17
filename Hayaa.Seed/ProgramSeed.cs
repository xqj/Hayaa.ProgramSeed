using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hayaa.Seed.Component;
using Hayaa.Seed.Model;

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
                result=_ProgramSentinel.InitSentinelService();
                ///检查本地配置是否支持分布式配置系统
                //支持分布式配置系统则获取配置
                ProgramDistributedConfig.Instance.RunInAppStartInit();          
                //检查是否支持服务工厂,支持服务工厂创建所有服务并将所有服务方法测试一遍
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            return result;
        }

      
    }
}
