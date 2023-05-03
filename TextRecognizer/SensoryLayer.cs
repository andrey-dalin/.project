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
        private NeuronWeb neuronWeb;
        public SensoryLayer(NeuronWeb neuronWeb) 
        {
            this.neuronWeb = neuronWeb;
        }
        public void SetInput(Bitmap input)
        {
            for (int i = 0; i < neuronWeb.Neurons.Length; i++)
                for (int x = 0; x < input.Width; x++)
                    for (int y = 0; y < input.Height; y++)
                    {
                        int colorOfPixel = Convert.ToInt32(input.GetPixel(x, y).R);
                        neuronWeb.Neurons[i].input[y, x] = Neuron.IsWhite(neuronWeb.Neurons[i].input[y, x]);
                    }


        }
    }
}
