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
        public int[,] matches;
        public int[,] weight;
        public int[,] input;
        public int sumOfMatches;
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
            matches = new int[height, width];
            weight = new int[height, width];
            input = new int[height, width];
        }

        public static bool IsWhite(int pixel)
        {
            return pixel > MyColors.NearWhite;
        }

        public Bitmap WeightToBMP()
        {
            Bitmap bitmap = new Bitmap(NeuronWeb.ResolutionX, NeuronWeb.ResolutionY);

            for (int y = 0; y < weight.GetLength(0); y++)
            {
                for (int x = 0; x < weight.GetLength(1); x++)
                {
                    int R = (int)weight[y, x];
                    int G = weight[y, x];
                    int B = weight[y, x];
                    Color color = Color.FromArgb(R, G, B);
                    bitmap.SetPixel(x, y, color);
                }
            }
            return bitmap;
        }

        public void WeightInColorToValueWeight()
        {

        }
    }
}
