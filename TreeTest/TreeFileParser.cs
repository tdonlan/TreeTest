using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TreeTest
{
    public class TreeFileParser
    {
        public static Tree getTreeFromFile(string path)
        {
            GlobalFlags gf = new GlobalFlags();
            
            Tree t = new Tree(gf);

            var treeNodeList = getTreeNodeListFromFile(path);
            t.treeNodeDictionary = TreeFactory.getTreeNodeDictionaryFromList(treeNodeList);
            t.currentIndex = treeNodeList[0].index;

            return t;

        }

        public static List<TreeNode> getTreeNodeListFromFile(string path)
        {
            List<TreeNode> treeNodeList = new List<TreeNode>();

            string fileTxt = File.ReadAllText(path);
            string[] nodeArray = fileTxt.Split(new string[] { "----" }, StringSplitOptions.RemoveEmptyEntries);

            foreach(var nodeStr in nodeArray)
            {
                string[] fieldArray = nodeStr.Split('|');
                if(fieldArray.Length > 3)
                {
                    TreeNode tempTreeNode = new TreeNode(int.Parse(fieldArray[0]), fieldArray[1], fieldArray[3], getTreeBranchListFromDelimStr(fieldArray), null);


                    treeNodeList.Add(tempTreeNode);
                }
            }

            return treeNodeList;
        }

        private static List<TreeBranch> getTreeBranchListFromDelimStr(string[] fieldArray)
        {
            List<TreeBranch> branchList = new List<TreeBranch>();
            for(int i =4;i<fieldArray.Length;i++)
            {
                string[] branchFieldArray = fieldArray[i].Split(':');
                if(branchFieldArray.Length > 1)
                {
                    TreeBranch tempBranch = new TreeBranch(branchFieldArray[1], long.Parse(branchFieldArray[0]), null);
                    branchList.Add(tempBranch);
                }

            }
            return branchList;
        }
    }
}
