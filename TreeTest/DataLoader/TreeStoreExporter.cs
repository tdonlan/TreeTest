using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleJson;

namespace TreeTest
{
    public class TreeStoreExporter
    {
        public static void exportTreeStore(TreeStore ts, string path)
        {
            List<string> exportPathList = new List<string>();
             List<TreeManifestItem> treeManifestList = new List<TreeManifestItem>();

            foreach(var key in ts.treeDictionary.Keys)
            {
                var tree = ts.treeDictionary[key];
                string exportPath = getTreeExportPath(key, tree, path);
                
                string treeJSON = JsonConvert.SerializeObject(tree);
                //string treeJSON = SimpleJson.SimpleJson.SerializeObject(tree, new TreeSerializationStrategy());

                File.WriteAllText(exportPath, treeJSON, Encoding.Default);

                treeManifestList.Add(new TreeManifestItem() {treeIndex=key,treeName=tree.treeName,treePath=exportPath,treeType=tree.treeType });
            }

            string manifestPath = path + "/manifest.json";
            File.WriteAllText(manifestPath, JsonConvert.SerializeObject(treeManifestList));
 
        }

        public static void exportTree(ITree t, string path)
        {
            string treeJSON = SimpleJson.SimpleJson.SerializeObject(t);
            File.WriteAllText(path, treeJSON);
        }

        public static string getTreeExportPath(long index, ITree t, string path)
        {
            return path + "/" + t.treeType + index + ".json";
        }
    }
}
