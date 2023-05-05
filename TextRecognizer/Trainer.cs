using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRecognizer
{
    public static class Trainer
    {
        //можно настраивать точность обучения
        public static float Accuracy = 10;

        public static void DecrementWeight(string name, Neuron[] neurons)
        {
            int indexOfNeuron = Array.FindIndex(neurons, x => x.name == name);

            for (int y = 0; y < neurons[indexOfNeuron].weight.GetLength(0); y++) 
                for (int x = 0; x < neurons[indexOfNeuron].weight.GetLength(1);  x++)
                {
                    neurons[indexOfNeuron].weight[y, x] -= 
                        neurons[indexOfNeuron].input[y, x] / Accuracy;

                    if (neurons[indexOfNeuron].weight[y, x] <= 0)
                    {
                        neurons[indexOfNeuron].weight[y, x] = 0;
                    }
                }
        }

        public static void IncrementWeight(string name, Neuron[] neurons)
        {
            int indexOfNeuron = Array.FindIndex(neurons, x => x.name == name);

            for (int y = 0; y < neurons[indexOfNeuron].weight.GetLength(0); y++)
                for (int x = 0; x < neurons[indexOfNeuron].weight.GetLength(1); x++)
                {
                    neurons[indexOfNeuron].weight[y, x] +=
                        neurons[indexOfNeuron].input[y, x] / Accuracy;

                    if (neurons[indexOfNeuron].weight[y, x] >= 1)
                    {
                        neurons[indexOfNeuron].weight[y, x] = 1;
                    }
                }
        }
    }
}
