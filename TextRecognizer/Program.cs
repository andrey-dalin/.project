using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextRecognizer
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            int resolutionX = 30;
            int resolutionY = 30;
            int numberOfRussianLetters = 33;
            string pathOfFolder = "A:\\Andrey\\.project\\TextRecognizer\\resource\\letters";

            NeuronWeb neuronWeb = new NeuronWeb();

            neuronWeb.neurons = new Neuron[numberOfRussianLetters];


            //Создаём имя для каждого нейрона
            neuronWeb.NamingNeurons();

            //Создаём путь для дальнейшего сохранения веса
            neuronWeb.MakePathForEveryone(pathOfFolder);

            //Задаём разрешение 
            neuronWeb.SetResolutionForEveryone(resolutionX, resolutionY);

            


            
        }

        
    }
}
