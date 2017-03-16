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

            ListOfArcs myGrapg = new ListOfArcs(pathInput, pathOutput, pathPrint);

            ListOfEdges edgesG = new ListOfEdges(myGrapg.QuantityVertex, myGrapg.VertexI, myGrapg.VertexJ);
            edgesG.ConnectivityСomponentAlg();
            /*
            //BEGIN Test Graph Structure - Список пучков дуг
            myGrapg.Smart_Delete(1);
            myGrapg.Smart_Delete(0);
            myGrapg.Smart_Add(1, 2);
            myGrapg.Smart_Add(1, 1);
            myGrapg.Smart_Add(2, 2);

            myGrapg.PrintToFile();
            myGrapg.PrintToGraph();
            //myGrapg.OpenFolder(pathPrint);
            //END Test Graph Structure - Список пучков дуг 
            */
            Console.ReadKey();
        }
    }

}
