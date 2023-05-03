using System;
using System.Drawing;
using System.Linq;

namespace TextRecognizer
{
    public class NeuronWeb
    {
        public static Neuron[] Neurons;

        public static int ResolutionX;
        public static int ResolutionY;

        public SensoryLayer SensoryLayer = new SensoryLayer(Neurons);
        public AssociativeLayer AssociativeLayer = new AssociativeLayer(Neurons);
        public ReactingLayer ReactingLayer = new ReactingLayer(Neurons);

        public void NamingNeurons()
        {
            for (int i = 'а', j = 0; i <= 'я'; i++, j++)
            {
                if (i == 'ж')
                {
                    this.Neurons[j] = new Neuron("ё");
                    j++;
                }
                string symbol = Convert.ToChar(i).ToString();
                this.Neurons[j] = new Neuron(symbol);
            }
        }

        public void MakePathForEveryone(string pathOfFolder)
        {
            foreach (Neuron neuron in Neurons) neuron.MakePathBMPOfWeight(neuron.name, pathOfFolder);
        }

        public void SetResolutionForEveryone()
        {
            foreach (Neuron neuron in Neurons) neuron.SetResolution(ResolutionX, ResolutionY);
        }

        

       

        

        
        public string Recognize(Bitmap input)
        {
            SensoryLayer.SetInput(input);
            AssociativeLayer.FindMatches();
            ReactingLayer.Sum();
            return ReactingLayer.GetAGuess();


        }

        public void Train(string trueName, string falseName)
        {
            Trainer.IncrementWeight(trueName, Neurons);
            Trainer.DecrementWeight(falseName, Neurons);
        }
    }
}
