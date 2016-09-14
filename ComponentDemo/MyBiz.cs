using Newtonsoft.Json;
using Rings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ComponentDemo
{
    public class MyBiz : MarshalByRefObject
    {
        [MyLog("业务方法1")]
        public object MyBizMethod1(string parameters)
        {
            LimitLoader limitloader = new LimitLoader();
            limitloader.Load();

            var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(parameters);
            string myinput = dic["myinput"].ToString();

            PrintData pd = new PrintData();
            pd.HeaderField = new List<string>()
            {
                "客户名称",
                "送货地址"
            };
            pd.HeaderValue = new Dictionary<string, string>()
            {
                {"客户名称","上海银行"},
                {"送货地址","西藏南路1号"}
            };

            pd.DetailField = new List<string>()
            {
                "商品名称","数量"
            };
            pd.DetailValue = new List<Dictionary<string, string>>();
            for (int i = 1; i <= 10; i++)
            {
                pd.DetailValue.Add(new Dictionary<string, string>() 
                {
                    {"商品名称","名称"+i},
                    {"数量",i.ToString()}
                });
            }

            PrintManager pm = new PrintManager(PluginContext.Current.Account.ApplicationId);
            int modelid = pm.RegisterPrintModel(pd);

            return new { message = "ok", result = StringHelper.GetString("多语言测试") + ":" + myinput, modelid = modelid };
        }
    }
}
