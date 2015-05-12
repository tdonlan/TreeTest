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
            TreeStore ts = loadTreeStoreJSON();
            TreeStore ts2 = loadTreeStoreSimple();
            runTreeStore(ts2);
        }

        private static TreeStore loadTreeStoreSimple()
        {
            string path = @"C:\GameDev\Games\UnityRPG\TreeTest\TreeTest\TestData\World1";
             GlobalFlags gf = new GlobalFlags();
            return new TreeStore(gf, path, "WorldManifest.txt");
        }

        private static TreeStore loadTreeStoreJSON()
        {
            string path = @"C:\GameDev\Games\UnityRPG\TreeTest\TreeTest\TestData\WorldJSON";
            GlobalFlags gf = new GlobalFlags();
            return TreeStoreLoader.loadTreeStoreFromManifest(path + "/manifest.json");
        }

        private static void runTreeStore(TreeStore ts)
        {
            TreeStoreRunner tsRunner = new TreeStoreRunner(ts);
        }

        private static void exportTreeStore(TreeStore ts)
        {
            string exportPath = @"C:\GameDev\Games\UnityRPG\TreeTest\TreeTest\TestData\WorldJSON";
            TreeStoreExporter.exportTreeStore(ts, exportPath);
        }
    }
}
