using System.IO;

namespace Graphs_Labs
{
    public class Edge
    {
        private int i;
        private int j;
        private int weight;

        public int I { get { return i; } set { i = value; } }
        public int J { get { return j; } set { j = value; } }
        public int W { get { return weight; } set { weight = value; } }
    }

    public class ListOfEdgesWithWeigh
    {
        private string pathInput;
        private string pathOutput;
        private string pathPrint;

        private int n, m;
        private Edge[] edges;

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
            edges = new Edge[m];
            
            for (int i = 1; i <= m; i++)
            {
                string[] args = lines[i].Split(' ');
                edges[i] = new Edge
                {
                    I = int.Parse(args[0]),
                    J = int.Parse(args[1]),
                    W = int.Parse(args[2])
                };
            }
        }
    }
}
