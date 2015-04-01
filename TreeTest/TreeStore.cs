using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Newtonsoft.Json;

namespace TreeTest
{

    public class TreeManifestItem
    {
        public string treePath { get; set; }
        public TreeType treeType { get; set; }
        public string treeName { get; set; }
        public long treeIndex { get; set; }
    }

    public class TreeStore
    {
        public Dictionary<long, Tree> treeDictionary { get; set; }
        public GlobalFlags globalFlags {get;set;}
        public string treeManifestPath { get; set; }
        
        public TreeStore(GlobalFlags globalFlags, string treeManifestPath)
        {
            this.globalFlags = globalFlags;
            this.treeDictionary = new Dictionary<long, Tree>();
            this.treeManifestPath = treeManifestPath;
        }

        private void LoadTreeStoreFromManifest()
        {
            //load the manifest
            string manifestStr = File.ReadAllText(treeManifestPath);
            var manifestJSON = JsonConvert.DeserializeObject<List<TreeManifestItem>>(manifestStr);

            foreach(var treeItem in manifestJSON)
            {
                Tree tempTree = SimpleTreeParser.getTreeFromFile(treeItem.treePath, treeItem.treeType, this.globalFlags);
                treeDictionary.Add(treeItem.treeIndex, tempTree);
            }
        }
    }
}
