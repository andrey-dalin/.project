﻿using System;
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
        public ArrayPoints(int size) 
        {
            points = new Point[size];
        }

        public void SetPoint(int x, int y)
        {
            if (index >= points.Length)
            {
                index = 0;
            }
                points[index] = new Point(x, y);
                index++;
            
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
