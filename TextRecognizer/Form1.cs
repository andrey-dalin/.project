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
        }
        private int iterationOfSampleGroup;
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
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                graphics = Graphics.FromImage(inputPicture);
                graphics.DrawLine(pen, e.X - 1, e.Y - 1, e.X, e.Y);
                pictureBox1.Image = inputPicture;
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

