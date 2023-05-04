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

        private Pen pen = new Pen(Color.Black, 15f);

        private NeuronWeb neuronWeb = new NeuronWeb();

        private void SetSize()
         {
            Rectangle rectangle = Screen.PrimaryScreen.Bounds;
            picture = new Bitmap(rectangle.Width, rectangle.Height);
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
            string guess = neuronWeb.Recognize((Bitmap)pictureBox1.Image);

            
            label3.Text = guess;

            textBox1.Text = guess;
            textBox1.Focus();
        }
    }


}
