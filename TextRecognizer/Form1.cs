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
            InitializeComponent();
        }

        private bool IsMouseDown;

        private ArrayPoints arrayPoints = new ArrayPoints(NeuronWeb.ResolutionX, NeuronWeb.ResolutionY);

        private Bitmap picture = new Bitmap(NeuronWeb.ResolutionX, NeuronWeb.ResolutionY);

        private Graphics graphics;

        private Pen pen = new Pen(Color.Black, 3f);

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
                IsMouseDown = true;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
                IsMouseDown= false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDown == false) return;

            arrayPoints.SetPoint(e.X, e.Y);

            if (arrayPoints.GetCountPoints() >= 2)
            {
                graphics.DrawLines(pen, arrayPoints.GetPoints());

                pictureBox1.Image = picture;
            }
        }
    }


}
