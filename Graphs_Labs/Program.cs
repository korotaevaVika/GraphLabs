using System;

namespace Graphs_Labs
{
    class Program
    {
        static void TestGraphStructure()
        {
            string pathInput = @"C:\Users\Виктория\Desktop\graph.txt";
            string pathPrint = @"C:\Users\Виктория\Desktop\graphViz.png";
            string pathOutput = @"C:\Users\Виктория\Desktop\out.txt";

            ListOfArcs myGrapg = new ListOfArcs(pathInput, pathOutput, pathPrint);

            #region УдалениеДобавлениеДуг
            myGrapg.Smart_Add(1, 2);
            myGrapg.PrintToGraph();

            myGrapg.Smart_Add(2, 2);
            myGrapg.PrintToGraph();

            myGrapg.Smart_Delete(1);
            myGrapg.PrintToGraph();

            myGrapg.Smart_Delete(0);
            myGrapg.PrintToGraph();

            myGrapg.Smart_Add(1, 2);
            myGrapg.PrintToGraph();

            myGrapg.PrintToFile();
            myGrapg.PrintToGraph();
            #endregion
        }

        static void TestConnectivityСomponentAlg()
        {
            string pathInput = @"C:\Users\Виктория\Desktop\graph.txt";
            string pathPrint = @"C:\Users\Виктория\Desktop\graphViz.png";
            string pathOutput = @"C:\Users\Виктория\Desktop\out.txt";

            ListOfArcs myGrapg = new ListOfArcs(pathInput, pathOutput, pathPrint);

            #region АлгоритмНахКомпонентСвязанности
            ListOfEdges edgesG = new ListOfEdges(myGrapg.QuantityVertex, myGrapg.VertexI, myGrapg.VertexJ);
            edgesG.ConnectivityСomponentAlg();
            #endregion
        }

        static void TestAlgKraskala()
        {
            string pathInput = @"C:\Users\Виктория\Desktop\graphEdges.txt";
            string pathPrint = @"C:\Users\Виктория\Desktop\graphViz.png";
            string pathOutput = @"C:\Users\Виктория\Desktop\out.txt";

            ListOfEdgesWithWeigh edgesWithWeigh = new ListOfEdgesWithWeigh(pathInput, pathOutput, pathPrint);
            edgesWithWeigh.AlgKraskala();
            edgesWithWeigh.PrintToGraph(ResultGraphAfterUsingAlgorithms.GraphAfterAlgKraskala);
        }

        static void TestBreadthFirstSearch()
        {
            string pathInput = @"C:\Users\Виктория\Desktop\graph.txt";
            string pathPrint = @"C:\Users\Виктория\Desktop\graphViz.png";
            string pathOutput = @"C:\Users\Виктория\Desktop\out.txt";

            ListOfArcs myGrapg = new ListOfArcs(pathInput, pathOutput, pathPrint);
            ListOfEdges edgesG = new ListOfEdges(myGrapg.QuantityVertex, myGrapg.VertexI, myGrapg.VertexJ);
            int s = 2;
            edgesG.BreadthFirstSearch(s);
        }

        static void TestBreadthFirstSearchForThreeVertexes()
        {
            string pathInput = @"C:\Users\Виктория\Desktop\graph.txt";
            string pathPrint = @"C:\Users\Виктория\Desktop\graphViz.png";
            string pathOutput = @"C:\Users\Виктория\Desktop\out.txt";

            ListOfArcs myGrapg = new ListOfArcs(pathInput, pathOutput, pathPrint);
            myGrapg.PrintToGraph();
            ListOfEdges edgesG = new ListOfEdges(myGrapg.QuantityVertex, myGrapg.VertexI, myGrapg.VertexJ);

            edgesG.BreadthFirstSearch(3, 4, 7);
        }

        static void TestAlgDeikstraWithBuckets()
        {
            string pathInput = @"C:\Users\Виктория\Desktop\graph.txt";
            string pathPrint = @"C:\Users\Виктория\Desktop\graphViz.png";
            string pathOutput = @"C:\Users\Виктория\Desktop\out.txt";

            ListOfEdges myGrapg = new ListOfEdges(pathInput, pathOutput, pathPrint);
            myGrapg.Print();
            // myGrapg.AlgDeikstra(0);
            myGrapg.AlgDeikstraWithBuckets(0);
        }
        static void Main(string[] args)
        {
            // TestBreadthFirstSearch();
            // TestBreadthFirstSearchForThreeVertexes();
            //TestAlgDeikstraWithBuckets();

            Heap heap = new Heap(new int[5] { 17, 525, 12, 51, 2 }, 5);
            heap.SORT_TREE();

            Console.ReadKey();



        }
    }

}
