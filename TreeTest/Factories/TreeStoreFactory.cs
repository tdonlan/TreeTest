using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeTest
{
    public class TreeStoreFactory
    {

        //create a full TreeStore randomly

        public static TreeStore getTreeStore()
        {
            return null;
        }


        private static Tree getTree(GlobalFlags flags, TreeType type, int index )
        {
            Tree tree = new Tree(flags, type);

            tree.treeIndex = index;
            tree.treeName = type.ToString() + index;
            //tree.treeNodeDictionary = TreeFactory.getTreeNodeDictionaryFromList();

            return tree;
        }
    }
}
