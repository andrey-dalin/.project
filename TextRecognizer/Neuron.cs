using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public float limit = 300f;
        public bool hasPath;

        public Neuron(string name)
        {
            this.name = name;
        }

        public void MakePathBMPOfWeight(string name, string pathOfFolder)
        {
            if (hasPath == false)
            {
                pathToBMP = @pathOfFolder + "\\" + name + ".bmp";
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


    }
}
