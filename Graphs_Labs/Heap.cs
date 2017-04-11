using System;

namespace Graphs_Labs
{
    public class Heap
    {
        int _n;
        int[] _array;
        public Heap(int[] array, int n)
        {
            _n = n;
            _array = array;
        }

        //НАРУЖНЫЙ РЕМОНТ 
        //УВЕЛИЧИЛСЯ ЭЛЕМЕНТ КУЧИ В ПОЗИЦИИ k0
        private void REM_N(int k0)
        {
            //ЦИКЛ ПОКА У ВЕРШИНЫ ЕСТЬ ПОТОМКИ
            int child1, child2;
            for (int k = k0; k < (_n - 1) / 2; k = child1)
            {
                child1 = 2 * k + 1;   //первый потомок k
                child2 = child1 + 1;   //второй потомок k
                if (child2 < _n && _array[child2] < _array[child1]) //Второй меньше первого
                    child1 = child2; //сравниваем со вторым потомком,
                                     //а иначе работаем с первым
                if (_array[k] < _array[child1]) break;
                int i = _array[k];
                _array[k] = _array[child1];
                _array[child1] = i;
            }
        }
        //ВНУТРЕННИЙ РЕМОНТ - УМЕНЬШИЛСЯ ЭЛЕМЕНТ КУЧИ В ПОЗИЦИИ k0
        private void REM_V(int k0)
        {
            int parent;
            //ЦИКЛ ПО СЛОЯМ ДЕРЕВА ДО КОРНЯ
            for (int k = k0; k > 0; k = parent)
            {
                parent = (k - 1) / 2; //предок k
                if (_array[parent] < _array[k]) break;
                int i = _array[k];
                _array[k] = _array[parent];
                _array[parent] = i;
            }
        }

        //ВЫБОР И УДАЛЕНИЕ МИНИМАЛЬНОГО ЭЛЕМЕНТА 
        private int GET_MIN()
        {
            int min = _array[0];
            _array[0] = _array[_n - 1];
            _n--;
            REM_N(0);
            return min;
        }

        //ДОБАВЛЕНИЕ ЭЛЕМЕНТА  a
        private void ADD(int a)
        {
            _array[_n] = a;
            _n++;
            REM_V(_n - 1);
        }
        //УДАЛЯЕМ ЭЛЕМЕНТ КУЧИ ИЗ ПОЗИЦИИ k0
        private void REMOVE(int k0)
        {
            int a = _array[k0];
            _array[k0] = _array[_n - 1];
            _n--;
            if (_array[k0] > a) //элемент увеличился
                REM_N(k0);  //наружный ремонт
            else             //элемент уменьшился
                REM_V(k0);  //внутренний ремонт
        }
        //ОКУЧИВАНИЕ
        private void HEAPIFY()
        {
            for (int k = (_n - 1) / 2; k >= 0; k--)
                REM_N(k);
        }

        //СОРТИРОВКА ДЕРЕВОМ
        public void SORT_TREE()
        {
            HEAPIFY();
            int k = _n;
            while (k > 0)
            {
                int i = _array[0];
                _array[0] = _array[k - 1];
                _array[k - 1] = i;
                k--;
                _n--;
                REM_N(0);
            }
        }


        //Просеивание
        private void Form(int[] a, int root, int bottom)
        {
            int new_elem;
            int child;
            new_elem = a[root];

            while (root <= bottom / 2)
            {
                child = 2 * root;
                if (child < bottom && a[child] < a[child + 1])
                    child++;
                if (new_elem >= a[child]) break;
                a[root] = a[child];
                root = child;
            }
            a[root] = new_elem;
        }

        public void Sort()
        {
            Write();
            for (int i = (_n / 2) - 1; i >= 0; i--)
            {
                Form(_array, i, _n);
            }
            // Просеиваем через пирамиду остальные элементы
            for (int i = _n - 1; i >= 1; i--)
            {
                int temp = _array[0];
                _array[0] = _array[i];
                _array[i] = temp;
                Form(_array, 0, i - 1);
                Write();
            }
            Write();
        }

        private void Write()
        {
            for (int i = 0; i < _n; i++)
            {
                Console.WriteLine("a[{0}] = {1}", i, _array[i]);
            }
            Console.WriteLine("--------");
        }
    }
}
