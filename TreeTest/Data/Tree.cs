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

        public virtual Dictionary<long, TreeNode> treeNodeDictionary { get; set; }

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

   

}
