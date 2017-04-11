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
