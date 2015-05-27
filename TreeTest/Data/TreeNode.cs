using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeTest
{

    #region Node

    public class TreeNode
    {
        public long index { get; set; }
        public string name { get; set; }
        
        public List<TreeBranch> branchList { get; set; }

        public List<TreeNodeFlagSet> flagSetList { get; set; }

        public TreeNode(long index, string name, List<TreeBranch> branchList, List<TreeNodeFlagSet> flagSetList)
        {
            this.index = index;
            this.name = name;

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
            if (selected > -1 && selected < branchList.Count)
            {
                return branchList[selected].linkIndex;
            }
            return -1;
        }

       

    }

    public class WorldTreeNode : TreeNode
    {
        public WorldNodeContent content { get; set; }

        public WorldTreeNode(long index, string name, List<TreeBranch> branchList, List<TreeNodeFlagSet> flagSetList, WorldNodeContent content)
            : base (index,name,branchList,flagSetList)
        {
            this.content = content;
        }

        public override string ToString()
        {
            string retval = string.Format("{0}.{1}: {2}\n", index, name, content);
            return retval;
        }
    }

    public class ZoneTreeNode: TreeNode
    {
        public ZoneNodeContent content { get; set; }

        public ZoneTreeNode(long index, string name, List<TreeBranch> branchList, List<TreeNodeFlagSet> flagSetList, ZoneNodeContent content)
            : base (index,name,branchList,flagSetList)
        {
            this.content = content;
        }

        public override string ToString()
        {
            string retval = string.Format("{0}.{1}: {2}\n", index, name, content);


            return retval;

        }
    }

    public class DialogTreeNode : TreeNode
    {
        public DialogNodeContent content { get; set; }

        public DialogTreeNode(long index, string name, List<TreeBranch> branchList, List<TreeNodeFlagSet> flagSetList, DialogNodeContent content)
            : base (index,name,branchList,flagSetList)
        {
            this.content = content;
        }

        public override string ToString()
        {
            string retval = string.Format("{0}.{1}: {2}\n", index, name, content);


            return retval;

        }

    }

    public class QuestTreeNode : TreeNode
    {
        public QuestNodeContent content { get; set; }

        public QuestTreeNode(long index, string name, List<TreeBranch> branchList, List<TreeNodeFlagSet> flagSetList, QuestNodeContent content)
            : base (index,name,branchList,flagSetList)
        {
            this.content = content;
        }

        public override string ToString()
        {
            string retval = string.Format("{0}.{1}: {2}\n", index, name, content);


            return retval;

        }

    }

    #endregion


    #region NodeContent

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

#endregion

}
