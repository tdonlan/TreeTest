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

            Tree t = SimpleTreeParser.getTreeFromFile(@"..\..\..\TestData\SimpleTree2.txt", gf);

            //Tree t = TreeFactory.getTree1(gf);

            //JSONHelper.ExportJSON(t, @"..\..\..\TestData\Tree3.txt");

            TreeRunner tr = new TreeRunner(t);
        }
    }
}
