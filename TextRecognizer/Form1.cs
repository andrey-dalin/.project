using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextRecognizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            NeuronWeb.ResolutionX = 30;
            NeuronWeb.ResolutionY = 30;
            InitializeComponent();
            SetSize();
            StartNeuronWeb();
        }

        private bool IsMouseDown;

        private ArrayPoints arrayPoints = new ArrayPoints(2);

        private Bitmap picture = new Bitmap(100, 100);

        private Graphics graphics;

        private Pen pen = new Pen(Color.Black, 3f);

        private NeuronWeb neuronWeb = new NeuronWeb();

        private string guess;

        private void SetSize()
         {
            Rectangle rectangle = Screen.PrimaryScreen.Bounds;
            picture = new Bitmap(90, 90);
            graphics = Graphics.FromImage(picture);

            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
        }

        private void StartNeuronWeb()
        {
            int numberOfRussianLetters = 33;
            string pathOfFolder = "A:\\Andrey\\.project\\TextRecognizer\\resource\\letters";            

            neuronWeb.Neurons = new Neuron[numberOfRussianLetters];

            //Создаём имя для каждого нейрона
            neuronWeb.NamingNeurons();

            //Создаём путь для дальнейшего сохранения веса
            neuronWeb.MakePathForEveryone(pathOfFolder);

            //Задаём разрешение 
            neuronWeb.SetResolutionForEveryone();
        }

        private void Clear()
        {

            graphics.Clear(pictureBox1.BackColor);
            pictureBox1.Image = picture;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            IsMouseDown = true;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            IsMouseDown = false;
            arrayPoints.ResetPoints();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDown == false) return;

            arrayPoints.SetPoint(e.X, e.Y);

            if (arrayPoints.GetCountPoints() >= 2)
            {
                graphics.DrawLines(pen, arrayPoints.GetPoints());

                pictureBox1.Image = picture;

                arrayPoints.SetPoint(e.X, e.Y);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //
            Bitmap asInput = new Bitmap(pictureBox1.Image, NeuronWeb.ResolutionX, NeuronWeb.ResolutionY);
            pictureBox2.Image = new Bitmap(asInput, picture.Size);

            guess = neuronWeb.Recognize((Bitmap)pictureBox1.Image);
            if (guess == string.Empty)
            {
                
                label4.Text = "ИИ не может угадать букву. Напишите правильную букву и нажмите обучить";
                return;
            }

            Neuron guessNeuron = neuronWeb.FindNeuron(guess);
            Bitmap weightToBMP = Converter.ArrayToBMP(guessNeuron.weight);
            pictureBox2.Image = new Bitmap(weightToBMP, picture.Size);

            label4.Text = "ИИ считает, что это буква – " + guess.ToUpper();
            label4.BackColor = Color.Aqua;

            textBox1.Text = guess;
            textBox1.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label4.Text = "Нарисуйте букву для ИИ";
            guess = "";
            textBox1.Text = "";
            Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show("Напишите правильную букву");
                return;
            }
                string trueName = textBox1.Text;
                string falseName = guess;

                neuronWeb.Train(trueName, falseName);

                textBox1.Text = "";
                label4.Text = "ИИ понял свои ошибки, но не до конца. Продолжайте рисовать.";
                label4.BackColor = Color.OrangeRed;
                Clear();
            if (falseName == string.Empty)
            {
                Neuron trueNeuron = neuronWeb.FindNeuron(trueName);
                Bitmap weightToBMP = Converter.ArrayToBMP(trueNeuron.weight);
                pictureBox2.Image = new Bitmap(weightToBMP, picture.Size);
            }
        }
    }
}
