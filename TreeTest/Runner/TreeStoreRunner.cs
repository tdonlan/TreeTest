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
                        RunWorldTree(curTree);
                        break;
                    case TreeType.Zone:
                        RunZoneTree(curTree);
                        break;
                    case TreeType.Dialog:
                        RunDialogTree(curTree);
                        break;
                    case TreeType.Quest:
                        RunQuestTree(curTree);
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
 

        private void RunWorldTree(Tree tree)
        {
            var currentNode = tree.getNode(tree.currentIndex);
            Console.WriteLine(currentNode.name.ToString());
            var menuList = currentNode.getBranchListDisplay(tree);

            WorldNodeContent content = (WorldNodeContent)currentNode.content;
          
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

        private void RunDialogTree(Tree tree)
        {
            var currentNode = tree.getNode(tree.currentIndex);
            Console.WriteLine(currentNode.name.ToString());
            var menuList = currentNode.getBranchListDisplay(tree);

            DialogNodeContent content = (DialogNodeContent)currentNode.content;
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

        private void RunZoneTree(Tree tree)
        {
            var currentNode = tree.getNode(tree.currentIndex);
            Console.WriteLine(currentNode.name.ToString());
            var menuList = currentNode.getBranchListDisplay(tree);

            ZoneNodeContent content = (ZoneNodeContent)currentNode.content;
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

        private void RunQuestTree(Tree tree)
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

