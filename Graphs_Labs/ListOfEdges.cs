using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs_Labs
{
    public class ListOfEdges
    {
        private int[] IJ, L, H, _stack, _color;
        private int m, n;
        private int w, componentNumber;
        private int[] Hn; //– номер первого непросмотренного ребра, выходящего из вершины 

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

                while (true) //Пока не попробуем сделать шаг 
                             //назад при пустом стеке
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
                        //Стек пуст. Обошли всю компоненту.
                        //Выходим из цикла While(1)
                        else
                        {  //Стек не пуст 
                            w--;  //Перемещаем указатель стека
                            currentVertex = _stack[w]; //Берем из стека 
                                                       //последнюю вершину
                        }
                        //Сделали шаг назад
                    }
                    //После шага вперед или назад переходим к 
                    //просмотру дуг из новой текущей вершины i
                }
            }

        }
    }
}
