﻿using System;
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
                ITree tempTree = SimpleTreeParser.getTreeFromFile(path, treeItem.treeType, gf);
                ts.treeDictionary.Add(treeItem.treeIndex, tempTree);
            }

            return ts;
        }

        public static ITree getTreeFromFile(string path,TreeType treeType,  GlobalFlags gf)
        {
            ITree t = null;
            List<ITreeNode> treeNodeList = null;
            switch (treeType)
            {
                case TreeType.World:
                    WorldTree worldTree = new WorldTree(gf, treeType);
                        treeNodeList = getTreeNodeListFromFile(path,treeType);
                       worldTree.treeNodeDictionary = getWorldTreeNodeFromList(treeNodeList);
                       worldTree.currentIndex = treeNodeList[0].index;
                    t = worldTree;
                    break;
                case TreeType.Zone:
                    ZoneTree zoneTree = new ZoneTree(gf, treeType);
                       treeNodeList = getTreeNodeListFromFile(path,treeType);
                       zoneTree.treeNodeDictionary = getZoneTreeNodeFromList(treeNodeList);
                       zoneTree.currentIndex = treeNodeList[0].index;
                       t = zoneTree;
                    break;
                case TreeType.Dialog:
                    DialogTree dialogTree = new DialogTree(gf, treeType);
                        treeNodeList = getTreeNodeListFromFile(path,treeType);
                        dialogTree.treeNodeDictionary = getDialogTreeNodeFromList(treeNodeList);
                        dialogTree.currentIndex = treeNodeList[0].index;
                        t = dialogTree;
                    break;
                case TreeType.Quest:
                    QuestTree questTree = new QuestTree(gf, treeType);
                        treeNodeList = getTreeNodeListFromFile(path,treeType);
                        questTree.treeNodeDictionary = getQuestTreeNodeFromList(treeNodeList);
                        questTree.currentIndex = treeNodeList[0].index;
                        t = questTree;
                    break;
                default:
                    break;
            }
            return t;
        }


        private static List<ITreeNode> getTreeNodeListFromFile(string path, TreeType treeType)
        {
            List<ITreeNode> treeNodeList = new List<ITreeNode>();

            string fileTxt = File.ReadAllText(path);
            var nodeList = ParseHelper.getSplitList(fileTxt, "----");
            foreach(var node in nodeList)
            {
                treeNodeList.Add(getTreeNode(node,treeType));
            }

            return treeNodeList;

        }

        private static ITreeNode getTreeNode(string str, TreeType treeType)
        {
            var nodePartList = ParseHelper.getSplitList(str, "--");
            var nodeDataStr = nodePartList[0];
            var linkDataStr = nodePartList[1];

            ITreeNode td = getTreeNodeFromDataStr(nodeDataStr, treeType);
            td.branchList = getTreeBranchListFromDataStr(linkDataStr);
            return td;

        }

        private static ITreeNode getTreeNodeFromDataStr(string nodeDataStr, TreeType treeType)
        {
            var dataList = ParseHelper.getSplitList(nodeDataStr, Environment.NewLine);

            ITreeNode node = null;
            switch(treeType)
            {
                case TreeType.World:
                    
                    var worldTreeNode =  new WorldTreeNode(Int64.Parse(dataList[0]), dataList[1], null, null, (WorldNodeContent)getTreeNodeContentFromStr(dataList[2], treeType));
                    if (dataList.Count > 3)
                    {
                        worldTreeNode.flagSetList = getFlagSetFromDataStr(dataList[3]);
                    }
                    node = worldTreeNode;
                    break; 
                case TreeType.Zone:
                    var zoneTreeNode = new ZoneTreeNode(Int64.Parse(dataList[0]), dataList[1], null, null, (ZoneNodeContent)getTreeNodeContentFromStr(dataList[2], treeType));
                    if (dataList.Count > 3)
                    {
                        zoneTreeNode.flagSetList = getFlagSetFromDataStr(dataList[3]);
                    }
                    node = zoneTreeNode;
                    break; 
                case TreeType.Dialog:
                    var dialogTreeNode = new DialogTreeNode(Int64.Parse(dataList[0]), dataList[1], null, null, (DialogNodeContent)getTreeNodeContentFromStr(dataList[2], treeType));
                    if (dataList.Count > 3)
                    {
                        dialogTreeNode.flagSetList = getFlagSetFromDataStr(dataList[3]);
                    }
                    node = dialogTreeNode;
                    break;
                case TreeType.Quest:
                    var questTreeNode = new QuestTreeNode(Int64.Parse(dataList[0]), dataList[1], null, null, (QuestNodeContent)getTreeNodeContentFromStr(dataList[2], treeType));
                    if (dataList.Count > 3)
                    {
                        questTreeNode.flagSetList = getFlagSetFromDataStr(dataList[3]);
                    }
                    node = questTreeNode;
                    break;
                default: break;
            }
         
            return node;
        }

        private static ITreeNodeContent getTreeNodeContentFromStr(string contentStr, TreeType treeType)
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
                    return null;
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


        public static Dictionary<long, WorldTreeNode> getWorldTreeNodeFromList(List<ITreeNode> treeNodeList)
        {
            Dictionary<long, WorldTreeNode> treeNodeDict = new Dictionary<long, WorldTreeNode>();
            foreach (var node in treeNodeList)
            {
                WorldTreeNode wNode = (WorldTreeNode)node;
                treeNodeDict.Add(wNode.index, wNode);
            }

            return treeNodeDict;
        }

        public static Dictionary<long, ZoneTreeNode> getZoneTreeNodeFromList(List<ITreeNode> treeNodeList)
        {
            Dictionary<long, ZoneTreeNode> treeNodeDict = new Dictionary<long, ZoneTreeNode>();
            foreach (var node in treeNodeList)
            {
                ZoneTreeNode wNode = (ZoneTreeNode)node;
                treeNodeDict.Add(wNode.index, wNode);
            }

            return treeNodeDict;
        }

        public static Dictionary<long, DialogTreeNode> getDialogTreeNodeFromList(List<ITreeNode> treeNodeList)
        {
            Dictionary<long, DialogTreeNode> treeNodeDict = new Dictionary<long, DialogTreeNode>();
            foreach (var node in treeNodeList)
            {
                DialogTreeNode wNode = (DialogTreeNode)node;
                treeNodeDict.Add(wNode.index, wNode);
            }

            return treeNodeDict;
        }

        public static Dictionary<long, QuestTreeNode> getQuestTreeNodeFromList(List<ITreeNode> treeNodeList)
        {
            Dictionary<long, QuestTreeNode> treeNodeDict = new Dictionary<long, QuestTreeNode>();
            foreach (var node in treeNodeList)
            {
                QuestTreeNode wNode = (QuestTreeNode)node;
                treeNodeDict.Add(wNode.index, wNode);
            }

            return treeNodeDict;
        }

    }
}
