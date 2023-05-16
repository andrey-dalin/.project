using System;

namespace TextRecognizer
{
    public static class Trainer
    {
        //можно настраивать точность обучения
        public static float Accuracy = 10;

        public static void DecrementWeight(string name, Neuron[] neurons)
        {
            int indexOfNeuron = Array.FindIndex(neurons, x => x.name == name);

            for (int y = 0; y < neurons[indexOfNeuron].weights.GetLength(0); y++) 
                for (int x = 0; x < neurons[indexOfNeuron].weights.GetLength(1);  x++)
                {
                    neurons[indexOfNeuron].weights[y, x] -= 
                        neurons[indexOfNeuron].input[y, x] / Accuracy;

                    if (neurons[indexOfNeuron].weights[y, x] <= 0)
                    {
                        neurons[indexOfNeuron].weights[y, x] = 0;
                    }
                }
        }

        public static void IncrementWeight(string name, Neuron[] neurons)
        {
            int indexOfNeuron = Array.FindIndex(neurons, x => x.name == name);

            for (int y = 0; y < neurons[indexOfNeuron].weights.GetLength(0); y++)
                for (int x = 0; x < neurons[indexOfNeuron].weights.GetLength(1); x++)
                {
                    neurons[indexOfNeuron].weights[y, x] +=
                        neurons[indexOfNeuron].input[y, x] / Accuracy;

                    if (neurons[indexOfNeuron].weights[y, x] >= 1)
                    {
                        neurons[indexOfNeuron].weights[y, x] = 1;
                    }
                }
        }
    }
}
