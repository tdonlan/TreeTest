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
        public string treeManifestFile { get; set; }
        public string path { get; set; }

        public TreeStore()
        {
            this.globalFlags = new GlobalFlags();
            this.treeDictionary = new Dictionary<long, Tree>();
        }


        public TreeStore(GlobalFlags globalFlags, List<Tree> treeList)
        {
            this.globalFlags = globalFlags;
            this.treeDictionary = new Dictionary<long, Tree>();
            LoadTreeDictionary(treeList);

        }
        
        public TreeStore(GlobalFlags globalFlags,string path, string treeManifestFile)
        {
            this.globalFlags = globalFlags;
            this.treeDictionary = new Dictionary<long, Tree>();
            this.treeManifestFile = treeManifestFile;
            this.path = path;

            LoadTreeStoreFromManifest();
        }

        private void LoadTreeDictionary(List<Tree> treeList)
        {
            foreach (var tree in treeList)
            {
                treeDictionary.Add(tree.treeIndex, tree);
            }
        }

        private string fullPath(string fileName)
        {
            return this.path + "/" + fileName;
        }

        private void LoadTreeStoreFromManifest()
        {
            //load the manifest
            string manifestStr = File.ReadAllText(fullPath(treeManifestFile));
            var manifestJSON = JsonConvert.DeserializeObject<List<TreeManifestItem>>(manifestStr);

            foreach(var treeItem in manifestJSON)
            {
                Tree tempTree = SimpleTreeParser.getTreeFromFile(fullPath(treeItem.treePath), treeItem.treeType, this.globalFlags);
                treeDictionary.Add(treeItem.treeIndex, tempTree);
            }
        }
    }
}
