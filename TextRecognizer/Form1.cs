using System;
using System.Drawing;
using System.Windows.Forms;

namespace TextRecognizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MyInitialize();
            StartNeuronWeb();
            toolStripComboScale.SelectedIndex = 0;
        }
        private int iterationOfSampleGroup;
        private int startX, startY, endX, endY;
        private double scaleX;
        private double scaleY;
        private Bitmap inputPicture;
        private Bitmap weightPicture;
        private Bitmap matchesPicture;
        private Graphics graphics;
        private Pen pen = new Pen(Color.Black, 30f);
        private Perceptron perceptron = new Perceptron();
        private string guess = string.Empty;

        private enum Answers
        {
            CannotGuess,
            Guess,
            DrawSymbol,
            WriteTrueSymbol,
            Trained,
            PictureClean,
            PutRussianLayout,
            LocalWeight
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
            guess = perceptron.Recognize(inputPicture);            

            if (guess == string.Empty)
            {
                ToAnswer(Answers.CannotGuess);
                SaveSamples();
                return;
            }

            ToAnswer(Answers.Guess);

            ShowMatches(guess);
            SaveSamples();
            
        }
        private void toolStripClean_Click(object sender, EventArgs e)
        {
            Clear();
            ToAnswer(Answers.PictureClean);
        }
        private void toolStripTrain_Click(object sender, EventArgs e)
        {
            if (toolStripTextBoxTrueSymbol.Text == string.Empty)
            {
                ToAnswer(Answers.WriteTrueSymbol);
                return;
            }
            if (Array.FindIndex(perceptron.Neurons, x => x.name == toolStripTextBoxTrueSymbol.Text.ToLower()) < 0)
            {
                ToAnswer(Answers.PutRussianLayout);
                return;
            }

            perceptron.SetInput(inputPicture);
            perceptron.Train(toolStripTextBoxTrueSymbol.Text.ToLower(), guess);

            ShowWeight(toolStripTextBoxTrueSymbol.Text.ToLower());
            SaveSamples();
            ToAnswer(Answers.Trained);

            Clear();            
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
            }
        }
        //methods
        private void MyInitialize()
        {
            iterationOfSampleGroup = 0;
            Sampler.CreateSamplesFolder();

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
            perceptron.NamingNeurons();
            perceptron.SetResolutionForEveryone();
        }
        private void ToAnswer(Answers answerOfNeuronWeb)
        {
            switch (answerOfNeuronWeb)
            {
                case Answers.CannotGuess:
                    toolStripStatusLabel1.Text = "ИИ: не могу угадать букву. Напишите правильную букву и нажмите обучить";
                    guess = string.Empty;
                    toolStripStatusLabel1.BackColor = Color.DeepPink;
                    toolStripTextBoxTrueSymbol.Focus();
                    break;

                case Answers.Guess:
                    toolStripStatusLabel1.Text = "ИИ: я думаю это буква" + " " + guess + ".";
                    if (guess == string.Empty)
                        throw new Exception("guess is empty string");

                    toolStripStatusLabel1.BackColor = Color.DeepSkyBlue;
                    toolStripTextBoxTrueSymbol.Focus();
                    break;

                case Answers.DrawSymbol:
                    toolStripStatusLabel1.Text = "ИИ: нарисуйте букву.";
                    guess = string.Empty;
                    toolStripStatusLabel1.BackColor = Color.OrangeRed;
                    break;

                case Answers.WriteTrueSymbol:
                    toolStripStatusLabel1.Text = "ИИ: напишите правильную букву, чтобы обучить.";
                    guess = string.Empty;
                    toolStripStatusLabel1.BackColor = Color.OrangeRed;
                    break;

                case Answers.Trained:
                    toolStripStatusLabel1.Text = "ИИ: понял свои ошибки. Продолжайте рисовать.";
                    guess = string.Empty;

                    toolStripStatusLabel1.BackColor = Color.Orange;
                    break;

                case Answers.PictureClean:
                    toolStripStatusLabel1.Text = "ИИ: холст очищен. Начинайте рисовать.";
                    guess = string.Empty;
                    toolStripTextBoxTrueSymbol.Text = string.Empty;

                    toolStripStatusLabel1.BackColor = Color.Green;
                    break;

                case Answers.PutRussianLayout:
                    toolStripStatusLabel1.Text = "ИИ: поставьте русскую раскладку.";
                    guess = string.Empty;
                    toolStripTextBoxTrueSymbol.Text = string.Empty;

                    toolStripStatusLabel1.BackColor = Color.OrangeRed;
                    break;

                case Answers.LocalWeight:
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
            Bitmap weightInBMP = Sampler.ArrayToBMP(perceptron.FindNeuron(symbol).weights);
            g.DrawImage(weightInBMP, rectangle);
            pictureBox2.Image = weightPicture;

            label2.Text = "Веса буквы";
        }
        private void ShowMatches(string symbol)
        {
            matchesPicture = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            Graphics g = Graphics.FromImage(matchesPicture);
            RectangleF rectangle = new RectangleF(0f, 0f, pictureBox2.Width, pictureBox2.Height);
            Bitmap matchesInBMP = Sampler.ArrayToBMP(perceptron.FindNeuron(symbol).matches);
            g.DrawImage(matchesInBMP, rectangle);
            pictureBox2.Image = matchesPicture;

            label2.Text = "Совпадающие пиксели";
        }

        private void SaveSamples()
        {
            if (guess != string.Empty)
            {
                Neuron tempNeuron = perceptron.FindNeuron(guess);
                Sampler.SaveImage(iterationOfSampleGroup + Sampler.matchesSuffix, tempNeuron.matches);                
            }
            Sampler.SaveImage(iterationOfSampleGroup + Sampler.inputSuffix, inputPicture);
            Sampler.SaveImage(iterationOfSampleGroup + Sampler.weightSuffix, weightPicture);
            iterationOfSampleGroup++;
        }
    }
}

