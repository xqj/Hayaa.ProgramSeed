using Hayaa.Seed.Component;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Hayaa.Seed.Util
{
   internal class SecurityProvider
    {
       private static string GetMd5(string source)
       {
           source = string.Format("{0}{1}{2}", DateTime.Now.ToString("yyyyMMdd"), source, DateTime.Now.ToString("HH"));
           MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
           byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(source));
           StringBuilder sBuilder = new StringBuilder();
           for (int i = 0; i < data.Length; i++)
           {
               sBuilder.Append(data[i].ToString("x2"));
           }         
           return sBuilder.ToString();
       }
       private static bool VerifyMd5(string source, string hash)
       {
           string hashOfInput = GetMd5(source);
           StringComparer comparer = StringComparer.OrdinalIgnoreCase;
           if (0 == comparer.Compare(hashOfInput, hash))
           {
               return true;
           }
           else
           {
               return false;
           }
       }
       public static string GetPassword()
       {
           string key = DateTime.Now.ToString("yyyyMMdd");
           try
           {
               key = ProgramDistributedConfig.Instance.GetAppConfig().TransfersSecurityKey;
           }
           catch
           {

           }
           return GetMd5(key);
       }
        public static bool VerifyPassword(string password)
        {
            string key = DateTime.Now.ToString("yyyyMMdd");
            try
            {
                key = ProgramDistributedConfig.Instance.GetAppConfig().TransfersSecurityKey;
            }
            catch
            {

            }
            return VerifyMd5(key, password);
        }
        public static string GetMd5Sign(string info)
        {            
            return GetMd5(info);
        }
        public static bool VerifyMd5Sign(string infoSign)
        {
            string key = DateTime.Now.ToString("yyyyMMdd");
            try
            {
                key = ProgramDistributedConfig.Instance.GetAppConfig().TransfersSecurityKey;
            }
            catch
            {

            }
            return VerifyMd5(key, infoSign);
        }
    }
  
}
