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
            MyInitialize();
            StartNeuronWeb();
            toolStripComboScale.SelectedIndex = 0;
        }
        int startX, startY, endX, endY = 0;

        double scaleX;
        double scaleY;

        private int scale = 100;

        Bitmap bitmap;

        private Graphics graphics;

        private Pen pen = new Pen(Color.Black, 20f);

        private NeuronWeb neuronWeb = new NeuronWeb();

        private string guess;

        private void MyInitialize()
        {
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bitmap;
            graphics = Graphics.FromImage(bitmap);


            scaleX = (double)pictureBox1.Image.Width / pictureBox1.Bounds.Width;
            scaleY = (double)pictureBox1.Image.Height / pictureBox1.Bounds.Height;

            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

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
            pictureBox1.Image = bitmap;
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            startX = Convert.ToInt32(Math.Ceiling(scaleX * e.X));
            startY = Convert.ToInt32(Math.Ceiling(scaleY * e.Y));
        }
        
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {


                Graphics g = Graphics.FromImage(bitmap);
                endX = Convert.ToInt32(Math.Ceiling(scaleX * e.X));
                endY = Convert.ToInt32(Math.Ceiling(scaleY * e.Y));
                Point start = new Point(startX, startY);
                Point end = new Point(endX, endY);
                if (startX != 0 & startY != 0)
                {
                    g.DrawLine(pen, start, end);
                    pictureBox1.Image = bitmap;
                    startX = endX;
                    startY = endY;
                }
                
            }
        }



        private void toolStripRecognize_Click(object sender, EventArgs e)
        {

            //
            //Bitmap asInput = new Bitmap(pictureBox1.Image, NeuronWeb.ResolutionX, NeuronWeb.ResolutionY);
            //pictureBox2.Image = new Bitmap(asInput, pictureBox2.Image.Width, pictureBox2.Image.Height);

            guess = neuronWeb.Recognize((Bitmap)pictureBox1.Image);
            if (guess == string.Empty)
            {

                toolStripStatusLabel1.Text = "ИИ не может угадать букву. Напишите правильную букву и нажмите обучить";
                return;
            }

            Neuron guessNeuron = neuronWeb.FindNeuron(guess);
            Bitmap weightToBMP = Converter.ArrayToBMP(guessNeuron.weight);
            pictureBox2.Image = new Bitmap(weightToBMP, pictureBox2.Width, pictureBox2.Height);

            toolStripStatusLabel1.Text = "ИИ считает, что это буква – " + guess.ToUpper();
            toolStripStatusLabel1.BackColor = Color.Aqua;

            toolStripTextBoxTrueSymbol.Text = guess;
            toolStripTextBoxTrueSymbol.Focus();
        }

        private void toolStripClean_Click(object sender, EventArgs e)
        {

            toolStripStatusLabel1.Text = "Нарисуйте букву для ИИ";
            guess = "";
            toolStripTextBoxTrueSymbol.Text = "";
            Clear();
        }


        private void toolStripTrain_Click(object sender, EventArgs e)
        {
            if (toolStripTextBoxTrueSymbol.Text == string.Empty)
            {
                MessageBox.Show("Напишите правильную букву");
                return;
            }
            string trueName = toolStripTextBoxTrueSymbol.Text;
            string falseName = guess;

            neuronWeb.Train(trueName, falseName);

            toolStripTextBoxTrueSymbol.Text = "";
            toolStripStatusLabel1.Text = "ИИ понял свои ошибки, но не до конца. Продолжайте рисовать.";
            toolStripStatusLabel1.BackColor = Color.OrangeRed;
            Clear();
            if (falseName == string.Empty)
            {
                Neuron trueNeuron = neuronWeb.FindNeuron(trueName);
                Bitmap weightToBMP = Converter.ArrayToBMP(trueNeuron.weight);
                pictureBox2.Image = new Bitmap(weightToBMP, pictureBox2.Width, pictureBox2.Height);
            }
        }




        private void toolStripComboScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (toolStripComboScale.SelectedIndex)
            {
                case 0:
                    pictureBox1.Size = bitmap.Size;
                    pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
                    pictureBox1.Dock = DockStyle.None;
                    pictureBox1.BorderStyle = BorderStyle.FixedSingle;

                    pictureBox2.Size = bitmap.Size;
                    pictureBox2.SizeMode = PictureBoxSizeMode.Normal;
                    pictureBox2.Dock = DockStyle.None;
                    pictureBox2.BorderStyle = BorderStyle.FixedSingle;

                    toolStripPlus.Enabled = false;
                    toolStripMinus.Enabled = false;


                    scaleX = (double)pictureBox1.Image.Width / pictureBox1.Bounds.Width;
                    scaleY = (double)pictureBox1.Image.Height / pictureBox1.Bounds.Height;

                    break;               

                case 1:
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.Dock = DockStyle.Fill;

                    pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox2.Dock = DockStyle.Fill;
                    pictureBox2.BorderStyle = BorderStyle.FixedSingle;

                    toolStripPlus.Enabled = false;
                    toolStripMinus.Enabled = false;


                    scaleX = (double)pictureBox1.Image.Width / pictureBox1.Bounds.Width;
                    scaleY = (double)pictureBox1.Image.Height / pictureBox1.Bounds.Height;

                    break;

                case 2:
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox1.Dock = DockStyle.None;
                    pictureBox1.BorderStyle = BorderStyle.FixedSingle;

                    pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox2.Dock = DockStyle.None;
                    pictureBox2.BorderStyle = BorderStyle.FixedSingle;

                    toolStripPlus.Enabled = true;
                    toolStripMinus.Enabled = true;


                    scaleX = (double)pictureBox1.Image.Width / pictureBox1.Bounds.Width;
                    scaleY = (double)pictureBox1.Image.Height / pictureBox1.Bounds.Height;

                    scale = 100;

                    break;

            }
        }

        private void toolStripPlus_Click(object sender, EventArgs e)
        {


            pictureBox1.Width = (int)(pictureBox1.Width * 1.2);
            pictureBox1.Height = (int)(pictureBox1.Height * 1.2);

            pictureBox2.Width = (int)(pictureBox1.Width * 1.2);
            pictureBox2.Height = (int)(pictureBox1.Height * 1.2);

            scaleX = (double)pictureBox1.Image.Width / pictureBox1.Bounds.Width;
            scaleY = (double)pictureBox1.Image.Height / pictureBox1.Bounds.Height;

            scale = (int)(scale * 1.2);
        }

        private void toolStripMinus_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Width > 12 & pictureBox1.Height > 12)
            {
                pictureBox1.Width = (int)(pictureBox1.Width / 1.2);
                pictureBox1.Height = (int)(pictureBox1.Height / 1.2);

                pictureBox2.Width = (int)(pictureBox1.Width / 1.2);
                pictureBox2.Height = (int)(pictureBox1.Height / 1.2);

                scaleX = (double)pictureBox1.Image.Width / pictureBox1.Bounds.Width;
                scaleY = (double)pictureBox1.Image.Height / pictureBox1.Bounds.Height;

                scale = (int)(scale / 1.2);
            }
        }
    }
}
