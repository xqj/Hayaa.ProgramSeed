using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Collections.Concurrent;

namespace Hayaa.Seed.Util
{
    internal class ServiceFactory
    {
        private static ConcurrentDictionary<string, Object> _servicecontainer;
        private static ServiceFactory _instance = new ServiceFactory();
        public static ServiceFactory Instance
        {
            get
            {
                return _instance;
            }
        }
        private ServiceFactory()
        {
            _servicecontainer = new ConcurrentDictionary<string, object>();
        }
        public void Clear()
        {
            _servicecontainer.Clear();
        }
        /// <summary>
        /// 获取服务，获取不到创建服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="appUserID"></param>
        /// <param name="compentFullClassName"></param>
        /// <returns></returns>
        public T GetService<T>(int appUserID, string compentShortClassName, Assembly assembly)
        {
            Type interfaceType = typeof(T);
            var interfaceName = interfaceType.ToString();
            var serviceKey = string.Format("{0}_{1}", interfaceName, appUserID.ToString());
            if (!_servicecontainer.ContainsKey(serviceKey))
            {
                if (!string.IsNullOrEmpty(compentShortClassName))
                {
                    Type classObject = assembly.GetType(compentShortClassName, true);
                    var serviceClass = Activator.CreateInstance(classObject);
                    _servicecontainer.TryAdd(serviceKey, serviceClass);
                }
            }
            return (T)_servicecontainer[serviceKey];
        }
        internal object GetServiceForTest(int appUserID, string compentShortClassName, Assembly assembly, string interfaceName)
        {
            var serviceKey = string.Format("{0}_{1}", interfaceName, appUserID.ToString());
            if (!_servicecontainer.ContainsKey(serviceKey))
            {
                if (!string.IsNullOrEmpty(compentShortClassName))
                {
                    Type classObject = assembly.GetType(compentShortClassName, true);
                    var serviceClass = Activator.CreateInstance(classObject);
                    _servicecontainer.TryAdd(serviceKey, serviceClass);
                }
            }
            return _servicecontainer[serviceKey];
        }
        public T GetService<T>(int appUserID, string compentFullClassName)
        {
            Type interfaceType = typeof(T);
            var interfaceName = interfaceType.ToString();
            var serviceKey = string.Format("{0}_{1}", interfaceName, appUserID.ToString());
            if (!_servicecontainer.ContainsKey(serviceKey))
            {
                if (!string.IsNullOrEmpty(compentFullClassName))
                {
                    Type classObject = Type.GetType(compentFullClassName, true);
                    var serviceClass = Activator.CreateInstance(classObject);
                    _servicecontainer.TryAdd(serviceKey, serviceClass);
                }
            }
            return (T)_servicecontainer[serviceKey];
        }
        internal object GetServiceForTest(int appUserID, string compentFullClassName, string interfaceName)
        {
              var serviceKey = string.Format("{0}_{1}", interfaceName, appUserID.ToString());
            if (!_servicecontainer.ContainsKey(serviceKey))
            {
                if (!string.IsNullOrEmpty(compentFullClassName))
                {
                    Type classObject = Type.GetType(compentFullClassName, true);
                    var serviceClass = Activator.CreateInstance(classObject);
                    _servicecontainer.TryAdd(serviceKey, serviceClass);
                }
            }
            return _servicecontainer[serviceKey];
        }
        /// <summary>
        /// 只获取不创建服务实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="appUserID"></param>
        /// <returns></returns>
        public T GetService<T>(int appUserID)
        {
            Type interfaceType = typeof(T);
            var interfaceName = interfaceType.ToString();
            var serviceKey = string.Format("{0}_{1}", interfaceName, appUserID.ToString());
            T service = default(T);
            if (_servicecontainer.ContainsKey(serviceKey))
            {
                service = (T)_servicecontainer[serviceKey];
            }
            return service;
        }
        internal object GetServiceForTest(int appUserID,string interfaceName)
        {
            var serviceKey = string.Format("{0}_{1}", interfaceName, appUserID.ToString());
            object service =null;
            if (_servicecontainer.ContainsKey(serviceKey))
            {
                service = _servicecontainer[serviceKey];
            }
            return service;
        }
        /// <summary>
        /// 获取服务，获取不到创建服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="compentFullServiceName"></param>
        /// <returns></returns>
        public T GetService<T>(string compentFullServiceName)
        {
            if (!string.IsNullOrEmpty(compentFullServiceName))
            {
                Type classObject = Type.GetType(compentFullServiceName, true);
                return (T)Activator.CreateInstance(classObject);
            }
            return default(T);
        }

        public T GetWcfService<T>(int appUserID,string serviceUrl,IEndpointBehavior endpointBehavior=null)
        {           
            Type interfaceType = typeof(T);
            var interfaceName = interfaceType.ToString();
            var serviceKey = string.Format("{0}_{1}", interfaceName, appUserID.ToString());
            if (!_servicecontainer.ContainsKey(serviceKey))
            {
                if (!string.IsNullOrEmpty(serviceUrl))
                {
                    BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
                    basicHttpBinding.MaxBufferPoolSize = 6553600;
                    basicHttpBinding.MaxReceivedMessageSize = 6553600;
                    EndpointAddress edpHttp = new EndpointAddress(serviceUrl);
                    ServiceEndpoint sepoint = new ServiceEndpoint(ContractDescription.GetContract(typeof(T)), basicHttpBinding, edpHttp);
                    if (endpointBehavior != null)
                    {
                        sepoint.Behaviors.Clear();
                        sepoint.Behaviors.Add(endpointBehavior);
                    }
                    ChannelFactory<T> factory = new ChannelFactory<T>(sepoint);
                    var serviceObj = factory.CreateChannel(edpHttp);
                    _servicecontainer.TryAdd(serviceKey, serviceObj);
                }
            }
            return (T)_servicecontainer[serviceKey];
        }
    }
}
