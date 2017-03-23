using System;
using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace Graphs_Labs
{
    public class ListOfArcs
    {
        private int m, n;
        private int[] H, L;
        private int[] I;
        private int[] J;
        private int free;

        private string pathInput;
        private string pathOutput;
        private string pathPrint;
        private string message;

        #region Properties
        public int[] VertexI { get { return I; } }
        public int[] VertexJ { get { return J; } }
        public int QuantityVertex { get { return n; } }
        #endregion Properties

        public ListOfArcs(string pathInput, string pathOutput, string pathPrint)
        {
            this.pathInput = pathInput;
            this.pathOutput = pathOutput;
            this.pathPrint = pathPrint;
            this.message = string.Empty;
            free = -1;
            Init(this.pathInput);
        }

        public ListOfArcs(
            int[] I,
            int[] J,
            int[] H,
            int[] L,
            int free,
            int n,
            string pathOutput,
            string pathPrint)
        {
            this.I = I;
            this.J = J;
            this.H = H;
            this.L = L;
            this.m = I.Length;
            this.n = n;
            this.free = free;
            this.pathOutput = pathOutput;
            this.pathPrint = pathPrint;
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

            I = new int[m];
            J = new int[m];
            H = new int[n];
            L = new int[m];

            for (int i = 1; i <= m; i++)
            {
                string[] args = lines[i].Split(' ');
                I[i - 1] = int.Parse(args[0]);
                J[i - 1] = int.Parse(args[1]);
            }
            for (int i = 0; i < n; i++)
            {
                H[i] = -1;
            }
            // Обход дуг сети
            for (int k = 0; k < m; k++)
            {
                int i = I[k]; // начальная вершина дуги
                              // добавляем дугу k в начало списка i
                L[k] = H[i]; // k ссылается на дугу, которая
                             // раньше была первой в списке i
                H[i] = k;    // k становится первой дугой в
                             // списке i
            } // списки 
        }

        public void Smart_Add(int i, int j)
        {
            message = "Add (" + i + "\t " + j + ")";
            if (free == -1)
            {
                int[] I_1, J_1, L_1;
                I_1 = new int[m * 2];
                J_1 = new int[m * 2];
                L_1 = new int[m * 2];

                for (int p = 0; p < m; p++)
                {
                    I_1[p] = I[p];
                    J_1[p] = J[p];
                    L_1[p] = L[p];
                }
                for (int h = m; h < 2 * m; h++)
                {
                    L_1[h] = free;
                    free = h;
                }
                L = L_1;
                I = I_1;
                J = J_1;


                m *= 2;
            }

            {
                var fut_free = L[free];

                I[free] = i;
                J[free] = j;

                var pos = free;
                if (H[i] == -1)
                {
                    H[i] = pos;
                    L[pos] = -1;
                }
                else
                {
                    L[free] = H[i];
                    H[i] = pos;
                }

                free = fut_free;
            }

            PrintToFile();
            PrintToGraph();
        }

        public void Smart_Delete(int number)
        {
            if (I.Length != 0 && number < m)
            {
                message = "Delete (" + I[number] + "\t" + J[number] + ")";

                int i = I[number];
                for (int k = H[i]; k != -1; k = L[k])
                {
                    if (H[i] == number)
                    {
                        H[i] = L[k];
                        break;
                    }
                    if (L[k] == number)
                    {
                        L[k] = L[L[k]];
                        break;
                    }
                }
                if (free == -1)
                {
                    free = number;
                    L[free] = -1;
                }
                else
                {
                    L[number] = free;
                    free = number;
                    //for (int k = free; k != -1; k = L[k])
                    //{
                    //    if (L[k] == -1)
                    //    {
                    //        L[k] = number;
                    //        break;
                    //    }
                    //}
                    //L[number] = -1;
                }

                message = string.Empty;
            }
        }


        public void PrintToFile()
        {
            using (StreamWriter file =
                new StreamWriter(this.pathOutput))
            {
                string description = string.Empty;
                file.WriteLine(DateTime.Today.ToString());
                file.WriteLine(message);

                file.WriteLine("I -> J");
                for (int t = 0; t < m; t++)
                {
                    description += I[t] + " -> " + J[t] + "; \n";
                }
                description += "\n";
                if (free != -1)
                {
                    for (int t = free; t != -1; t = L[t])
                    {
                        description = description.Replace(I[t] + " -> " + J[t] + "; ", string.Empty);
                    }
                }
                file.WriteLine(description);
                description = string.Empty;
                file.WriteLine("H Array -> ");

                for (int i = 0; i < n; i++)
                {
                    description += H[i] + "\t";
                }
                file.WriteLine(description);
                description = string.Empty;
                file.WriteLine("L Array -> ");
                for (int i = 0; i < m; i++)
                {
                    description += L[i] + "\t";
                }
                file.WriteLine(description + "\n---");

                message = string.Empty;
            }

        }
        public void PrintToGraph()
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
            for (int t = 0; t < m; t++)
            {
                description += I[t] + " -> " + J[t] + "; ";
            }
            description += "}";
            if (free != -1)
            {
                for (int t = free; t != -1; t = L[t])
                {
                    description = description.Replace(I[t] + " -> " + J[t] + "; ", string.Empty);
                }
            }
            byte[] output = wrapper.GenerateGraph(description, Enums.GraphReturnType.Png);
            using (Stream ms = new MemoryStream(output))
            {
                System.Drawing.Image i = System.Drawing.Image.FromStream(ms);
                i.Save(pathPrint, ImageFormat.Png);
            }

        }

        public void OpenFolder(string filePath)
        {
            string folderPath = filePath.Remove(filePath.LastIndexOf(Path.DirectorySeparatorChar));
            if (Directory.Exists(folderPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = folderPath,
                    FileName = "explorer.exe"
                };
                Process.Start(startInfo);
                Console.WriteLine(string.Format("{0} Directory open...", folderPath));
            }
            else
            {
                Console.WriteLine(string.Format("{0} Directory does not exist!", folderPath));
            }
        }

        public ListOfArcs Clone()
        {
            return new ListOfArcs(I, J, H, L, free, n, this.pathOutput, this.pathPrint);
        }


    }

}
