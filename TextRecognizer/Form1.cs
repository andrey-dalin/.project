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
using System.Xml.Linq;

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
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(System.Globalization.CultureInfo.GetCultureInfo("Ru"));
        }
        private string pathOfSamples = AppDomain.CurrentDomain.BaseDirectory + "samples\\";
        private int iterationOfSampleGroup;
        private int startX, startY, endX, endY;
        private double scaleX;
        private double scaleY;
        private int scale = 100;
        private Bitmap inputPicture;
        private Bitmap weightPicture;
        private Bitmap matchesPicture;
        private Graphics graphics;
        private Pen pen = new Pen(Color.Black, 30f);
        private NeuronWeb neuronWeb = new NeuronWeb();
        private string guess = string.Empty;

        private enum answers
        {
            CannotGuess,
            Guess,
            DrawSymbol,
            WriteTrueSymbol,
            Trained,
            PictureClean,
            PutRussianLayout,
            LocalWeight
        };
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            startX = Convert.ToInt32(Math.Ceiling(scaleX * e.X));
            startY = Convert.ToInt32(Math.Ceiling(scaleY * e.Y));
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                graphics = Graphics.FromImage(inputPicture);
                endX = Convert.ToInt32(Math.Ceiling(scaleX * e.X));
                endY = Convert.ToInt32(Math.Ceiling(scaleY * e.Y));
                Point start = new Point(startX, startY);
                Point end = new Point(endX, endY);
                if (startX != 0 & startY != 0)
                {
                    graphics.DrawLine(pen, start, end);
                    pictureBox1.Image = inputPicture;
                    startX = endX;
                    startY = endY;
                }

            }
        }
        private void toolStripRecognize_Click(object sender, EventArgs e)
        {
            guess = neuronWeb.Recognize(inputPicture);            

            if (guess == string.Empty)
            {
                ToAnswer(answers.CannotGuess);
                SaveSamples();
                return;
            }

            ToAnswer(answers.Guess);

            ShowMatches(guess);
            SaveSamples();
            
        }
        private void toolStripClean_Click(object sender, EventArgs e)
        {
            Clear();
            ToAnswer(answers.PictureClean);
        }
        private void toolStripTrain_Click(object sender, EventArgs e)
        {
            if (toolStripTextBoxTrueSymbol.Text == string.Empty)
            {
                ToAnswer(answers.WriteTrueSymbol);
                return;
            }
            if (Array.FindIndex(neuronWeb.Neurons, x => x.name == toolStripTextBoxTrueSymbol.Text.ToLower()) < 0)
            {
                ToAnswer(answers.PutRussianLayout);
                return;
            }

            neuronWeb.SetInput(inputPicture);
            neuronWeb.Train(toolStripTextBoxTrueSymbol.Text.ToLower(), guess);

            ShowWeight(toolStripTextBoxTrueSymbol.Text.ToLower());
            SaveSamples();
            ToAnswer(answers.Trained);

            Clear();
            //int sumOfFonts;
            //InstalledFontCollection fonts = new InstalledFontCollection();
            //sumOfFonts = fonts.Families.Length;
            //toolStripStatusLabel2.Text = sumOfFonts.ToString();

            
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
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Sampler.DeleteSamplesFolder(pathOfSamples);
        }
        private void toolStripSave_Click(object sender, EventArgs e)
        {
            neuronWeb.SaveWeights();
        }
        private void toolStripLocalWeights_Click(object sender, EventArgs e)
        {
            neuronWeb.GetLocalWeights();
            ToAnswer(answers.LocalWeight);
        }
        //private methods
        private void MyInitialize()
        {
            iterationOfSampleGroup = 0;
            Sampler.CreateSamplesFolder(pathOfSamples);
            neuronWeb.CreateWeightsFolder();

            inputPicture = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            weightPicture = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            matchesPicture = new Bitmap(pictureBox2.Width, pictureBox2.Height);

            for (int x = 0; x < pictureBox1.Width; x++)
                for (int y = 0; y < pictureBox1.Height; y++)
                {
                    inputPicture.SetPixel(x, y, Color.White);
                    weightPicture.SetPixel(x, y, Color.White);
                    matchesPicture.SetPixel(x, y, Color.White);
                }
        

            pictureBox1.Image = inputPicture;
            pictureBox2.Image = weightPicture;
            graphics = Graphics.FromImage(inputPicture);

            pictureBox1.Size = inputPicture.Size;
            pictureBox2.Size = inputPicture.Size;

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
            string pathOfFolder = AppDomain.CurrentDomain.BaseDirectory + "\\weight";

            neuronWeb.Neurons = new Neuron[numberOfRussianLetters];

            //Создаём имя для каждого нейрона
            neuronWeb.NamingNeurons();

            //Создаём путь для дальнейшего сохранения веса
            neuronWeb.MakePathForEveryone(pathOfFolder);

            //Задаём разрешение 
            neuronWeb.SetResolutionForEveryone();
        }
        private void ToAnswer(answers answerOfNeuronWeb)
        {
            switch (answerOfNeuronWeb)
            {
                case answers.CannotGuess:
                    toolStripStatusLabel1.Text = "ИИ: не могу угадать букву. Напишите правильную букву и нажмите обучить";
                    guess = string.Empty;
                    toolStripStatusLabel1.BackColor = Color.DeepPink;
                    toolStripTextBoxTrueSymbol.Focus();
                    break;

                case answers.Guess:
                    toolStripStatusLabel1.Text = "ИИ: я думаю это буква" + " " + guess + ".";
                    if (guess == string.Empty)
                        throw new Exception("guess is empty string");

                    toolStripStatusLabel1.BackColor = Color.DeepSkyBlue;
                    toolStripTextBoxTrueSymbol.Focus();
                    break;

                case answers.DrawSymbol:
                    toolStripStatusLabel1.Text = "ИИ: нарисуйте букву.";
                    guess = string.Empty;
                    toolStripStatusLabel1.BackColor = Color.OrangeRed;
                    break;

                case answers.WriteTrueSymbol:
                    toolStripStatusLabel1.Text = "ИИ: напишите правильную букву, чтобы обучить.";
                    guess = string.Empty;
                    toolStripStatusLabel1.BackColor = Color.OrangeRed;
                    break;

                case answers.Trained:
                    toolStripStatusLabel1.Text = "ИИ: понял свои ошибки. Продолжайте рисовать.";
                    guess = string.Empty;
                    toolStripTextBoxTrueSymbol.Text = string.Empty;

                    toolStripStatusLabel1.BackColor = Color.Orange;
                    break;

                case answers.PictureClean:
                    toolStripStatusLabel1.Text = "ИИ: холст очищен. Начинайте рисовать.";
                    guess = string.Empty;
                    toolStripTextBoxTrueSymbol.Text = string.Empty;

                    toolStripStatusLabel1.BackColor = Color.Green;
                    break;

                case answers.PutRussianLayout:
                    toolStripStatusLabel1.Text = "ИИ: поставьте русскую раскладку.";
                    guess = string.Empty;
                    toolStripTextBoxTrueSymbol.Text = string.Empty;

                    toolStripStatusLabel1.BackColor = Color.OrangeRed;
                    break;

                case answers.LocalWeight:
                    toolStripStatusLabel1.Text = "ИИ: вы начали с обученного ИИ.";
                    guess = string.Empty;
                    toolStripTextBoxTrueSymbol.Text = string.Empty;

                    toolStripStatusLabel1.BackColor = Color.Green;
                    break;
            }
        }
        private void Clear()
        {

            graphics.Clear(Color.White);
            pictureBox1.Image = inputPicture;
            
            
        }
        private void ShowWeight(string symbol)
        {
            weightPicture = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            Graphics g = Graphics.FromImage(weightPicture);
            RectangleF rectangle = new RectangleF(0f, 0f, pictureBox2.Width, pictureBox2.Height);
            Bitmap weightInBMP = Sampler.ArrayToBMP(neuronWeb.FindNeuron(symbol).weight);
            g.DrawImage(weightInBMP, rectangle);
            pictureBox2.Image = weightPicture;

            label2.Text = "Веса буквы";
        }
        private void ShowMatches(string symbol)
        {
            matchesPicture = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            Graphics g = Graphics.FromImage(matchesPicture);
            RectangleF rectangle = new RectangleF(0f, 0f, pictureBox2.Width, pictureBox2.Height);
            Bitmap matchesInBMP = Sampler.ArrayToBMP(neuronWeb.FindNeuron(symbol).matches);
            g.DrawImage(matchesInBMP, rectangle);
            pictureBox2.Image = matchesPicture;

            label2.Text = "Совпадающие пиксели";
        }

        private void SaveSamples()
        {
            if (guess != string.Empty)
            {
                Neuron tempNeuron = neuronWeb.FindNeuron(guess);
                Sampler.SaveImage(pathOfSamples + iterationOfSampleGroup + Sampler.matchesSuffix, tempNeuron.matches);                
            }
            Sampler.SaveImage(pathOfSamples + iterationOfSampleGroup + Sampler.inputSuffix, inputPicture);
            Sampler.SaveImage(pathOfSamples + iterationOfSampleGroup + Sampler.weightSuffix, weightPicture);
            iterationOfSampleGroup++;
        }
    }
}

