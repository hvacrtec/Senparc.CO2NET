using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.CO2NET.Helpers;
using Senparc.CO2NET.Helpers.Serializers;

namespace Senparc.CO2NET.Tests.Helpers
{
    public partial class SerializerHelperTests
    {
        #region 微信JSON的忽略null值测试
        [TestMethod()]
        public void GetJsonStringTest_Null()
        {
            var obj =
                new
                {
                    X =
                        new RootClass()
                        {
                            A = "Jeffrey",
                            B = 31,
                            C = null,
                            ElementClassA = new ElementClass() { A = "Jeffrey", B = null },
                            ElementClassB = null
                        },
                    Y = new
                    {
                        O = "0",
                        Z = (string)null
                    }
                };

            var obj2 = new RootClass()
            {
                A = "Jeffrey",
                B = 31,
                C = null,
                ElementClassA = new ElementClass() { A = "Jeffrey", B = null },
                ElementClassB = null
            };

            var obj3 = new RootClass()
            {
                A = "Jeffrey",
                B = 31,
                C = null,
                ElementClassA = new ElementClass() { A = "Jeffrey", B = null },
                ElementClassB = null,
                ElementClass2 = new ElementClass2()
                {
                    A = "A"
                }
            };

            DateTime dt1 = DateTime.Now;

            var json = SerializerHelper.GetJsonString(obj, new JsonSetting(true, new List<string>(new[] { "Z", "C" })));
            Console.WriteLine(json);

            var json2 = SerializerHelper.GetJsonString(obj2, new JsonSetting(true, new List<string>(new[] { "B" })));
            Console.WriteLine(json2);

            var json3 = SerializerHelper.GetJsonString(obj3, new JsonSetting(true, null, new List<Type>(new[] { typeof(ElementClass2) })));
            Console.WriteLine(json3);

            Console.WriteLine((DateTime.Now - dt1).TotalMilliseconds);
        }

        public class RootClass : JsonIgnoreNull, IJsonIgnoreNull
        {
            private string _private { get; set; }
            public string A { get; set; }
            public int B { get; set; }
            public int? C { get; set; }
            public ElementClass ElementClassA { get; set; }
            public ElementClass ElementClassB { get; set; }
            public ElementClass2 ElementClass2 { get; set; }

            public RootClass()
            {
                _private = "Private";
            }
        }

        public class ElementClass : JsonIgnoreNull, IJsonIgnoreNull
        {
            public string A { get; set; }
            public string B { get; set; }
            public RootClass RootClass { get; set; }
        }

        public class ElementClass2
        {
            public string A { get; set; }
            public string B { get; set; }
            public RootClass RootClass { get; set; }
        }
        #endregion


        #region ExpandoObject类型转换测试

        [TestMethod()]
        public void GetJsonStringTest_Expando()
        {
            dynamic test = new ExpandoObject();
            test.x = "Senparc.Weixin SDK";
            test.y = DateTime.Now;

            DateTime dt1 = DateTime.Now;

            var json = SerializerHelper.GetJsonString(test);
            Console.WriteLine(json);

            Console.WriteLine((DateTime.Now - dt1).TotalMilliseconds);

        }

        #endregion


        #region 常规序列化、反序列化

        [Serializable]
        public class Data
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        [TestMethod()]
        public void GetJsonStringTest()
        {
            var data = new Data()
            {
                Id = 1,
                Name = "Senparc"
            };
            string json = SerializerHelper.GetJsonString(data);
            Assert.AreEqual("{\"Id\":1,\"Name\":\"Senparc\"}", json);
            Console.WriteLine(json);
        }


        #endregion

        [TestMethod()]
        public void GetObjectTest()
        {
            string json = "{\"Id\":1,\"Name\":\"Senparc\"}";
            Data data = SerializerHelper.GetObject<Data>(json);

            Assert.AreEqual(1, data.Id);
            Assert.AreEqual("Senparc", data.Name);
        }

        #region JsonSetting 测试

        [Serializable]
        public class WeixinData
        {
            public int Id { get; set; }
            public string UserName { get; set; }
            public string Note { get; set; }
            public string Sign { get; set; }
            public Sex Sex { get; set; }
        }


        [TestMethod]
        public void JsonSettingTest()
        {
            var weixinData = new WeixinData()
            {
                Id = 1,
                UserName = "JeffreySu",
                Note = null,
                Sign = null,
                Sex = Sex.男
            };

            //string json = js.GetJsonString(weixinData);
            //Console.WriteLine(json);

            //JsonSetting jsonSetting = new JsonSetting(true);
            //string json2 = js.GetJsonString(weixinData, jsonSetting);
            //Console.WriteLine(json2);

            JsonSetting jsonSetting3 = new JsonSetting(true, new List<string>() { "Note" });
            string json3 = SerializerHelper.GetJsonString(weixinData, jsonSetting3);
            Console.WriteLine(json3);
        }

        #endregion

    }
}
