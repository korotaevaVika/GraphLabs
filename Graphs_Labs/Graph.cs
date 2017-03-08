using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace Graphs_Labs
{
    public class Graph
    {
        private int m, n;
        private int[] H, L;
        private int[] I;
        private int[] J;

        private string pathInput;
        private string pathOutput;
        private string pathPrint;
        private string message;

        public Graph(string pathInput, string pathOutput, string pathPrint)
        {
            this.pathInput = pathInput;
            this.pathOutput = pathOutput;
            this.pathPrint = pathPrint;
            this.message = string.Empty;
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

        public void Add(int i, int j)
        {
            message = "Add (" + i + "\t " + j + ")";
            int[] I_1, J_1, L_1;
            I_1 = new int[m + 1];
            J_1 = new int[m + 1];
            L_1 = new int[m + 1];

            for (int p = 0; p < m; p++)
            {
                I_1[p] = I[p];
                J_1[p] = J[p];
                L_1[p] = L[p];
            }
            I_1[m] = i;
            J_1[m] = j;
            L = L_1;
            I = I_1;
            J = J_1;

            if (H[i] == -1)
            {
                H[i] = m;
                L[m] = -1;
            }
            else
            {
                for (int k = H[i]; k != -1; k = L[k])
                {
                    if (L[k] == -1)
                    {
                        L[k] = m;
                        L[m] = -1;
                        break;
                    }
                }
            }

            m++;
            PrintToFile();
            PrintToGraph();
        }

        public void Delete(int number)
        {
            if (I.Length != 0 && number < m)
            {
                message = "Delete (" + I[number] + "\t" + J[number] + ")";
                int[] I_1, J_1, L_1;
                I_1 = new int[m - 1];
                J_1 = new int[m - 1];
                L_1 = new int[m - 1];

                for (int p = 0; p < m - 1; p++)
                {
                    if (p < number)
                    {
                        I_1[p] = I[p];
                        J_1[p] = J[p];
                        L_1[p] = L[p];
                    }
                    else
                    {
                        I_1[p] = I[p + 1];
                        J_1[p] = J[p + 1];
                        L_1[p] = (L[p + 1] == -1 ? L[p + 1] : (L[p + 1] > number) ? L[p + 1] - 1 : L[p + 1]);
                    }
                }
                int[] H_1 = new int[n];

                for (int p = 0; p < n; p++)
                {
                    if (H[p] != -1 && H[p] > number)
                    {
                        H_1[p] = H[p] - 1;
                    }
                    else H_1[p] = H[p];
                }
                //int count = 0;
                int i = I[number];
                for (int k = H[i]; L[k] != -1; k = L[k])
                {
                    if (H[i] == number)
                    {
                        H_1[i] = L[k];
                        break;
                    }
                    if (L[k] == number)
                    {

                        if (L[L[k]] == -1)
                        {
                            if (k < number)
                            {
                                L_1[k] = -1;
                            }
                            else
                            {
                                L_1[k - 1] = -1;
                            }
                        }
                        else
                            if ((L[L[k]] < number))
                        {
                            if (k < number)
                            {
                                L_1[k] = L[L[k]];
                            }
                            else
                            {
                                L_1[k - 1] = L[L[k]];
                            }
                        }
                        else if ((L[L[k]] > number))
                        {
                            if (k < number)
                            {
                                L_1[k] = L[L[k]] - 1;
                            }
                            else
                            {
                                L_1[k - 1] = L[L[k]] - 1;
                            }
                        }
                        break;
                    }
                }

                I = I_1;
                J = J_1;
                L = L_1;
                H = H_1;
                m--;
                PrintToFile();
                PrintToGraph();
                message = string.Empty;
            }
        }

        public void PrintToFile()
        {
            using (StreamWriter file =
                new StreamWriter(this.pathOutput))
            {
                file.WriteLine(DateTime.Today.ToString());
                file.WriteLine(message);
                file.WriteLine("I J L");

                for (int p = 0; p < m; p++)
                {
                    file.WriteLine(I[p] + " " + J[p] + " " + L[p]);
                }
                file.WriteLine("H: ");
                for (int p = 0; p < n; p++)
                {
                    file.Write(H[p] + "\t");
                }
                file.WriteLine("----------------\n\n");
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
            byte[] output = wrapper.GenerateGraph(description, Enums.GraphReturnType.Png);
            using (Stream ms = new MemoryStream(output))
            {
                System.Drawing.Image i = System.Drawing.Image.FromStream(ms);
                i.Save(pathPrint, ImageFormat.Png);
            }
            OpenFolder(this.pathPrint);
        }

        private void OpenFolder(string filePath)
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
            }
            else
            {
                Console.WriteLine(string.Format("{0} Directory does not exist!", folderPath));
            }
        }

    }
}
