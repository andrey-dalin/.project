using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRecognizer
{
    public static class Converter
    {
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
    }
}
