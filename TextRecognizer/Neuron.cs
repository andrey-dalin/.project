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
        public string pathToBMP;
        public float[,] matches;
        public float[,] weight;
        public float[,] input;
        public float sumOfMatches;
        public float limit = 180;
        public bool hasPath;

        public Neuron(string name)
        {
            this.name = name;
        }

        public void MakePathOfWeightBMP(string name, string pathOfFolder)
        {
            if (hasPath == false)
            {
                pathToBMP = @pathOfFolder + "\\" + name;
                //File.Create(@"A:\Andrey\.project\TextRecognizer\resource\letters\" + name + ".bmp").Close();             

                hasPath = true;
            }
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

        public void SaveWeights()
        {
            File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\weights\\" + name + ".txt").Close();

            StreamWriter streamWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\weights\\" + name + ".txt");

            for (int y = 0; y < NeuronWeb.ResolutionY; y++)
                for (int x = 0; x < NeuronWeb.ResolutionX; x++)
                {

                    //writer.Write(weight[y, x] + " ");
                    if (x == 0 & y != 0)
                    {
                        //writer.WriteLine();
                        streamWriter.WriteLine();
                    }
                    streamWriter.Write(weight[y, x] + " ");

                }
            streamWriter.Close();
        }

        public void GetLocalWeights()
        {
            StreamReader streamReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\weights\\" + name + ".txt");
            char[] textOfWeights = streamReader.ReadToEnd().ToCharArray();
            int i = 0;
            int x = 0;
            int y = 0;

            //for (int y = 0; y < NeuronWeb.ResolutionY; y++)
            //    for (int x = 0; x < NeuronWeb.ResolutionX; x++)
            //    {
            //        if (textOfWeights[i] == ',')
            //        {
            //            weight[y - 1, x - 1] = Convert.ToSingle(textOfWeights[i - 1] + textOfWeights[i] + textOfWeights[i + 1]);
            //            i += 2;
            //        }
            //        if (textOfWeights[i] == '0')
            //        {
            //            weight[y, x] = 0;
            //        }
            //        if (textOfWeights[i] == '1')
            //        {
            //            weight[y, x] = 1;
            //        }
            //        i++;
            while (i < textOfWeights.Length)
            {
                string temp = string.Empty;
                while (textOfWeights[i] != ' ' | textOfWeights[i] != '\n')
                {
                    temp.Append(textOfWeights[i]);
                }
                weight[y, x] = Convert.ToSingle(temp);
                i++;
                x++;
                y++;
            }
        }
    }
}

