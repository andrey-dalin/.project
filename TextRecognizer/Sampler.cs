using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRecognizer
{
    public static class Sampler
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
        public static void SaveImage(string name, Bitmap bitmap)
        {            
            bitmap.Save(AppDomain.CurrentDomain.BaseDirectory + "samples\\" + name + ".bmp");
        }
        public static void SaveImage(string name, float[,] array)
        {
            Bitmap bitmap = ArrayToBMP(array);
            bitmap.Save(AppDomain.CurrentDomain.BaseDirectory + "samples\\" + name + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
        }
        public static void CreateSamplesFolder()
        {
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "samples\\");
        }
        public static void DeleteSamplesFolder()
        {
            Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "samples\\", true);
        }
    }
}
