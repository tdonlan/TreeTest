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
                
                ITree tempTree = loadTreeFromPath(treeItem.treeType, treeItem.treePath);
                treeStore.treeDictionary.Add(treeItem.treeIndex, tempTree);
            }

            return treeStore;
        }

        public static ITree loadTreeFromPath(TreeType treeType, string treePath)
        {
            string treeStr = File.ReadAllText(treePath);

            switch (treeType)
            {
                case TreeType.World:
                    //var treeJSON = SimpleJson.SimpleJson.DeserializeObject<Tree>(treeStr, new TreeSerializationStrategy());
                    return JsonConvert.DeserializeObject<WorldTree>(treeStr);

                case TreeType.Zone:
                    return JsonConvert.DeserializeObject<ZoneTree>(treeStr);

                case TreeType.Dialog:
                    return JsonConvert.DeserializeObject<DialogTree>(treeStr);

                case TreeType.Quest:
                    return JsonConvert.DeserializeObject<QuestTree>(treeStr);

                default: return null;
            }



        }
    }
}
