using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleJson;

namespace TreeTest
{
    public class TreeSerializationStrategy : PocoJsonSerializerStrategy 
    {
        public override object DeserializeObject(object value, Type type)
        {

            if(type == typeof(TreeNodeContent))
            {
                int i = 0;
            }

            return base.DeserializeObject(value, type);
        }

      
       
    }
}
