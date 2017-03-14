using System;

namespace Graphs_Labs
{
    class Program
    {
        static void Main(string[] args)
        {
            string pathInput = @"C:\Users\Виктория\Desktop\graph.txt";
            string pathPrint = @"C:\Users\Виктория\Desktop\graphViz.png";
            string pathOutput = @"C:\Users\Виктория\Desktop\out.txt";

            Graph myGrapg = new Graph(pathInput, pathOutput, pathPrint);

            myGrapg.Smart_Delete(1);
            myGrapg.Smart_Delete(0);
            myGrapg.Smart_Add(1, 2);
            myGrapg.Smart_Add(1, 1);
            myGrapg.Smart_Add(2, 2);

            myGrapg.PrintToFile();
            myGrapg.PrintToGraph();
            //myGrapg.OpenFolder(pathPrint);
            Console.ReadKey();
        }
    }

}
