using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Collections.Concurrent;
using Hayaa.Seed.Model;
using Hayaa.Seed.Util;

namespace Hayaa.Seed.Component
{
    public class ProgramPlatformServiceFactory
    {
        private Dictionary<int, Dictionary<string, ServiceWorker>> _serviceData;
        private ConcurrentDictionary<string, Assembly> _assembliesData;
        private static ProgramPlatformServiceFactory _instance = new ProgramPlatformServiceFactory();

        public static ProgramPlatformServiceFactory Instance
        {
            get { return _instance; }
        }
        private ProgramPlatformServiceFactory()
        {
            _assembliesData = new ConcurrentDictionary<string, Assembly>();
            _serviceData = new Dictionary<int, Dictionary<string, ServiceWorker>>();
            InitData(ProgramDistributedConfig.Instance.GetServiceWorkers());
            
        }
        private void InitData(List<ServiceWorker> appServiceConfigs)
        {
             if (appServiceConfigs != null)
            {
                var appUserIDs = appServiceConfigs.Select(a => a.AppUserID).Distinct().ToList();
                if (appUserIDs != null)
                {
                    appUserIDs.ForEach(a =>
                    {
                        if (!_serviceData.ContainsKey(a))
                        {
                            _serviceData.Add(a, null);
                            _serviceData[a] = appServiceConfigs.FindAll(b => b.AppUserID == a).ToDictionary(c => c.ComponentInterface, c => c);
                        }
                    });
                }
            }
        }
        public void Clear()
        {
            _serviceData.Clear();
            _assembliesData.Clear();
            ServiceFactory.Instance.Clear();
        }
        public T CreateWcfService<T>(int appUserID, IEndpointBehavior endpointBehavior = null)
        {
            T tobj = default(T);
            tobj = ServiceFactory.Instance.GetService<T>(appUserID);
            if (tobj != null) return tobj;
            var key = typeof(T).FullName;
            if (_serviceData.Count == 0)
            {
                InitData(ProgramDistributedConfig.Instance.GetServiceWorkers());
            }
            if (_serviceData[appUserID].ContainsKey(key))
            {
                var serviceData = _serviceData[appUserID][key];
                if (serviceData.ComponentType == 2)
                    tobj = ServiceFactory.Instance.GetWcfService<T>(appUserID, serviceData.ServiceUrl);
            }
            return tobj;
        }
        public T CreateService<T>(int appUserID)
        {
            T tobj = default(T);
            tobj = ServiceFactory.Instance.GetService<T>(appUserID);
            if (tobj != null) return tobj;
            var key = typeof(T).FullName;
            if (_serviceData.Count == 0)
            {
                InitData(ProgramDistributedConfig.Instance.GetServiceWorkers());
            }
            if (_serviceData[appUserID].ContainsKey(key))
            {
                var serviceData = _serviceData[appUserID][key];
                string classkey = string.Format("{0}_{1}", appUserID, serviceData.ComponentServiceCompeleteName);
                if (serviceData.ComponentType == 2) return ServiceFactory.Instance.GetWcfService<T>(appUserID, serviceData.ServiceUrl);
                if (!_assembliesData.ContainsKey(classkey))
                {
                    if (!string.IsNullOrEmpty(serviceData.ComponentAssemblyFileStorePath))
                    {
                        if (serviceData.ComponentAssemblyFileStorePath.StartsWith(@"~"))
                            serviceData.ComponentAssemblyFileStorePath = System.Web.HttpContext.Current.Server.MapPath(serviceData.ComponentAssemblyFileStorePath);
                        if (!serviceData.ComponentAssemblyFileStorePath.EndsWith(@"\")) serviceData.ComponentAssemblyFileStorePath = serviceData.ComponentAssemblyFileStorePath + "\\";
                        _assembliesData.TryAdd(classkey, Assembly.LoadFrom(serviceData.ComponentAssemblyFileStorePath + serviceData.ComponentAssemblyFileName));
                    }
                }
                if (!string.IsNullOrEmpty(serviceData.ComponentAssemblyFileStorePath))
                    tobj = ServiceFactory.Instance.GetService<T>(appUserID, serviceData.ComponentServiceName, _assembliesData[classkey]);
                else
                    tobj = ServiceFactory.Instance.GetService<T>(appUserID, serviceData.ComponentServiceCompeleteName);
            }
            return tobj;
        }
        /// <summary>
        /// dll程序集测试
        /// </summary>
        /// <param name="appUserID"></param>
        /// <param name="interfaceName"></param>
        /// <returns></returns>
        internal Object CreateServiceForTest(int appUserID, string interfaceName)
        {
            Object tobj = null;
            tobj = ServiceFactory.Instance.GetServiceForTest(appUserID, interfaceName);
            if (tobj != null) return tobj;
            var key = interfaceName;
            if (_serviceData.Count == 0)
            {
                InitData(ProgramDistributedConfig.Instance.GetServiceWorkers());
            }
            if (_serviceData[appUserID].ContainsKey(key))
            {
                var serviceData = _serviceData[appUserID][key];
                string classkey = string.Format("{0}_{1}", appUserID, serviceData.ComponentServiceCompeleteName);
                if (!_assembliesData.ContainsKey(classkey))
                {
                    if (!string.IsNullOrEmpty(serviceData.ComponentAssemblyFileStorePath))
                    {
                        if (serviceData.ComponentAssemblyFileStorePath.StartsWith(@"~"))
                            serviceData.ComponentAssemblyFileStorePath = System.Web.HttpContext.Current.Server.MapPath(serviceData.ComponentAssemblyFileStorePath);
                        if (!serviceData.ComponentAssemblyFileStorePath.EndsWith(@"\")) serviceData.ComponentAssemblyFileStorePath = serviceData.ComponentAssemblyFileStorePath + "\\";
                        _assembliesData.TryAdd(classkey, Assembly.LoadFrom(serviceData.ComponentAssemblyFileStorePath + serviceData.ComponentAssemblyFileName));
                    }
                }
                if (!string.IsNullOrEmpty(serviceData.ComponentAssemblyFileStorePath))
                    tobj = ServiceFactory.Instance.GetServiceForTest(appUserID, serviceData.ComponentServiceName, _assembliesData[classkey], interfaceName);
                else
                    tobj = ServiceFactory.Instance.GetServiceForTest(appUserID, serviceData.ComponentServiceCompeleteName, interfaceName);
            }
            return tobj;
        }
    }
}
