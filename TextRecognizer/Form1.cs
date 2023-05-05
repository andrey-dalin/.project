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
        int x0, y0;

        private int scale = 100;

        Bitmap bitmap = new Bitmap(600, 600);

        private Graphics graphics;

        private Pen pen = new Pen(Color.Black, 3f);

        private NeuronWeb neuronWeb = new NeuronWeb();

        private string guess;

        private void SetSize()
        {
            x0 = y0 = 0;

            pictureBox1.Image = new Bitmap(200, 200);
            graphics = Graphics.FromImage(bitmap);

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
            //pictureBox1.Image = picture;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Graphics g = Graphics.FromImage(bitmap);
                g.DrawLine(pen, x0 - scale /100, y0 -scale / 100, e.X, e.Y);
                pictureBox1.Image = bitmap;
            }

            x0 = e.X;
            y0 = e.Y;

        }

        private void toolStripRecognize_Click(object sender, EventArgs e)
        {

            //
            Bitmap asInput = new Bitmap(pictureBox1.Image, NeuronWeb.ResolutionX, NeuronWeb.ResolutionY);
            pictureBox2.Image = new Bitmap(asInput, 90, 90);

            guess = neuronWeb.Recognize((Bitmap)pictureBox1.Image);
            if (guess == string.Empty)
            {

                toolStripStatusLabel1.Text = "ИИ не может угадать букву. Напишите правильную букву и нажмите обучить";
                return;
            }

            Neuron guessNeuron = neuronWeb.FindNeuron(guess);
            Bitmap weightToBMP = Converter.ArrayToBMP(guessNeuron.weight);
            pictureBox2.Image = new Bitmap(weightToBMP, 90, 90);

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
                pictureBox2.Image = new Bitmap(weightToBMP, 90, 90);
            }
        }


        private void toolStripComboScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (toolStripComboScale.SelectedIndex)
            {
                case 0:
                    pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
                    pictureBox1.Dock = DockStyle.Fill;
                    toolStripPlus.Enabled = false;
                    toolStripMinus.Enabled = false;

                    MessageBox.Show(pictureBox1.Size.ToString());
                    break;

                case 1:
                    pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                    pictureBox1.Dock = DockStyle.Fill;
                    toolStripPlus.Enabled = false;
                    toolStripMinus.Enabled = false;
                    break;

                case 2:
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.Dock = DockStyle.Fill;
                    toolStripPlus.Enabled = false;
                    toolStripMinus.Enabled = false;
                    break;

                case 3:
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox1.Dock = DockStyle.Fill;
                    toolStripPlus.Enabled = false;
                    toolStripMinus.Enabled = false;


                    break;

                case 4:
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.Dock = DockStyle.None;
                    toolStripPlus.Enabled = true;
                    toolStripMinus.Enabled = true;

                    scale = 100;

                    MessageBox.Show(pictureBox1.Size.ToString());
                    break;

            }
        }






        private void toolStripPlus_Click(object sender, EventArgs e)
        {
            pictureBox1.Width = (int)(pictureBox1.Width * 1.2);
            pictureBox1.Height = (int)(pictureBox1.Height * 1.2);

            scale = (int)( scale * 1.2);
        }
        private void toolStripMinus_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Width > 12 & pictureBox1.Height > 12)
            {
                pictureBox1.Width = (int)(pictureBox1.Width / 1.2);
                pictureBox1.Height = (int)(pictureBox1.Height / 1.2);

                scale = (int)(scale / 1.2);
            }           
        }






    }
}
