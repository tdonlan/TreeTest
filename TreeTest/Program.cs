using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeTest
{
    class Program
    {
        static void Main(string[] args)
        {

            GlobalFlags gf = new GlobalFlags();
       

            Tree t = TreeFactory.getTree1(gf);

            JSONHelper.ExportJSON(t, @"C:\GameDev\Games\UnityRPG\TreeTest\TreeTest\TestData\Tree3.txt");

            TreeRunner tr = new TreeRunner(t);
        }
    }
}
