using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRecognizer
{
    public class ReactingLayer
    {
        private NeuronWeb neuronWeb;

        public ReactingLayer(NeuronWeb neuronWeb) 
        {
            this.neuronWeb = neuronWeb;
        }

        public void FindMatches()
        {
            //Из 2 статьи масштабирование
            for (int i = 0; i < neuronWeb.Neurons.Length; i++)
                for (int x = 0; x < neuronWeb.Neurons[0].weight.GetLength(0); x++)
                    for (int y = 0; y < neuronWeb.Neurons[0].weight.Rank; y++)
                    {
                        int input = neuronWeb.Neurons[i].input[y, x];                        
                        int weight = neuronWeb.Neurons[i].weight[y, x];

                        //если пиксель белый в входном пикселе, то убираем. Если черный, то в совпадения пишем текущий вес, присвоенный пикселю
                        neuronWeb.Neurons[i].matches[y, x] = input * weight;

                    }

        }

    }
}
