using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Bitmap bitmap = new Bitmap(100, 100);

        private void button1_Click(object sender, EventArgs e)
        {
            
            bitmap.Save("A:\\Andrey\\fdf\\textffffffff.bmp");
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox1.Image = bitmap;

        }
    }
}
