using System;
using System.Drawing;
using System.IO;
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
        public void SetResolutionForEveryone()
        {
            foreach (Neuron neuron in Neurons) neuron.SetResolution(ResolutionX, ResolutionY);
        }
        public void SaveWeights()
        {
            foreach (Neuron neuron in Neurons) neuron.SaveWeights();
        }
        public void GetLocalWeights()
        {
            foreach (Neuron neuron in Neurons) neuron.GetLocalWeights();
        }
        public void SetInput(Bitmap input)
        {
            if (input == null)
                throw new ArgumentNullException("input", "SetInput is null");

           Bitmap scaledInput = new Bitmap(input, ResolutionX, ResolutionY);

            for (int i = 0; i < Neurons.Length; i++)
                for (int x = 0; x < ResolutionX; x++)
                    for (int y = 0; y < ResolutionY; y++)
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
        public void FindMatches()
        {
            for (int i = 0; i < Neurons.Length; i++)
                for (int y = 0; y < ResolutionY; y++)
                    for (int x = 0; x < ResolutionX; x++)
                        Neurons[i].matches[y, x] = Neurons[i].input[y, x] * Neurons[i].weight[y, x];
        }
        public void Sum()
        {
            for (int i = 0; i < Neurons.Length; i++)
                for (int y = 0; y < ResolutionY; y++)
                    for (int x = 0; x < ResolutionX; x++)
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
        public void CreateWeightsFolder()
        {
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\weights");
        }
    }
}
