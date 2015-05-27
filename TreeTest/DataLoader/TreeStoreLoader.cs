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
    public class TreeStoreLoader
    {
        public static TreeStore loadTreeStoreFromManifest(string manifestPath)
        {
            string manifestStr = File.ReadAllText(manifestPath);
            var manifestJSON = JsonConvert.DeserializeObject<List<TreeManifestItem>>(manifestStr);

            GlobalFlags globaFlags = new GlobalFlags();

            TreeStore treeStore = new TreeStore();

            foreach (var treeItem in manifestJSON)
            {
                Tree tempTree = loadTreeFromPath(treeItem.treePath);
                treeStore.treeDictionary.Add(treeItem.treeIndex, tempTree);
            }

            return treeStore;
        }

        public static Tree loadTreeFromPath(string treePath)
        {
            string treeStr = File.ReadAllText(treePath);
            //var treeJSON = SimpleJson.SimpleJson.DeserializeObject<Tree>(treeStr, new TreeSerializationStrategy());
            var treeJSON = JsonConvert.DeserializeObject<Tree>(treeStr);

            return treeJSON;

        }
    }
}
