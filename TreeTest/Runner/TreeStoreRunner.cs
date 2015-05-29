using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeTest
{
    public class TreeStoreRunner
    {
        public TreeStore treeStore { get; set; }
        public long currentTreeIndex { get; set; }
        private bool running { get; set; }

        public TreeStoreRunner(TreeStore ts)
        {
            this.treeStore = ts;
            this.running = true;
            this.currentTreeIndex = 0; //the world should default to tree 0, but maybe look this up?

            RunTreeStore();
        }

        public void RunTreeStore()
        {
            while (running)
            {

                var curTree = treeStore.treeDictionary[currentTreeIndex];
                switch (curTree.treeType)
                {
                    case TreeType.World:
                        RunWorldTree((WorldTree)curTree);
                        break;
                    case TreeType.Zone:
                        RunZoneTree((ZoneTree)curTree);
                        break;
                    case TreeType.Dialog:
                        RunDialogTree((DialogTree)curTree);
                        break;
                    case TreeType.Quest:
                        RunQuestTree((QuestTree)curTree);
                        break;
                    default:
                        running = false;
                        break;
                }
            }
        }

        private void SelectTree(long index)
        {
            if(treeStore.treeDictionary.ContainsKey(index))
            {
                currentTreeIndex = index;
            }
        }
 

        private void RunWorldTree(WorldTree tree)
        {
            var currentNode = (WorldTreeNode)tree.getNode(tree.currentIndex);
            Console.WriteLine(currentNode.name.ToString());
            var menuList = currentNode.getBranchListDisplay(tree);

            WorldNodeContent content = ((WorldTreeNode)currentNode).content;
          
            menuList = addMenuItem(menuList,content.zoneName);

            if (menuList.Count > 0)
            {
                var selected = TreeRunner.displayMenuGetInt(menuList);

                if (selected > currentNode.branchList.Count)
                {
                    SelectTree(content.linkIndex);
                }
                else
                {
                    tree.SelectNode(currentNode.getBranchIndex(selected));
                }
            }
            else
            {
                running = false;
            }
        }

        private void RunDialogTree(DialogTree tree)
        {
            var currentNode = (DialogTreeNode)tree.getNode(tree.currentIndex);
            Console.WriteLine(currentNode.name.ToString());
            var menuList = currentNode.getBranchListDisplay(tree);

            DialogNodeContent content = ((DialogTreeNode)currentNode).content;
            menuList = addMenuItem(menuList, "Leave Conversation");

            if (menuList.Count > 0)
            {
                var selected = TreeRunner.displayMenuGetInt(menuList);

                if (selected > currentNode.branchList.Count)
                {
                    SelectTree(content.linkIndex);
                }
                else
                {
                    tree.SelectNode(currentNode.getBranchIndex(selected));
                }
            }
            else
            {
                running = false;
            }
        }

        private void RunZoneTree(ZoneTree tree)
        {
            var currentNode = (ZoneTreeNode)tree.getNode(tree.currentIndex);
            Console.WriteLine(currentNode.name.ToString());
            var menuList = currentNode.getBranchListDisplay(tree);

            ZoneNodeContent content = ((ZoneTreeNode)currentNode).content;
            menuList = addMenuItem(menuList, content.nodeName);

            if (menuList.Count > 0)
            {
                var selected = TreeRunner.displayMenuGetInt(menuList);

                if (selected > currentNode.branchList.Count)
                {
                    SelectTree(content.linkIndex);
                }
                else
                {
                    tree.SelectNode(currentNode.getBranchIndex(selected));
                }
            }
            else
            {
                running = false;
            }
        }

        private void RunQuestTree(QuestTree tree)
        {

        }

        private List<string> addMenuItem(List<string> menuList, string strItem)
        {
            var index = menuList.Count + 1;
            menuList.Add(string.Format("-->{0}. {1}",index.ToString(),strItem));
            return menuList;
        }
    }
}

