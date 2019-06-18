using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;

namespace ConfigSaveByJson
{
    public static class ConfigSaveByJson
    {
        /// <summary>
        /// 文件锁
        /// </summary>
        private static object File_Lock = new object();


        /// <summary>
        /// 写入
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="data">数据类</param>
        /// <param name="path">文件地址</param>
        /// <returns></returns>
        public static int Write<T>(T data,string path) where T:new()
        {
            lock (File_Lock)
            {
                FileStream file;
                try
                {
                    file = new FileStream(path, FileMode.Create);
                }
                catch (IOException)
                {
                    // IO错误，以后写到日志里
                    return -1;
                }
                catch (System.Security.SecurityException)
                {
                    //          CoolApiExtensions.AddLog(MainPlugin.coolQApi, CoolQLogLevel.Error, "rtw_Data.xml文件出现文件权限错误！");
                    //文件权限错误，以后写到日志里
                    return -2;
                }
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                try
                {
                    serializer.WriteObject(file, data);
                }
                catch (InvalidOperationException)
                {
                    //         CoolApiExtensions.AddLog(MainPlugin.coolQApi, CoolQLogLevel.Error, "rtw_Data.xml文件出现写入失败！");
                    //写入失败
                    return -3;
                }
                finally
                {
                    file.Close();
                }
                return 0;
            }
        }


        /// <summary>
        /// 读取特定路径的文件。如果有任何错误都是返回全新的类
        ///  
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static T Read<T>(string path)where T: new()
        {
            lock (File_Lock)
            {
                T t = new T();
                FileStream file;
                try
                {
                    using (file = new FileStream(path, FileMode.Open))
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                        t = (T)serializer.ReadObject(file);
                    }

                }
                catch (System.Security.SecurityException)
                {
                    //文件权限错误，以后写到日志里
                    return new T();
                }
                catch (FileNotFoundException)
                {
                    return new T();
                }
                catch (IOException)
                {
                    // IO错误，代表所有的file异常，以后写到日志里
                    return new T();
                }
                catch (InvalidOperationException)
                {
                    //读取失败
                    return new T();
                }
                return t;
            }
        }


    }
}
