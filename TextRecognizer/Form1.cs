using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
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

        private Pen pen = new Pen(Color.Black, 30f);

        private NeuronWeb neuronWeb = new NeuronWeb();

        private string guess = string.Empty;

        private enum answerOfNeuronWeb
        {
            CannotGuess,
            Guess,
            DrawSymbol,
            WriteTrueSymbol,
            Trained,
            PictureClean
        };

        private void MyInitialize()
        {
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            for (int x = 0; x < pictureBox1.Width; x++)
                for (int y = 0; y < pictureBox1.Height; y++)
                    bitmap.SetPixel(x, y, Color.White);
        

            pictureBox1.Image = bitmap;
            pictureBox2.Image = bitmap;
            graphics = Graphics.FromImage(bitmap);

            pictureBox1.Size = bitmap.Size;
            pictureBox2.Size = bitmap.Size;

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

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            startX = Convert.ToInt32(Math.Ceiling(scaleX * e.X));
            startY = Convert.ToInt32(Math.Ceiling(scaleY * e.Y));
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                graphics = Graphics.FromImage(bitmap);
                endX = Convert.ToInt32(Math.Ceiling(scaleX * e.X));
                endY = Convert.ToInt32(Math.Ceiling(scaleY * e.Y));
                Point start = new Point(startX, startY);
                Point end = new Point(endX, endY);
                if (startX != 0 & startY != 0)
                {
                    graphics.DrawLine(pen, start, end);
                    pictureBox1.Image = bitmap;
                    startX = endX;
                    startY = endY;
                }

            }
        }


        private void ToAnswerNeuronWeb(answerOfNeuronWeb answerOfNeuronWeb)
        {
            switch (answerOfNeuronWeb)
            {
                case answerOfNeuronWeb.CannotGuess:
                    toolStripStatusLabel1.Text = "ИИ: не могу угадать букву. Напишите правильную букву и нажмите обучить";
                    guess = string.Empty;
                    toolStripStatusLabel1.BackColor = Color.DeepPink;
                    toolStripTextBoxTrueSymbol.Focus();
                    break;

                case answerOfNeuronWeb.Guess:
                    toolStripStatusLabel1.Text = "ИИ: я думаю это буква" + " " + guess + ".";
                    if (guess == string.Empty)
                        throw new Exception("guess is empty string");

                    toolStripStatusLabel1.BackColor = Color.DeepSkyBlue;
                    toolStripTextBoxTrueSymbol.Focus();
                    break;

                case answerOfNeuronWeb.DrawSymbol:
                    toolStripStatusLabel1.Text = "ИИ: нарисуйте букву.";
                    guess = string.Empty;
                    toolStripStatusLabel1.BackColor = Color.OrangeRed;
                    break;

                case answerOfNeuronWeb.WriteTrueSymbol:
                    toolStripStatusLabel1.Text = "ИИ: напишите правильную букву, чтобы обучить.";
                    guess = string.Empty;
                    toolStripStatusLabel1.BackColor = Color.OrangeRed;
                    break;

                case answerOfNeuronWeb.Trained:
                    toolStripStatusLabel1.Text = "ИИ: понял свои ошибки. Продолжайте рисовать.";
                    guess = string.Empty;
                    toolStripTextBoxTrueSymbol.Text = string.Empty;

                    toolStripStatusLabel1.BackColor = Color.Orange;
                    break;

                case answerOfNeuronWeb.PictureClean:
                    toolStripStatusLabel1.Text = "ИИ: холст очищен. Начинайте рисовать.";
                    guess = string.Empty;
                    toolStripTextBoxTrueSymbol.Text = string.Empty;

                    toolStripStatusLabel1.BackColor = Color.Green;
                    break;
            }
        }
        private void Clear()
        {

            graphics.Clear(Color.White);
            pictureBox1.Image = bitmap;
            
            
        }

        private void toolStripRecognize_Click(object sender, EventArgs e)
        {
            guess = neuronWeb.Recognize(bitmap);

            if (guess == string.Empty)
            {
                ToAnswerNeuronWeb(answerOfNeuronWeb.CannotGuess);
                return;
            }

            ToAnswerNeuronWeb(answerOfNeuronWeb.Guess);
            Bitmap temp = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            Graphics g = Graphics.FromImage(temp);
            RectangleF rectangle = new RectangleF(0f, 0f, pictureBox2.Width, pictureBox2.Height);
            Bitmap weightInBMP = Converter.ArrayToBMP(neuronWeb.FindNeuron(guess).weight);
            g.DrawImage(weightInBMP, rectangle);
            pictureBox2.Image = temp;
        }

        private void toolStripClean_Click(object sender, EventArgs e)
        {
            Clear();
            ToAnswerNeuronWeb(answerOfNeuronWeb.PictureClean);
        }


        private void toolStripTrain_Click(object sender, EventArgs e)
        {
            if (toolStripTextBoxTrueSymbol.Text == string.Empty)
            {
                ToAnswerNeuronWeb(answerOfNeuronWeb.WriteTrueSymbol);
                return;
            }

            neuronWeb.SetInput(bitmap);
            neuronWeb.Train(toolStripTextBoxTrueSymbol.Text.ToLower(), guess);
            

            Bitmap temp = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            Graphics g = Graphics.FromImage(temp);
            RectangleF rectangle = new RectangleF(0f, 0f, pictureBox2.Width, pictureBox2.Height);
            Bitmap weightInBMP = Converter.ArrayToBMP(neuronWeb.FindNeuron(toolStripTextBoxTrueSymbol.Text.ToLower()).weight);
            g.DrawImage(weightInBMP, rectangle);
            pictureBox2.Image = temp;


            Neuron neuron = neuronWeb.FindNeuron(toolStripTextBoxTrueSymbol.Text.ToLower());
            float sum = 0;
            
            for (int x = 0; x < neuron.weight.GetLength(1); x++)
            {
                for (int y = 0; y < neuron.weight.GetLength(0); y++)
                {
                    sum += neuron.weight[y, x];
                }
            }

            ToAnswerNeuronWeb(answerOfNeuronWeb.Trained);

            toolStripStatusLabel1.Text = sum.ToString();
            Clear();
            int sumOfFonts;
            InstalledFontCollection fonts = new InstalledFontCollection();
            sumOfFonts = fonts.Families.Length;
            toolStripStatusLabel2.Text = sumOfFonts.ToString();
        }




        private void toolStripComboScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (toolStripComboScale.SelectedIndex)
            {
                case 0:
                    pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
                    pictureBox1.Dock = DockStyle.None;
                    pictureBox1.BorderStyle = BorderStyle.FixedSingle;

                   
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

            pictureBox2.Width = (int)(pictureBox2.Width * 1.2);
            pictureBox2.Height = (int)(pictureBox2.Height * 1.2);

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

                pictureBox2.Width = (int)(pictureBox2.Width / 1.2);
                pictureBox2.Height = (int)(pictureBox2.Height / 1.2);

                scaleX = (double)pictureBox1.Image.Width / pictureBox1.Bounds.Width;
                scaleY = (double)pictureBox1.Image.Height / pictureBox1.Bounds.Height;

                scale = (int)(scale / 1.2);
            }
        }
    }
}

