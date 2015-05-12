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
        public static Tree getTreeFromFile(string path,TreeType treeType,  GlobalFlags gf)
        {
            
            Tree t = new Tree(gf, treeType);

            var treeNodeList = getTreeNodeListFromFile(path,treeType);
            t.treeNodeDictionary = TreeFactory.getTreeNodeDictionaryFromList(treeNodeList);
            t.currentIndex = treeNodeList[0].index;

            return t;

        }
         
        private static List<TreeNode> getTreeNodeListFromFile(string path, TreeType treeType)
        {
            List<TreeNode> treeNodeList = new List<TreeNode>();

            string fileTxt = File.ReadAllText(path);
            var nodeList = ParseHelper.getSplitList(fileTxt, "----");
            foreach(var node in nodeList)
            {
                treeNodeList.Add(getTreeNode(node,treeType));
            }

            return treeNodeList;

        }

        private static TreeNode getTreeNode(string str, TreeType treeType)
        {
            var nodePartList = ParseHelper.getSplitList(str, "--");
            var nodeDataStr = nodePartList[0];
            var linkDataStr = nodePartList[1];

            TreeNode td = getTreeNodeFromDataStr(nodeDataStr, treeType);
            td.branchList = getTreeBranchListFromDataStr(linkDataStr);
            return td;

        }

        private static TreeNode getTreeNodeFromDataStr(string nodeDataStr, TreeType treeType)
        {
            var dataList = ParseHelper.getSplitList(nodeDataStr, Environment.NewLine);
            TreeNode node = new TreeNode(Int64.Parse(dataList[0]), dataList[1], getTreeNodeContentFromStr(dataList[2],treeType), null, null);
            
            if(dataList.Count > 3)
            {
                node.flagSetList = getFlagSetFromDataStr(dataList[3]);
            }
            return node;
        }

        private static TreeNodeContent getTreeNodeContentFromStr(string contentStr, TreeType treeType)
        {
            var contentList = ParseHelper.getSplitList(contentStr,";");
            switch(treeType)
            {
                case TreeType.World:
                    return new WorldNodeContent() {linkIndex=Int64.Parse(contentList[0]), zoneName=contentList[1] };
                case TreeType.Zone:
                    return new ZoneNodeContent() { linkIndex = Int64.Parse(contentList[0]), nodeName = contentList[1], nodeType = getZoneNodeTypeFromStr(contentList[1]) };
                case TreeType.Dialog:
                    return new DialogNodeContent() {linkIndex = Int64.Parse(contentList[0]), speaker=contentList[1],text=contentList[2] };
                case TreeType.Quest:
                    return new QuestNodeContent() {content=contentStr };
                default:
                    return new TreeNodeContent() ;
            }
        }

        private static ZoneNodeType getZoneNodeTypeFromStr(string zoneTypeStr)
        {
            return (from data in Enum.GetValues(typeof(ZoneNodeType)).Cast<ZoneNodeType>().ToList()
                            where data.ToString() == zoneTypeStr
                            select data).FirstOrDefault();
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
