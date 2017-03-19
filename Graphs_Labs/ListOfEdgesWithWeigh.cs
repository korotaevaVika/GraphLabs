using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;
using System;
using System.Drawing.Imaging;
using System.IO;

namespace Graphs_Labs
{
    public class Edge : IComparable
    {
        private int i;
        private int j;
        private int weight;

        public int I { get { return i; } set { i = value; } }
        public int J { get { return j; } set { j = value; } }
        public int C { get { return weight; } set { weight = value; } }

        public int CompareTo(object obj)
        {
            if (obj is Edge)
            {
                return weight.CompareTo((obj as Edge).C);  // compare edges
            }
            throw new ArgumentException("Object is not an Edge");
        }
    }

    public enum ResultGraphAfterUsingAlgorithms
    {
        GraphAfterAlgKraskala,
        InitialGraph
    }

    public class ListOfEdgesWithWeigh
    {
        private string pathInput;
        private string pathOutput;
        private string pathPrint;

        private int n, m;
        private int w; //позиция, куда надо заносить номер очередного выбранного ребра
        private Edge[] edges;
        private int[] K, H, L, X, M;
        //K - Номера включенных в кратчайшее связывающее дерево ребер
        //H[0..n - 1]: H[k] - номер первой вершины, входящей в подмножество.
        //L[0..n - 1]: L[i] – следующая вершина, входящая в то же подмножество, что и вершина.
        //X[0..n - 1]: X[k] – число вершин в k-м подмножестве.
        //M[0..n - 1]: M[k] -  номер подмножества, в которое входит вершина k. 
        //M - как массив предков

        public ListOfEdgesWithWeigh(string pathInput, string pathOutput, string pathPrint)
        {
            this.pathInput = pathInput;
            this.pathOutput = pathOutput;
            this.pathPrint = pathPrint;

            Init(this.pathInput);
        }

        private void Init(string pathInput)
        {
            string[] lines = File.ReadAllLines(pathInput);
            foreach (var item in lines)
            {
                item.Trim();
            }
            n = int.Parse(lines[0].Split(' ')[0]);
            m = int.Parse(lines[0].Split(' ')[1]);
            w = 0;
            edges = new Edge[m];

            K = new int[n - 1];
            H = new int[n];
            L = new int[n];
            X = new int[n];
            M = new int[n];

            //Инициализация данных в задаче Объединить-Найти
            for (int k = 0; k < n; k++)
            {
                H[k] = k;
                L[k] = -1;
                X[k] = 1;
                M[k] = k;
            }

            for (int i = 1; i <= m; i++)
            {
                string[] args = lines[i].Split(' ');
                edges[i - 1] = new Edge
                {
                    I = int.Parse(args[0]),
                    J = int.Parse(args[1]),
                    C = int.Parse(args[2])
                };
            }
        }

        private void SortEdges()
        {
            Array.Sort(edges);
        }

        private int Find(int x)
        {
            if (M[x] == x) return x;
            return M[x] = Find(M[x]);
        }

        private void Union(int mi, int mj)
        {
            int k, l;
            //k – номер меньшего, l – номер большего подмн-ва

            bool isMiSmallerThanMj = (X[mi] < X[mj]);
            k = isMiSmallerThanMj ? mi : mj;
            l = isMiSmallerThanMj ? mj : mi;


            //обход меньшего подмножества
            int i;
            for (i = H[k]; L[i] != -1; i = L[i])
                M[i] = l; //перекрашиваем вершины

            M[i] = l;     //перекрашиваем последнюю вершину
            L[i] = H[l];  //к списку k прицепляем список l
            H[l] = H[k];  //переносим начало списка
            X[l] += X[k];  //размер нового подмножества
        }

        public void AlgKraskala()
        {
            int mi, mj;
            int i, j;
            SortEdges();
            for (int k = 0; k < m && w < n - 1; k++)
            {
                //Заканчиваем просмотр после исчерпания дуг 
                //графа или когда полностью сформируется кратчайшее связывающее дерево

                i = edges[k].I; //Начало дуги k
                j = edges[k].J; //Конец дуги k
                mi = Find(i); //Номера подмножеств, в которых
                mj = Find(j); //находятся элементы i и j.

                if (mi != mj)
                {
                    K[w] = k;//Добавляем ребро k в граф K
                    w++;     //Передвигаем указатель
                    Union(mi, mj);//Объединяем множества mi,mj
                }
            }
        }

        public void PrintToGraph(ResultGraphAfterUsingAlgorithms graphToPrint)
        {
            // These three instances can be injected via the IGetStartProcessQuery, 
            //                                               IGetProcessStartInfoQuery and 
            //                                               IRegisterLayoutPluginCommand interfaces

            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);

            // GraphGeneration can be injected via the IGraphGeneration interface

            var wrapper = new GraphGeneration(getStartProcessQuery,
                                              getProcessStartInfoQuery,
                                              registerLayoutPluginCommand);
            string description = "digraph{";
            if (graphToPrint == ResultGraphAfterUsingAlgorithms.InitialGraph)
            {
                for (int t = 0; t < m; t++)
                {
                    description += edges[t].I + " -> " + edges[t].J +
                        " [label=" + edges[t].C + ",weight=" + edges[t].C + "] ;";
                }
            }
            else if (graphToPrint == ResultGraphAfterUsingAlgorithms.GraphAfterAlgKraskala)
            {
                for (int t = 0; t < w; t++)
                {
                    description += edges[K[t]].I + " -> " + edges[K[t]].J +
                        " [label=" + edges[K[t]].C + ",weight=" + edges[K[t]].C + "] ;";
                }
            }

            description += "}";

            byte[] output = wrapper.GenerateGraph(description, Enums.GraphReturnType.Png);
            using (Stream ms = new MemoryStream(output))
            {
                System.Drawing.Image i = System.Drawing.Image.FromStream(ms);
                i.Save(pathPrint, ImageFormat.Png);
            }

        }
    }
}
