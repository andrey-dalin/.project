using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TextRecognizer
{
    public class NeuronWeb
    {
        public Neuron[] Neurons;

        public static int ResolutionX;
        public static int ResolutionY;

        public Neuron FindNeuron(string name)
        {
            if (name == string.Empty)
            {
                return null;
            }
            int indexInArray = Array.FindIndex(Neurons, (Neuron neuron) => neuron.name == name);
            return Neurons[indexInArray];
        }

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

        public void SetInput(Bitmap input)
        {
            if (input != null)
            {
                Bitmap scaledInput = new Bitmap(input, new Size(ResolutionX, ResolutionY));

                for (int i = 0; i < Neurons.Length; i++)
                    for (int x = 0; x < scaledInput.Width; x++)
                        for (int y = 0; y < scaledInput.Height; y++)
                        {
                            float colorOfPixel = Convert.ToInt32(scaledInput.GetPixel(x, y).R);

                            if (Neuron.IsWhite(colorOfPixel))
                            {
                                Neurons[i].input[y, x] = MyColors.White;
                            }
                            else
                            {
                                Neurons[i].input[y, x] = MyColors.Black;
                            }
                        }
            }
            else
            {
                MessageBox.Show("Нарисуйте букву");
            }
        }

        public void FindMatches()
        {
            //Из 2 статьи масштабирование
            for (int i = 0; i < Neurons.Length; i++)
                for (int y = 0; y < Neurons[0].weight.GetLength(0); y++)
                    for (int x = 0; x < Neurons[0].weight.GetLength(1); x++)
                    {
                        float input = Neurons[i].input[y, x];

                        Neurons[i].matches[y, x] = input * Neurons[i].weight[y, x];
                    }
        }

        public void Sum()
        {
            //прибавляем в сумме совпадений все совпадающие пиксели
            for (int i = 0; i < Neurons.Length; i++)
                for (int y = 0; y < Neurons[0].weight.GetLength(0); y++)
                    for (int x = 0; x < Neurons[0].weight.GetLength(1); x++)
                    {
                        Neurons[i].sumOfMatches += Neurons[i].matches[y, x];
                    }
        }

        public string GetAGuess()
        {
            //находим нейрон, в котором максимальная сумма совпадений
            int numberInArray;
            float maxBlackInSums;
            float[] sums = new float[Neurons.Length];

            //создаём массив с суммами совпадений
            for (int i = 0; i < Neurons.Length; i++)
            {
                sums[i] = Neurons[i].sumOfMatches;
            }

            maxBlackInSums = sums.Max();

            //находим номер индекса в массиве с максимальной суммой совпадений
            numberInArray = Array.FindIndex(sums, (float match) => match == maxBlackInSums);

            if (Neurons[numberInArray].sumOfMatches >= Neurons[numberInArray].limit)
            {
                return Neurons[numberInArray].name;
            }
            else
            {                
                return string.Empty;
            }
        }
        public string Recognize(Bitmap input)
        {
            //Сенсорный слой
            SetInput(input);
            //Ассоциативный слой
            FindMatches();
            //Реагирующий слой
            Sum();
            return GetAGuess();
        }

        public void Train(string trueName, string falseName)
        {
            Trainer.IncrementWeight(trueName, Neurons);
            if (falseName == string.Empty) 
            {
                return;
            }
            Trainer.DecrementWeight(falseName, Neurons);
        }
    }
}
