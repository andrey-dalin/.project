using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextRecognizer
{
    public class Neuron
    {
        public string name;
        public string pathToBMP;
        public int[,] matches;
        public int[,] weight;
        public int[,] input;
        public int limit;
        public int sumOfMatches;
        public bool hasPath;

       public Neuron(string name)
        {
            this.name = name;
        }

        public void MakePathBMPOfWeight(string name, string pathOfFolder)
        {
            if (hasPath == false)
            {
                pathToBMP = @pathOfFolder + "\\" + name + ".bmp";
                //File.Create(@"A:\Andrey\.project\TextRecognizer\resource\letters\" + name + ".bmp").Close();             
                                
                hasPath = true;
            }            
        }

        public void SetResolution(int width, int height)
        {
            matches = new int[height, width];
            weight = new int[height, width];
            input = new int[height, width];
        }

    }

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
            //for (int i = 0; i < neurons.Length; i++)
            //{
            //    for (int x = 0; x < neurons[0].multiply.Width; x++)
            //    {
            //        for (int y = 0; y < neurons[0].multiply.Height; y++)
            //        {
            //            if (Convert.ToInt32(neurons[i].input.GetPixel(x, y).R) > 250)
            //            {
            //                neurons[i].multiply.SetPixel(x, y, Color.White);
            //            }
            //            else
            //            {
            //                neurons[i].multiply.SetPixel(x, y, neurons[i].weight.GetPixel(x, y));
            //            }
            //        }
            //    }
            //}

            //Из 2 статьи масштабирование
            for (int i = 0; i < neurons.Length; i++)
                for (int x = 0; x < neurons[0].weight.GetLength(0); x++)
                    for (int y = 0; y < neurons[0].weight.Rank; y++)
                    {
                        //если пиксель белый в входном пикселе, то убираем, если черный, то в совпадения пишем текущий вес, присвоенный пикселю
                        if (neurons[i].input[y, x] > 250)
                        {
                            neurons[i].matches[y, x] = 0;
                        }
                        else
                        {
                            neurons[i].matches[y,x] = neurons[i].weight[y, x];
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
            int maxValueInSums;
            int[] sums = new int[neurons.Length];

            //создаём массив с суммами совпадений
            for (int i = 0; i < neurons.Length; i++)
            {
                sums[i] = neurons[i].sumOfMatches;
            }

            //ищем максимальную сумму совпадений
            maxValueInSums = sums.Max();

            //находим номер индекса в массиве с максимальной суммой совпадений
            numberInArray = Array.Find(sums, (int match) => match == maxValueInSums);
            

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
