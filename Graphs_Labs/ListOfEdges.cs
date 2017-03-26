using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;

namespace Graphs_Labs
{
    public class ListOfEdges
    {
        private int[] IJ, L, H, _stack, _color;
        private int[] W;    //Weights
        private int m, n;
        private int w, componentNumber;
        private int[] Hn;   //номер первого непросмотренного ребра, выходящего из вершины 
        private string pathPrint;

        public ListOfEdges(int n, int[] I, int[] J)
        {
            this.n = n;

            this.m = I.Length;
            IJ = new int[2 * m];
            H = new int[n];
            L = new int[2 * m];
            _stack = new int[n];
            _color = new int[n];

            for (int i = 0; i < n; i++) { H[i] = _color[i] = -1; }

            for (int k = 0; k < m; k++)
            {
                IJ[k] = I[k];
                IJ[2 * m - 1 - k] = J[k];
            }
            // Обход ребер сети
            for (int k = 0; k < 2 * m; k++)
            {
                int i = IJ[k]; // один из концов ребра
                // добавляем ребро в начало списка i
                L[k] = H[i];
                H[i] = k;
            }
            Hn = H;
        }

        public ListOfEdges(int n, int[] I, int[] J, int[] WeightArray)
        {
            this.n = n;
            this.m = I.Length;

            IJ = new int[2 * m];
            H = new int[n];
            L = new int[2 * m];
            _stack = new int[n];
            _color = new int[n];
            W = new int[2 * m];

            for (int i = 0; i < n; i++) { H[i] = _color[i] = -1; }

            for (int k = 0; k < m; k++)
            {
                IJ[k] = I[k];
                IJ[2 * m - 1 - k] = J[k];
                W[k] = W[2 * m - 1 - k] = WeightArray[k];
            }
            // Обход ребер сети
            for (int k = 0; k < 2 * m; k++)
            {
                int i = IJ[k]; // один из концов ребра
                // добавляем ребро в начало списка i
                L[k] = H[i];
                H[i] = k;
            }
            Hn = H;
        }

        public ListOfEdges(string pathInput, string pathOutput, string pathPrint)
        {
            this.pathPrint = pathPrint;
            InitFromFile(pathInput);
        }

        private void InitFromFile(string pathInput)
        {
            string[] lines = File.ReadAllLines(pathInput);
            foreach (var item in lines)
            {
                item.Trim();
            }
            n = int.Parse(lines[0].Split(' ')[0]);
            m = int.Parse(lines[0].Split(' ')[1]);

            bool AreWeightGiven;
            AreWeightGiven = bool.TryParse(lines[0].Split(' ')[2], out AreWeightGiven);

            if (AreWeightGiven) W = new int[2 * m];

            IJ = new int[2 * m];

            for (int k = 1; k <= m; k++)
            {
                string[] args = lines[k].Split(' ');
                IJ[k - 1] = int.Parse(args[0]);
                IJ[2 * m - k] = int.Parse(args[1]);
                if (AreWeightGiven) W[k - 1] = W[2 * m - k] = int.Parse(args[2]);
            }

            H = new int[n];
            L = new int[2 * m];
            _stack = new int[n];
            _color = new int[n];

            for (int i = 0; i < n; i++) { H[i] = _color[i] = -1; }
            // Обход ребер сети
            for (int k = 0; k < 2 * m; k++)
            {
                int i = IJ[k]; // один из концов ребра
                // добавляем ребро в начало списка i
                L[k] = H[i];
                H[i] = k;
            }
            Hn = H;
        }

