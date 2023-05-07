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
                throw new ArgumentNullException("name", "name is empty string");

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
            if (input == null)
                throw new ArgumentNullException("input", "SetInput is null");

            

            for (int i = 0; i < Neurons.Length; i++)
                for (int x = 0; x < input.Width; x++)
                    for (int y = 0; y < input.Height; y++)
                    {
                        float colorOfPixel = Convert.ToInt32(input.GetPixel(x, y).R);

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

        public void FindMatches()
        {
            for (int i = 0; i < Neurons.Length; i++)
                for (int y = 0; y < Neurons[0].weight.GetLength(0); y++)
                    for (int x = 0; x < Neurons[0].weight.GetLength(1); x++)
                        Neurons[i].matches[y, x] = Neurons[i].input[y, x] * Neurons[i].weight[y, x];
        }

        public void Sum()
        {
            for (int i = 0; i < Neurons.Length; i++)
                for (int y = 0; y < Neurons[0].weight.GetLength(0); y++)
                    for (int x = 0; x < Neurons[0].weight.GetLength(1); x++)
                        Neurons[i].sumOfMatches += Neurons[i].matches[y, x];
        }

        public string GetAGuess()
        {
            int numberInArray;
            float maxBlackInSums;
            float[] sums = new float[Neurons.Length];
                       
            for (int i = 0; i < Neurons.Length; i++)
                sums[i] = Neurons[i].sumOfMatches;

            maxBlackInSums = sums.Max();

            numberInArray = Array.FindIndex(sums, (float match) => match == maxBlackInSums);


            return Neurons[numberInArray].name;
          
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
