using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hayaa.Seed.Component;
using Hayaa.Seed.Model;

namespace Hayaa.Seed.Component
{
    internal class ProgramDoctorService
    {
        private List<ServiceWorker> _serviceData;
        private static ProgramDoctorService _instance = new ProgramDoctorService();

        public static ProgramDoctorService Instance
        {
            get { return _instance; }
        }
        private ProgramDoctorService()
        {
            _serviceData = ProgramDistributedConfig.Instance.GetServiceWorkers();
        }
        public bool Test(ref string msg)
        {
            if (_serviceData == null)
            {
                msg = "无服务实例配置数据";
                return false;
            }
            string testMsg = "";
            _serviceData.ForEach(a => {
                if (a == null) return;
                if (a.ComponentType == 2) return;
                var obj=ProgramPlatformServiceFactory.Instance.CreateServiceForTest(a.AppUserID, a.ComponentInterface);
                Type t = Type.GetType(a.ComponentInterface);
                var methods=t.GetMethods();
                for (var i = 0; i < methods.Length; i++)
                {
                    try
                    {
                        methods[i].Invoke(obj, null);
                    }
                    catch (Exception ex)
                    {
                        testMsg = testMsg +"方法:"+ methods[i].Name+"未通过测试;";
                    }
                }
            });
            if (!string.IsNullOrEmpty(testMsg))
            {
                msg = testMsg;
            }else
            msg = "测试通过";
            
            return true;
        }
    }
}
