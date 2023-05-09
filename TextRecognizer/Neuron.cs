using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TextRecognizer.Properties;

namespace TextRecognizer
{
    public class Neuron
    {
        public string name;
        public float[,] matches;
        public float[,] weight;
        public float[,] input;
        public float sumOfMatches;
        public float limit = 180;

        public Neuron(string name)
        {
            this.name = name;
        }
        public void SetResolution(int width, int height)
        {
            matches = new float[height, width];
            weight = new float[height, width];
            input = new float[height, width];
        }

        public static bool IsWhite(float pixel)
        {
            return pixel > MyColors.NearWhite;
        }

        public void SaveWeightsTXT()
        {
            File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\weights\\" + name + ".txt").Close();

            StreamWriter streamWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\weights\\" + name + ".txt");

            for (int y = 0; y < NeuronWeb.ResolutionY; y++)
                for (int x = 0; x < NeuronWeb.ResolutionX; x++)
                {
                    if (x == 0 & y != 0)
                    {
                        streamWriter.WriteLine();
                    }
                    streamWriter.Write(Math.Round(weight[y, x], 1) + " ");
                }
            streamWriter.Close();
        }

        public void GetLocalWeightsTXT()
        {
            StreamReader streamReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\weights\\" + name + ".txt");
            string textOfWeights = streamReader.ReadToEnd();
            int i = 0;

            for (int y = 0; y < NeuronWeb.ResolutionY; y++)
                for (int x = 0; x < NeuronWeb.ResolutionX; x++)
                {
                    if (textOfWeights[i] == ' ') i++;
                    if (textOfWeights[i] == '\r') i++;
                    if (textOfWeights[i] == '\n') i++;
                    if (textOfWeights[i] == '0' & textOfWeights[i + 1] == ' ')
                    {
                        weight[y, x] = 0;
                        i += 2;
                    }
                    else if (textOfWeights[i] == '0' && textOfWeights[i + 1] == ',')
                    {
                        weight[y, x] = Convert.ToSingle(textOfWeights[i].ToString() +
                            textOfWeights[i + 1].ToString() +
                            textOfWeights[i + 2].ToString());
                        i += 3;
                    }
                    else if (textOfWeights[i] == '1' & textOfWeights[i + 1] == ' ')
                    {
                        weight[y, x] = 1;
                        i += 2;
                    }
                }
            streamReader.Close();
        }        
    }
}

