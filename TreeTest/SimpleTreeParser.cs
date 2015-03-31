using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TreeTest
{
    public class SimpleTreeParser
    {
        public static Tree getTreeFromFile(string path, GlobalFlags gf)
        {
            
            Tree t = new Tree(gf);

            var treeNodeList = getTreeNodeListFromFile(path);
            t.treeNodeDictionary = TreeFactory.getTreeNodeDictionaryFromList(treeNodeList);
            t.currentIndex = treeNodeList[0].index;

            return t;

        }
         
        private static List<TreeNode> getTreeNodeListFromFile(string path)
        {
            List<TreeNode> treeNodeList = new List<TreeNode>();

            string fileTxt = File.ReadAllText(path);
            var nodeList = ParseHelper.getSplitList(fileTxt, "----");
            foreach(var node in nodeList)
            {
                treeNodeList.Add(getTreeNode(node));
            }

            return treeNodeList;

        }

        private static TreeNode getTreeNode(string str)
        {
            var nodePartList = ParseHelper.getSplitList(str, "--");
            var nodeDataStr = nodePartList[0];
            var linkDataStr = nodePartList[1];

            TreeNode td = getTreeNodeFromDataStr(nodeDataStr);
            td.branchList = getTreeBranchListFromDataStr(linkDataStr);
            return td;

        }

        private static TreeNode getTreeNodeFromDataStr(string nodeDataStr)
        {
            var dataList = ParseHelper.getSplitList(nodeDataStr, Environment.NewLine);
            TreeNode node = new TreeNode(Int64.Parse(dataList[0]), dataList[1], dataList[2], null, null);
            
            if(dataList.Count > 3)
            {
                node.flagSetList = getFlagSetFromDataStr(dataList[3]);
            }
            return node;
        }

        //Defaulting to a list of bool flags
        private static List<TreeNodeFlagSet> getFlagSetFromDataStr(string flagSetStr)
        {
            List<TreeNodeFlagSet> flagSetList = new List<TreeNodeFlagSet>();

            var flagList = ParseHelper.getSplitListInBlock(flagSetStr,";","{","}");
            foreach(var flag in flagList)
            {
                flagSetList.Add(new TreeNodeFlagSet(){flagName=flag,flagType=FlagType.boolFlag,value="true"});
            }
            return flagSetList;
        }

        private static List<TreeBranch> getTreeBranchListFromDataStr(string linkDataStr)
        {
            List<TreeBranch> branchList = new List<TreeBranch>();

            var linkList = ParseHelper.getSplitList(linkDataStr, Environment.NewLine);
            foreach(var link in linkList)
            {
                TreeBranch tb = new TreeBranch();
                var linkDataList = ParseHelper.getSplitList(link, ":");
                tb.linkIndex = Int64.Parse(linkDataList[0]);
                tb.description = ParseHelper.removeBlock(linkDataList[1],"{","}");

                tb.conditionList = getTreeBranchConditionList(linkDataList[1]);

                branchList.Add(tb);
            }

            return branchList;
        }

        private static List<TreeBranchCondition> getTreeBranchConditionList(string linkStr)
        {
            List<TreeBranchCondition> branchCondList = new List<TreeBranchCondition>();
            var conditionList = ParseHelper.getSplitListInBlock(linkStr,";", "{", "}");
            foreach( var cond in conditionList)
            {
                branchCondList.Add(new TreeBranchCondition(cond, "true", CompareType.Equal));
            }

            return branchCondList;

        }

    }
}
