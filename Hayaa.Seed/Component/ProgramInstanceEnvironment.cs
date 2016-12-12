using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Hayaa.Seed.Model;

namespace Hayaa.Seed.Component
{
    class ProgramInstanceEnvironment
    {
        internal InstanceEnvironmentInfo ScanEnvironment()
        {
            return new InstanceEnvironmentInfo() {
                  SystemIPs=GetSystemIP(),
                   AppPool=GetAppPool()
            };
        }

        private string GetAppPool()
        {
            string appPool = "";
            try
            {
                //由于宿主程序类型不确定，先按照非web程序处理
                appPool = Environment.GetEnvironmentVariable("APP_POOL_ID", EnvironmentVariableTarget.Process);
                //由于vs10与vs13调试版本差异，IISExpress下vs10获取不到应用程序池
                if (string.IsNullOrEmpty(appPool))
                {
                    var request = (System.Web.HttpContext.Current != null) ? System.Web.HttpContext.Current.Request : null;
                    if (request != null)//宿主可能非web
                        appPool = request.ServerVariables["APP_POOL_ID"];
                }
                if (string.IsNullOrEmpty(appPool))//有可能是窗体程序
                {
                    appPool = AppDomain.CurrentDomain.FriendlyName;
                }
            }
            catch (Exception ex)
            {
                 //权限不足或者应用程序池的名字环境变量的名字不是APP_POOL_ID
               // LoggerPool.Instance.DefaultLogger.Error("获取应用程序池异常:{0}", ex.Message);
            }
            return appPool;
        }

        private List<IPAddress> GetSystemIP()
        {
            IPHostEntry hostComputer = Dns.GetHostEntry(Dns.GetHostName());
            var list = hostComputer.AddressList;
            if (list != null)
                return list.ToList();
            else
                return null;
        }
    }
}