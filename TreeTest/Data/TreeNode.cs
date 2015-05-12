using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeTest
{

    public class Tree
    {
        public string treeName { get; set; }
        public long treeIndex { get; set; }
        public long currentIndex { get; set; }
        public TreeType treeType { get; set; }

        public Dictionary<long, TreeNode> treeNodeDictionary { get; set; }

        public GlobalFlags globalFlags;

        public Tree(GlobalFlags globalFlags, TreeType treeType)
        {
            currentIndex = 0;
            treeNodeDictionary = new Dictionary<long, TreeNode>();
            this.globalFlags = globalFlags;
            this.treeType = treeType;
        }

        public TreeNode getNode(long index)
        {
            if(treeNodeDictionary.ContainsKey(index))
            {
                return treeNodeDictionary[index];
            }
            return null;
        }

        public void SelectNode(long index)
        {
            this.currentIndex = index;

            treeNodeDictionary[currentIndex].SelectNode(this);

        }
    }

    public class TreeBranchCondition
    {
        public string flagName { get; set; }
        public string value { get; set; }
        public CompareType flagCompareType { get; set; }

        public TreeBranchCondition(string flagName, string value, CompareType compareType)
        {
            this.flagName = flagName;
            this.value = value;
            this.flagCompareType = compareType;
        }
    }

    public class TreeNodeFlagSet
    {
        public string flagName { get; set; }
        public string value { get; set; }
        public FlagType flagType { get; set; }

    }


    public class TreeBranch
    {
        public string description { get; set; }
        public long linkIndex { get; set; }
        public List<TreeBranchCondition> conditionList { get; set; }

        public TreeBranch()
        {
            this.conditionList = new List<TreeBranchCondition>();
        }
            
        public TreeBranch(string description, long linkIndex, List<TreeBranchCondition> conditionList)
        {
            this.description = description;
            this.linkIndex = linkIndex;
            this.conditionList = conditionList;
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", description, linkIndex);
        }
    }

    public class TreeNodeContent
    {
       
    }

    public class WorldNodeContent : TreeNodeContent
    {
        public long linkIndex { get; set; }
        public string zoneName { get; set; }
    }

    public class ZoneNodeContent : TreeNodeContent
    {
        public ZoneNodeType nodeType { get; set; }
        public string nodeName { get; set; }
        public long linkIndex { get; set; }
    }

    public class DialogNodeContent : TreeNodeContent
    {
        public string speaker { get; set; }
        public string text { get; set; }
        public long linkIndex { get; set; }

    }

    public class QuestNodeContent : TreeNodeContent
    {
        public string content { get; set; }
    }

    public class TreeNode
    {
        public long index { get; set; }
        public string name { get; set; }
        public TreeNodeContent content { get; set; }

        public List<TreeBranch> branchList { get; set; }

        public List<TreeNodeFlagSet> flagSetList { get; set; }

        public TreeNode(long index, string name, TreeNodeContent content, List<TreeBranch> branchList, List<TreeNodeFlagSet> flagSetList)
        {
            this.index = index;
            this.name = name;
            this.content = content;
            this.branchList = branchList;
            this.flagSetList = flagSetList;
        }

        public void SelectNode(Tree t)
        {
            if (flagSetList != null)
            {
                foreach (var flag in flagSetList)
                {
                    t.globalFlags.addFlag(flag.flagName, flag.flagType, flag.value);
                }
            }
        }

        public List<string> getBranchListDisplay(Tree t)
        {
            List<string> strList = new List<string>();
            int count = 1;
            foreach (var tb in branchList)
            {
                var branchInclude = true;
                //check conditions on branch
                if (tb.conditionList != null)
                {
                    foreach (var cond in tb.conditionList)
                    {
                        if (!t.globalFlags.checkFlag(cond.flagName, cond.value, cond.flagCompareType))
                        {
                            branchInclude = false;
                        }
                    }
                }

                if (branchInclude)
                {
                    strList.Add(string.Format("-->{0}. {1}", count, tb.ToString()));
                    count++;
                }
            }

            return strList;
        }

        //given the index of the selected index, return the new branch index
        public long getBranchIndex(int selected)
        {
            selected--;
            if(selected > -1 && selected < branchList.Count)
            {
                return branchList[selected].linkIndex;
            }
            return -1;
        }

        public override string ToString()
        {
            string retval = string.Format("{0}.{1}: {2}\n", index, name, content);
           

            return retval;

        }

    }
}
