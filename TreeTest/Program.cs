using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace TreeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\tdonlan\Documents\Personal\Dev\TreeTest\TreeTest\TestData\World1";
            /*
            List<TreeManifestItem> treeManifest = new List<TreeManifestItem>();
            treeManifest.Add(new TreeManifestItem() {treeIndex=0,treeName="World",treePath="World1.txt",treeType=TreeType.World });
            treeManifest.Add(new TreeManifestItem() { treeIndex = 1, treeName = "ZoneTown", treePath = "ZoneTown.txt", treeType = TreeType.Zone });
            treeManifest.Add(new TreeManifestItem() { treeIndex = 2, treeName = "DialogZoe", treePath = "DialogZoe.txt", treeType = TreeType.Dialog });

            string jsonManifest =  JsonConvert.SerializeObject(treeManifest);
            */

            GlobalFlags gf = new GlobalFlags();

            TreeStore ts = new TreeStore(gf, path,"WorldManifest.txt");

            TreeStoreRunner tsRunner = new TreeStoreRunner(ts);
            
        }
    }
}
