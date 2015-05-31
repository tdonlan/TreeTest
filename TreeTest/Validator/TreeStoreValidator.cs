using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeTest
{
    public class TreeStoreValidator
    {
        public static bool validateTreeStore(TreeStore ts)
        {
            bool isValid = true;
             //validate all trees
            foreach(ITree t in ts.treeDictionary.Values)
            {
                Console.WriteLine("Validating " + t.treeType);
                if(!validateTree(t))
                {
                    isValid = false;
                }
            }

            //validate global flags

            return isValid;
        }

        //validate there are no dead end links
        public static bool validateTree(ITree tree)
        {
            return tree.validateTreeLinks();
          
        }

       

       
        //validate that every flag accessed is set somewhere
        public static bool validateGlobalFlags(TreeStore ts)
        {

            return false;
        }
    }
}
