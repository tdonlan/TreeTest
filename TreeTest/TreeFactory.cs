using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeTest
{
    public class TreeFactory
    {
        public static Tree getTree1(GlobalFlags globalFlags)
        {
            Tree t1 = new Tree(globalFlags);

            t1.treeNodeDictionary.Add(1, new TreeNode(1, "Start", "This is the Starting Node! Everything Starts Here!", getTreeBranchList(new List<string>() {"2:second" }), null));
            t1.treeNodeDictionary.Add(2, new TreeNode(2, "Second", "Second is the best.", getTreeBranchList(new List<string>() { "3:last","1:go back to the start:restart"}),null ));
            t1.treeNodeDictionary.Add(3, new TreeNode(3, "Third", "This is the one with the ....", getTreeBranchList(new List<string>() { "2:go back","1:restart:restart"}), getFlagSetList(new List<string>(){"restart"})));
            t1.currentIndex = 1;

            return t1;
        }

        //<num>:<description>:<condition flag>
        private static List<TreeBranch> getTreeBranchList(List<string> delimStrArray)
        {
            List<TreeBranch> retvalList = new List<TreeBranch>();
            foreach(var str in delimStrArray)
            {
                TreeBranch tempBranch = new TreeBranch();
                string[] delimArray = str.Split(':');
                if(delimArray.Length > 0)
                {
                    tempBranch.linkIndex = long.Parse(delimArray[0]);
                }
                if (delimArray.Length > 1)
                {
                    tempBranch.description = delimArray[1];
                }
                if(delimArray.Length > 2)
                {
                    TreeBranchCondition tempCond = new TreeBranchCondition(delimArray[2],"true",CompareType.Equal);
                    tempBranch.conditionList.Add(tempCond);
                }

                retvalList.Add(tempBranch);

            }

            return retvalList;
        }


        private static List<TreeNodeFlagSet> getFlagSetList(List<string> flagList)
        {
            List<TreeNodeFlagSet> flagSetList = new List<TreeNodeFlagSet>();
            foreach(var str in flagList)
            {
                TreeNodeFlagSet tempFlagSet = new TreeNodeFlagSet();
                tempFlagSet.flagName = str;
                tempFlagSet.value = "true";

                flagSetList.Add(tempFlagSet);   
            }

            return flagSetList;

        }

        //TO DO: only return branches where conditions are met
        private static List<TreeBranch> getTreeBranchList(long[] lArray)
        {
            List<TreeBranch> branchList = new List<TreeBranch>();
            foreach(var l in lArray.ToList())
            {
                branchList.Add(new TreeBranch(l.ToString(),l,null));
            }
            return branchList;
        }

        public static Dictionary<long, TreeNode> getTreeNodeDictionaryFromList(List<TreeNode> treeNodeList)
        {
            Dictionary<long, TreeNode> treeNodeDict = new Dictionary<long,TreeNode>();
            foreach(var node in treeNodeList)
            {
                treeNodeDict.Add(node.index, node);
            }

            return treeNodeDict;
        }
    }
}
