using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRecognizer
{
    public class ReactingLayer
    {
        private Neuron[] neurons;

        public ReactingLayer(Neuron[] neurons)
        {
            this.neurons = neurons;
        }

        public void Sum()
        {
            //прибавляем в сумме совпадений все совпадающие пиксели
            for (int i = 0; i < neurons.Length; i++)
                for (int x = 0; x < neurons[0].weight.GetLength(0); x++)
                    for (int y = 0; y < neurons[0].weight.Rank; y++)
                    {
                        neurons[i].sumOfMatches += neurons[i].matches[y, x];
                    }
        }

        public string GetAGuess()
        {
            //находим нейрон, в котором максимальная сумма совпадений
            int numberInArray;
            int maxBlackInSums;
            int[] sums = new int[neurons.Length];

            //создаём массив с суммами совпадений
            for (int i = 0; i < neurons.Length; i++)
            {
                sums[i] = neurons[i].sumOfMatches;
            }

            //ищем максимальную сумму совпадений, чем ближе к 0 тем лучше, так как черный цвет стремиться к нулю
            maxBlackInSums = sums.Min();

            //находим номер индекса в массиве с максимальной суммой совпадений
            numberInArray = Array.FindIndex(sums, (int match) => match == maxBlackInSums);


            return neurons[numberInArray].name;

        }
    }
}
