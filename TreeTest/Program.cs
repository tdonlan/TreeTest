using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.IO;

namespace TreeTest
{
    class Program
    {
        static void Main(string[] args)
        {

            //testWriteSimpleJSON();

            //testSimpleJSON();
            //testJSONNet();

            //TreeStore ts = loadTreeStoreJSON();
            TreeStore ts2 = loadTreeStoreSimple();

            //readWriteSimpleJSON(ts2.treeDictionary[0]);

            exportTreeStore(ts2);
            runTreeStore(ts2);
        }

        private static TreeStore loadTreeStoreSimple()
        {
            string path = @"..\..\..\TestData\World1";
             GlobalFlags gf = new GlobalFlags();
            return new TreeStore(gf, path, "WorldManifest.txt");
        }

        private static TreeStore loadTreeStoreJSON()
        {
            string path = @"..\..\..\TestData\WorldJSON";
            GlobalFlags gf = new GlobalFlags();
            return TreeStoreLoader.loadTreeStoreFromManifest(path + "/manifest.json");
        }

        private static void runTreeStore(TreeStore ts)
        {
            TreeStoreRunner tsRunner = new TreeStoreRunner(ts);
        }

        private static void exportTreeStore(TreeStore ts)
        {
            string exportPath = @"..\..\..\TestData\WorldJSON";
            TreeStoreExporter.exportTreeStore(ts, exportPath);
        }


        private static void readWriteSimpleJSON(Tree t)
        {
              string simpleJSON= SimpleJson.SimpleJson.SerializeObject(t);

              string netJSON = JsonConvert.SerializeObject(t);

              Tree t2 = SimpleJson.SimpleJson.DeserializeObject<Tree>(netJSON);

        }

        private static void testWriteSimpleJSON()
        {
            WorldNodeContent nodeContent = new WorldNodeContent();
            nodeContent.linkIndex = 1;
            nodeContent.zoneName = "Zone1";

           string simpleJSON= SimpleJson.SimpleJson.SerializeObject(nodeContent);

           WorldNodeContent nodeContent2 = SimpleJson.SimpleJson.DeserializeObject<WorldNodeContent>(simpleJSON);

           
        }

        private static void testSimpleJSON()
        {
            string path = @"..\..\..\TestData\WorldJSON\Dialog2.json";
            File.ReadAllText(path);

            Tree t = SimpleJson.SimpleJson.DeserializeObject<Tree>(File.ReadAllText(path));
        }

        private static void testJSONNet()
        {
            string path = @"..\..\..\TestData\WorldJSON\Dialog2.json";
            File.ReadAllText(path);

            Tree t = JsonConvert.DeserializeObject<Tree>(File.ReadAllText(path));
            //Tree t = SimpleJson.SimpleJson.DeserializeObject<Tree>(File.ReadAllText(path));
        }
    }
}