        public void BreadthFirstSearch(int s)
        {
            int[] R, P, Q;
            //R - массив растояний
            //P - модифицированный массив предков
            //Q - очередь вершин для просмотра
            R = new int[n];
            P = new int[n];
            int read, write;
            Q = new int[n];
            int i, j;
            for (i = 0; i < n; i++)
            {
                R[i] = n;
                P[i] = -2; //Пока все вершины недоступны или недостижимы
            }
            R[s] = 0;
            P[s] = -1;

            Q[0] = s;
            read = 0;         // Откуда читать
            write = 1;         // Куда писать

            while (read < write) // Пока очередь не пуста
            {
                i = Q[read];   // Берем очередную вершину для 
                               // просмотра
                read++;        // Передвигаем указатель чтения

                // Просмотр дуг, выходящих из вершины i
                for (int k = H[i]; k != -1; k = L[k])
                {
                    j = IJ[2 * m - 1 - k];
                    if (R[j] == n)// Вершина j не помечена
                    {
                        R[j] = R[i] + 1; //Расстояние до j на единицу больше, чем до i
                        P[j] = k;      //Последняя дуга на пути
                                       //из s в j - это k
                        Q[write] = j;        //Помещаем вершину j в 
                                             //очередь на просмотр
                        write++;     //Передвигаем указатель записи
                    }
                }
            }

            Console.WriteLine("Results поиска в глубину");
            for (int t = 0; t < n; t++)
            {
                Console.WriteLine("R[{0}] = {1}", t, R[t]);
            }

        }

        public void BreadthFirstSearch(int a, int b, int c)
        {
            int[,] R, P, Q;
            int[] colorForBFSearch;
            Dictionary<Tuple<int, int>, int> results = new Dictionary<Tuple<int, int>, int>();

            R = new int[n, 3];
            P = new int[n, 3];
            Q = new int[n, 3];
            colorForBFSearch = new int[n];

            int read = 0;
            int[] writeArr = new int[3];

            int[] i_vertex = new int[3];
            int[] j_vertex = new int[3];

            for (int m = 0; m < n; m++)
            {
                R[m, 0] = n;
                R[m, 1] = n;
                R[m, 2] = n;

                P[m, 0] = -2;
                P[m, 1] = -2;
                P[m, 2] = -2;

                colorForBFSearch[m] = -1;
            }
            colorForBFSearch[a] = a;
            colorForBFSearch[b] = b;
            colorForBFSearch[c] = c;

            R[a, 0] = 0;
            R[b, 1] = 0;
            R[c, 2] = 0;

            P[a, 0] = -1;
            P[b, 1] = -1;
            P[c, 2] = -1;

            Q[0, 0] = a;
            Q[0, 1] = b;
            Q[0, 2] = c;

            writeArr[0] = 1;
            writeArr[1] = 1;
            writeArr[2] = 1;


            int[] vertexes = new int[3] { a, b, c };

            while (read < writeArr[0])
            {
                i_vertex[0] = Q[read, 0];
                i_vertex[1] = Q[read, 1];
                i_vertex[2] = Q[read, 2];

                read++;

                for (int t = 0; t < 3; t++)
                {
                    for (int k = H[i_vertex[t]]; k != -1; k = L[k])
                    {
                        j_vertex[t] = IJ[2 * m - 1 - k];
                        if (R[j_vertex[t], t] == n)
                        {
                            R[j_vertex[t], t] = R[i_vertex[t], t] + 1;
                            P[j_vertex[t], t] = k;
                            Q[writeArr[t], t] = j_vertex[t];
                            writeArr[t]++;

                            if (colorForBFSearch[j_vertex[t]] == -1)
                            {
                                colorForBFSearch[j_vertex[t]] = vertexes[t];
                            }
                            else
                            {
                                int way = R[j_vertex[t], t] +
                                    R[j_vertex[t], Array.IndexOf<int>(vertexes, colorForBFSearch[j_vertex[t]])];
                                try
                                {
                                    results.Add(new Tuple<int, int>(vertexes[t], colorForBFSearch[j_vertex[t]]), way);
                                }
                                catch { }
                            }

                        }
                    }

                }
                if (results.Count != 0)
                {
                    break;
                }
            }

            Console.WriteLine("Результаты поиска в глубину для трех вершин: {0}, {1} и {2}", a, b, c);
            foreach (var item in results)
            {
                Console.WriteLine("{0} -> {1} за {2}", item.Key.Item1, item.Key.Item2, item.Value);
            }
        }

