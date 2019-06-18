using ConfigSaveByJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        public static TestModel _testmodel;

        static void Main(string[] args)
        {
            ConfigSaveByJson.ConfigSaveByJson.Write(new TestModel(),Path.Combine( Environment.CurrentDirectory,"test.json"));

            _testmodel = ConfigSaveByJson.ConfigSaveByJson.Read<TestModel>(Path.Combine(Environment.CurrentDirectory, "test.json"));

            Console.WriteLine(_testmodel.Name + _testmodel.Url);
            Console.ReadKey();
        }
    }
    /// <summary>
    /// 测试用的类
    /// </summary>
    [DataContract]
    public class TestModel
    {
        [DataMember]
        public string Name { get; set; } = "Kong";

        [DataMember]
        public string Url { get; set; } = "jiangsimpler.github.com";
    }
}
