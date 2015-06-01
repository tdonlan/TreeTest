using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace TreeTest
{
    public class JSONHelper
    {
        private  const bool USE_SIMPLE_JSON = false;

        public static string export(Object o)
        {
            if (USE_SIMPLE_JSON)
            {
                return SimpleJson.SimpleJson.SerializeObject(o, new EnumSupportedStrategy());
            }
            else
            {
                return JsonConvert.SerializeObject(o);
            }

        }

        public static object import(string json, Type T)
        {

            if(USE_SIMPLE_JSON)
            {
                return SimpleJson.SimpleJson.DeserializeObject(json, T,new EnumSupportedStrategy());
           
            }
            else
            {
                return JsonConvert.DeserializeObject(json, T);
            }
        }


    }
}
