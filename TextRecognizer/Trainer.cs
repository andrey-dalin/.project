using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRecognizer
{
    public static class Trainer
    {
        public static void DecrementWeight(string name, Neuron[] neurons)
        {
            int indexOfNeuron = Array.FindIndex(neurons, x => x.name == name);

            for (int x = 0; x < neurons[indexOfNeuron].weight.GetLength(0); x++) 
                for (int y = 0; y < neurons[indexOfNeuron].weight.Rank;  y++)
                {
                    neurons[indexOfNeuron].weight[y, x] -= 
                        neurons[indexOfNeuron].input[y, x] / 2;
                }
        }

        public static void IncrementWeight(string name, Neuron[] neurons)
        {
            int indexOfNeuron = Array.FindIndex(neurons, x => x.name == name);

            for (int x = 0; x < neurons[indexOfNeuron].weight.GetLength(0); x++)
                for (int y = 0; y < neurons[indexOfNeuron].weight.Rank; y++)
                {
                    neurons[indexOfNeuron].weight[y, x] +=
                        neurons[indexOfNeuron].input[y, x] / 2;
                }
        }
    }
}
