namespace Graphs_Labs
{
    public class Buckets
    {
        int[] B;//Первая вершина в черпаке
        int[] Fw;//Forward
        int[] Bk;//Backward

        public Buckets(int n)
        {
            B = new int[n];//Первая вершина в черпаке
            Fw = new int[n];
            Bk = new int[n];

            for (int i = 0; i < n; i++)
            {
                B[i] = -1;
            }
        }

        public int GetVertexFromBucket(int k)
        {
            int i = B[k];
            if (i != -1) B[k] = Fw[i];
            // удалили i из черпака
            return i;
        }

        public void InsertVertexIntoBucket(int i, int k)
        {
            int j = B[k];
            // Вставка элемента в начало
            Fw[i] = j;
            if (j != -1) Bk[j] = i;
            B[k] = i;
        }

        public void RemoveVertexFromBucket(int i, int k)
        {
            int fi = Fw[i]; // Следующая в черпаке
            int bi = Bk[i]; // Предыдущая в черпаке

            if (i == B[k])
            {
                B[k] = fi;   // i была первой в черпаке
            }
            else
            {
                Fw[bi] = fi; // i была не первой

                if (fi != -1) // i не последняя
                    Bk[fi] = bi;
            }
        }
    }
}
