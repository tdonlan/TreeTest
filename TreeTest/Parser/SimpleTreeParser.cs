using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace TreeTest
{
    public class SimpleTreeParser
    {

        public static TreeStore LoadTreeStoreFromManifest(string manifest)
        {
            TreeStore ts = new TreeStore();

            GlobalFlags gf = new GlobalFlags();
            //load the manifest
            string manifestStr = File.ReadAllText(manifest);
            //var manifestJSON = SimpleJson.SimpleJson.DeserializeObject<List<TreeManifestItem>>(manifestStr);
            var manifestJSON = JsonConvert.DeserializeObject<List<TreeManifestItem>>(manifestStr);

            foreach (var treeItem in manifestJSON)
            {
                string path = ParseHelper.getFullPath(manifest, treeItem.treePath);
                Tree tempTree = SimpleTreeParser.getTreeFromFile(path, treeItem.treeType, gf);
                ts.treeDictionary.Add(treeItem.treeIndex, tempTree);
            }

            return ts;
        }

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

            TreeNode node = null;
            switch(treeType)
            {
                case TreeType.World:
                    node = new WorldTreeNode(Int64.Parse(dataList[0]), dataList[1], null, null, (WorldNodeContent)getTreeNodeContentFromStr(dataList[2], treeType));
                    if (dataList.Count > 3)
                    {
                        node.flagSetList = getFlagSetFromDataStr(dataList[3]);
                    }
                    break; 
                case TreeType.Zone:
                    node = new ZoneTreeNode(Int64.Parse(dataList[0]), dataList[1], null, null, (ZoneNodeContent)getTreeNodeContentFromStr(dataList[2], treeType));
                    if (dataList.Count > 3)
                    {
                        node.flagSetList = getFlagSetFromDataStr(dataList[3]);
                    }
                    break; 
                case TreeType.Dialog:
                    node = new DialogTreeNode(Int64.Parse(dataList[0]), dataList[1], null, null, (DialogNodeContent)getTreeNodeContentFromStr(dataList[2], treeType));
                    if (dataList.Count > 3)
                    {
                        node.flagSetList = getFlagSetFromDataStr(dataList[3]);
                    }
                    break;
                case TreeType.Quest:
                    node = new QuestTreeNode(Int64.Parse(dataList[0]), dataList[1], null, null, (QuestNodeContent)getTreeNodeContentFromStr(dataList[2], treeType));
                    if (dataList.Count > 3)
                    {
                        node.flagSetList = getFlagSetFromDataStr(dataList[3]);
                    }
                    break;
                default: break;
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
