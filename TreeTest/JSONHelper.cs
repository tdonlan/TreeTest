using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace TreeTest
{
    public class JSONHelper
    {
        public static void ExportJSON(Tree t, string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(t));
        }

        public static Tree ImportJSON(string path)
        {
            return JsonConvert.DeserializeObject<Tree>(File.ReadAllText(path));
        }
    }
}
