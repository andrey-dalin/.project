using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRecognizer
{
    public class ArrayPoints
    {
        
        private int index = 0;

        private Point[] points;
        public ArrayPoints(int width, int height) 
        {
            points = new Point[width * height];
        }

        public void SetPoint(int x, int y)
        {
            if (index < points.Length)
            {
                points[index] = new Point(x, y);
                index++;
            }

            index = 0;
        }

        public void ResetPoints()
        {
            index = 0;
        }

        public int GetCountPoints()
        {
            return index;
        }

        public Point[] GetPoints()
        {
            return points;
        }
    }
}
