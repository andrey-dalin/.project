using System;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;

namespace TextRecognizer
{
    public class Perceptron
    {
        public Neuron[] Neurons = new Neuron[33];
        public static int ResolutionX = 400;
        public static int ResolutionY = 400;
        public float Accuracy = 10;

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

                        if (colorOfPixel > 250) Neurons[i].input[y, x] = 0;
                        else Neurons[i].input[y, x] = 1;
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
            Sum();
            //Реагирующий слой
            return GetAGuess();
        }
        public void Train(string trueName, string falseName)
        {
            int indexOfTrueNeuron = Array.FindIndex(Neurons, x => x.name == trueName);
            int indexOfFalseNeuron = Array.FindIndex(Neurons, x => x.name == falseName);

            for (int y = 0; y < ResolutionY; y++)
                for (int x = 0; x < ResolutionX; x++)
                {
                    Neurons[indexOfTrueNeuron].weights[y, x] += 
                        Neurons[indexOfTrueNeuron].input[y, x] / Accuracy;

                    if (Neurons[indexOfTrueNeuron].weights[y, x] >= 1) 
                        Neurons[indexOfTrueNeuron].weights[y, x] = 1;

                    if (falseName != string.Empty)
                    {
                        Neurons[indexOfFalseNeuron].weights[y, x] -= 
                            Neurons[indexOfFalseNeuron].input[y, x] / Accuracy;

                        if (Neurons[indexOfFalseNeuron].weights[y, x] <= 0)
                            Neurons[indexOfFalseNeuron].weights[y, x] = 0;
                    }
                }
        }
    }
}
