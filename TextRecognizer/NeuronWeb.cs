using System;
using System.Drawing;
using System.Linq;

namespace TextRecognizer
{
    public class NeuronWeb
    {
        public Neuron[] Neurons;
        public static int ResolutionX;
        public static int ResolutionY;

        public Neuron FindNeuron(string name)
        {
            int index = Array.FindIndex(Neurons, (Neuron neuron) => neuron.name == name);
            return Neurons[index];
        }
        public void NamingNeurons()
        {
            for (int i = 'а', j = 0; i <= 'я'; i++, j++)
            {
                if (i == 'ж')
                {
                    Neurons[j] = new Neuron("ё");
                    j++;
                }
                string symbol = Convert.ToChar(i).ToString();
                Neurons[j] = new Neuron(symbol);
            }
        }        
        public void SetResolutionForEveryone()
        {
            foreach (Neuron neuron in Neurons) neuron.SetResolution(ResolutionX, ResolutionY);
        }     
        public void SetInput(Bitmap input)
        {
           Bitmap scaledInput = new Bitmap(input, ResolutionX, ResolutionY);

            for (int i = 0; i < Neurons.Length; i++)
                for (int x = 0; x < ResolutionX; x++)
                    for (int y = 0; y < ResolutionY; y++)
                    {
                        float colorOfPixel = Convert.ToInt32(scaledInput.GetPixel(x, y).R);

                        if (Neuron.IsWhite(colorOfPixel)) Neurons[i].input[y, x] = MyColors.White;
                        else Neurons[i].input[y, x] = MyColors.Black;
                    }
        }
        public void FindMatches()
        {
            for (int i = 0; i < Neurons.Length; i++)
                for (int y = 0; y < ResolutionY; y++)
                    for (int x = 0; x < ResolutionX; x++)
                        Neurons[i].matches[y, x] = Neurons[i].input[y, x] * Neurons[i].weights[y, x];
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
            int index;
            float[] sums = new float[Neurons.Length];

            for (int i = 0; i < Neurons.Length; i++) sums[i] = Neurons[i].sumOfMatches;

            index = Array.FindIndex(sums, (float match) => match == sums.Max());
            
            if (Neurons[index].sumOfMatches > Neurons[index].limit) return Neurons[index].name;
            else return string.Empty;
          
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
            if (falseName == string.Empty) return;

            Trainer.DecrementWeight(falseName, Neurons);
        }        
    }
}
