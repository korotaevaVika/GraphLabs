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

            /*
            #region АлгоритмНахКомпонентСвязанности
            ListOfEdges edgesG = new ListOfEdges(myGrapg.QuantityVertex, myGrapg.VertexI, myGrapg.VertexJ);
            edgesG.ConnectivityСomponentAlg();
            #endregion
            */

            #region УдалениеДобавлениеДуг
            myGrapg.Smart_Add(1, 2);
            myGrapg.PrintToGraph();

            myGrapg.Smart_Add(2, 2);
            myGrapg.PrintToGraph();

            //myGrapg.Smart_Delete(1);
            //myGrapg.PrintToGraph();

            //myGrapg.Smart_Delete(0);
            //myGrapg.PrintToGraph();

            //myGrapg.Smart_Add(1, 2);
            //myGrapg.PrintToGraph();

            myGrapg.PrintToFile();
            myGrapg.PrintToGraph();
            #endregion


            Console.ReadKey();
        }
    }

}
