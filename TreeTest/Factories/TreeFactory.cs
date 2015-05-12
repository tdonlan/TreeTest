using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeTest
{
    public class TreeFactory
    {
      

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

        public static IDictionary<long, TreeNode> getSampleTreeNodeDictionary()
        {
            List<string> nodeList = new List<string>() { };
            return null;
        }
    }
}
