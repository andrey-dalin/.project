using System;
using System.Drawing;
using System.Linq;

namespace TextRecognizer
{
    public class NeuronWeb
    {
        public Neuron[] neurons;

        public void NamingNeurons()
        {
            for (int i = 'а', j = 0; i <= 'я'; i++, j++)
            {
                if (i == 'ж')
                {
                    this.neurons[j] = new Neuron("ё");
                    j++;
                }
                string symbol = Convert.ToChar(i).ToString();
                this.neurons[j] = new Neuron(symbol);
            }
        }

        public void MakePathForEveryone(string pathOfFolder)
        {
            foreach (Neuron neuron in neurons) neuron.MakePathBMPOfWeight(neuron.name, pathOfFolder);
        }

        public void SetResolutionForEveryone(int width, int height)
        {
            foreach (Neuron neuron in neurons) neuron.SetResolution(width, height);
        }

        public void SetInput(Bitmap input)
        {
            for (int i = 0; i < neurons.Length; i++)
                for (int x = 0; x < input.Width; x++)
                    for (int y = 0; y < input.Height; y++)
                    {
                        int colorOfPixel = Convert.ToInt32(input.GetPixel(x, y).R);
                        neurons[i].input[y, x] = colorOfPixel;
                    }


        }

        public void FindMatches()
        {
            //Из 2 статьи масштабирование
            for (int i = 0; i < neurons.Length; i++)
                for (int x = 0; x < neurons[0].weight.GetLength(0); x++)
                    for (int y = 0; y < neurons[0].weight.Rank; y++)
                    {
                        int input = neurons[i].input[y, x];
                        int match = neurons[i].matches[y, x];
                        int weight = neurons[i].weight[y, x];

                        //если пиксель белый в входном пикселе, то убираем. Если черный, то в совпадения пишем текущий вес, присвоенный пикселю
                        if (Neuron.IsWhite(input))
                        {
                            match = MyColors.White;
                        }
                        else
                        {
                            match = neurons[i].weight[y, x];
                        }
                    }

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
        public string Recognize(Bitmap input)
        {
            SetInput(input);
            FindMatches();
            Sum();
            return GetAGuess();
        }
    }
}
