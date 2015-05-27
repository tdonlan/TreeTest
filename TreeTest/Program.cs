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
            if(args.Length > 0)
            {
                switch (args[0].ToUpper())
                {
                    case "-HELP":
                    case "-?":
                        help();
                        break;
                    case "-R":
                        if (args.Length > 1) { run(args[1]); }

                        else { Console.WriteLine("Missing arguments <manifest>."); }
                        break;
                    case "-C":
                        if (args.Length > 2) { convert(args[1], args[2]); }
                        else { Console.WriteLine("Missing arguments <inputdirectory> <outputdirectory>."); }

                        break;
                    case "-CF":
                        if (args.Length > 3) { convertFile(args[1], args[2], args[3]); }
                        else { Console.WriteLine("Missing arguments <type> <inputfile> <outputfile>."); }
                        break;
                    default:
                        Console.WriteLine("Invalid command specified.");
                        break;
                }
            }
        }

        public static void help()
        {
            string outStr = "TreeTest Utility\n";
            outStr += "2015 - tdonlan\n";
            outStr += "Used to run TreeStore files, and convert from simple data files to JSON format.\n";
            outStr += "TreeTest.exe [-help|-?|-r|-c|-cf] <input> <output>\n";
            outStr += "-help: Display this message.\n";
            outStr += "-?: Display this message.\n";
            outStr += "-r: Run.   <manifest file>.\n";
            outStr += "-c: Convert from Simple to JSON format. <input directory> <output directory>.\n ";
            outStr += "-cf: Convert File from Simple to JSON format. <type> <input directory> <output director>.\n";
            Console.Write(outStr);
        }

        

        public static void run(string manifest)
        {
            Console.WriteLine("Running " + manifest);

            GlobalFlags gf = new GlobalFlags();
            TreeStore ts = TreeStoreLoader.loadTreeStoreFromManifest(manifest);

            TreeStoreRunner tr = new TreeStoreRunner(ts);
         
        }

        public static void convert(string inputDirectory, string outputDirectory)
        {
            Console.WriteLine("Converting from " + inputDirectory + " to " + outputDirectory);

            string manifest = inputDirectory + "/manifest.json";
            TreeStore ts = SimpleTreeParser.LoadTreeStoreFromManifest(manifest);
            TreeStoreExporter.exportTreeStore(ts, outputDirectory);
        }

        public static void convertFile(string type, string inputFile, string outputFile)
        {
            Console.WriteLine("Converting tree of type " + type + " from " + inputFile + " to " + outputFile);

            GlobalFlags gf = new GlobalFlags();

            TreeType treeType = TreeType.World;

            switch(type.ToUpper())
            {
                case "DIALOG":
                    treeType = TreeType.Dialog;
                    break;
                case "WORLD":
                    treeType = TreeType.World;
                    break;
                case "ZONE":
                    treeType = TreeType.Zone;
                    break;
                case "QUEST":
                    treeType = TreeType.Quest;
                    break;
                default:
                    Console.WriteLine("Invalid tree type specified.");
                    return;

            }

            Tree t = SimpleTreeParser.getTreeFromFile(inputFile, treeType, gf);

            TreeStoreExporter.exportTree(t, outputFile);

        }

    }
}
