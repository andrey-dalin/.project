using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRecognizer
{
    public static class Visualizer
    {
        public static string weightSuffix = "weight";
        public static string inputSuffix = "input";
        public static string matchesSuffix = "matches";

        public static Bitmap ArrayToBMP(float[,] array)
        {
            Bitmap bitmap = new Bitmap(NeuronWeb.ResolutionX, NeuronWeb.ResolutionY);

            for (int y = 0; y < array.GetLength(0); y++)
                for (int x = 0; x < array.GetLength(1); x++)
                {
                    int R = Convert.ToInt32(255 - array[y, x] * 255);
                    Color color = Color.FromArgb(R, R, R);

                    bitmap.SetPixel(x, y, color);
                }
            return bitmap;
        }
        public static void SaveImage(string path, Bitmap bitmap)
        {            
            bitmap.Save(path + ".bmp");
        }
        public static void SaveImage(string path, float[,] array)
        {
            Bitmap bitmap = ArrayToBMP(array);
            bitmap.Save(path +".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
        }

        public static void CreateSamplesFolder(string path)
        {
            Directory.CreateDirectory(path);
        }
        public static void DeleteSamplesFolder(string path)
        {
            Directory.Delete(path, true);
        }
    }
}
