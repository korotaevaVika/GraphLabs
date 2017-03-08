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
            myGrapg.Delete(4);
            myGrapg.PrintToGraph();
            Console.ReadKey();
        }
    }

}
