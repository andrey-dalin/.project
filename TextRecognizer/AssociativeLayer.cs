using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRecognizer
{
    public class AssociativeLayer
    {
        private Neuron[] neurons;

        public AssociativeLayer(Neuron[] neurons)
        {
            this.neurons = neurons;
        }

        public void FindMatches()
        {
            //Из 2 статьи масштабирование
            for (int i = 0; i < neurons.Length; i++)
                for (int x = 0; x < neurons[0].weight.GetLength(0); x++)
                    for (int y = 0; y < neurons[0].weight.Rank; y++)
                    {
                        int input = neurons[i].input[y, x];
                        int weight = neurons[i].weight[y, x];

                        //если пиксель белый в входном пикселе, то убираем. Если черный, то в совпадения пишем текущий вес, присвоенный пикселю
                        neurons[i].matches[y, x] = input * weight;

                    }

        }
    }
}
