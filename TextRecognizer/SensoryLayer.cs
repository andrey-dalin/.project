using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRecognizer
{
    public class SensoryLayer
    {
        private Neuron[] neurons;
        public SensoryLayer(Neuron[] neurons) 
        {
            this.neurons = neurons;
        }
        public void SetInput(Bitmap input)
        {
            for (int i = 0; i < neurons.Length; i++)
                for (int x = 0; x < input.Width; x++)
                    for (int y = 0; y < input.Height; y++)
                    {
                        int colorOfPixel = Convert.ToInt32(input.GetPixel(x, y).R);
                        neurons[i].input[y, x] = Neuron.IsWhite(neurons[i].input[y, x]);
                    }


        }
    }
}