        public void ConnectivityСomponentAlg()
        {
            w = 0;
            componentNumber = -1;
            int currentVertex;
            int j, k;
            j = k = -1;
            for (int i = 0; i < n; i++) //Цикл по вершинам
            {
                if (_color[i] != -1) continue;

                componentNumber++; //Номер текущей компоненты связности
                currentVertex = i; // i – текущая вершина

                while (true)
                {
                    _color[currentVertex] = componentNumber; //Помечаем текущую вершину

                    //ПРОСМОТР РЕБЕР, ТЕКУЩЕЙ ВЕРШИНЫ, НАЧИНАЯ С ПЕРВОГО НЕПРОСМОТРЕННОГО
                    for (k = Hn[currentVertex]; k != -1; k = L[k])
                    {
                        j = IJ[2 * m - 1 - k]; //Вершина на другом конце ребра
                        if (_color[j] == -1) break;    //нашли непомеченную вершину
                    }

                    if (k != -1)
                    { //ШАГ ВПЕРЕД
                        Hn[currentVertex] = L[k]; //Запоминаем первую непросмотренную дугу в вершине i
                        _stack[w] = currentVertex; //Записываем вершину i в стек
                        w++;      //Перемещаем указатель стека
                        currentVertex = j;    //Теперь текущей вершиной 
                                              //будет j
                                              //Сделали шаг вперед
                    }
                    else
                    {// ШАГ НАЗАД
                        if (w == 0) break;
                        else
                        {
                            w--;  //Перемещаем указатель стека
                            currentVertex = _stack[w]; //Берем из стека 
                                                       //последнюю вершину
                        }
                        //Сделали шаг назад
                    }
                }
            }

        }

        public void Print()
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
            if (W != null)
            {
                for (int t = 0; t < m; t++)
                {
                    description += IJ[t] + " -> " + IJ[2 * m - 1 - t] + " [label=" + W[t] + ",weight=" + W[t] + "] ;";
                }
            }
            else
            {
                for (int t = 0; t < m; t++)
                {
                    description += IJ[t] + " -> " + IJ[2 * m - 1 - t] + " ;";
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

        public void AlgDeikstra(int s)
        {
            int[] R = new int[n];//Distances
            int[] P = new int[n];//Ancestor
            int[] Q = new int[n];//Queue
            int h_Q, i;
            for (i = 0; i < n; i++)
            {
                R[i] = int.MaxValue;
                P[i] = -2;  // Все вершины недоступны
            }

            R[s] = 0;
            P[s] = -1;

            h_Q = s;       //First in a queue
            Q[s] = -1;

            int min, pi, pj, j;
            pi = -1;

            while (h_Q != -1) // While queue is not empty
            {
                //ВЫБОР ВЕРШИНЫ С МИНИМАЛЬНЫМ ТЕКУЩИМ РАССТОЯНИЕМ
                min = int.MaxValue;
                //j  – текущая вершина при проходе по списку
                //pj - предыдущая при проходе по списку
                for (pj = -1, j = h_Q; j != -1; pj = j, j = Q[j])
                {
                    if (R[j] < min)
                    {
                        min = R[j]; //новый минимум
                        pi = pj;  //достигается на вершине,
                                  //следующей за pi
                    }
                }

                if (pi != -1)
                {
                    i = Q[pi];
                    Q[pi] = Q[i]; // Удаляем i из списка
                }
                else
                { // i – первая в списке
                    i = h_Q;
                    h_Q = Q[i]; //Передвигаем начало списка
                }

                // Просмотр дуг, выходящих из вершины i
                for (int k = H[i]; k != -1; k = L[k])
                {
                    j = IJ[2 * m - 1 - k]; // Противоположная вершина

                    int rj = R[j];
                    if (R[i] + W[k] < rj)
                    { // Основное соотношение нарушено
                        R[j] = R[i] + W[k]; //Новое текущее
                                            //расстояние до j
                        P[j] = k;    //Последняя дуга на пути
                                     //из s в j 
                        if (rj == int.MaxValue) // j была не помечена
                        { // Добавляем j в Q
                            Q[j] = h_Q;
                            h_Q = j;
                        }
                    }
                }
            }
        }
    }

}
