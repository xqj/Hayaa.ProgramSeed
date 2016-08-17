using ApiStore.ApiConfigClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiStore.ApiConfigClient
{
    internal class DoctorService
    {
        private List<Rel_ConfigSolution_AppUser_Component> _serviceData;
        private static DoctorService _instance = new DoctorService();

        public static DoctorService Instance
        {
            get { return _instance; }
        }
        private DoctorService()
        {
            var clientConfig = ConfigClientFactory.Instance.GetConfigClient();
            _serviceData = clientConfig.GetAppUserServiceConfigs();           
        }
        public bool Test(ref string msg)
        {
            if (_serviceData == null)
            {
                msg = "无服务实例配置数据";
                return false;
            }           
            _serviceData.ForEach(a => {
                if (a == null) return;
                if (a.ComponentType == 2) return;
                var obj=PlatformServiceFactory.Instance.CreateServiceForTest(a.AppUserID, a.ComponentInterface);
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

                    }
                }
            });
            msg = "测试通过";
            
            return true;
        }
    }
}
