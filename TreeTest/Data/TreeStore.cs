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
     

        private void LoadTreeDictionary(List<Tree> treeList)
        {
            foreach (var tree in treeList)
            {
                treeDictionary.Add(tree.treeIndex, tree);
            }
        }

    }
}
