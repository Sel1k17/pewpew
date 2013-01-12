﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PEWPEW
{
    public partial class MainScreen : Form
    {
        List<Shape> Shapes = new List<Shape>();
        Point ShapeStart;
        bool IsShapeStart = true;
        string curFile;
        //Pen p;
        Pen p1 = new Pen(Color.Black);
        Pen p2 = new Pen(Color.Black);
        Shape TempShape;
        //Pen p3 = new Pen(Color.Red,2);
        public MainScreen()
        {
            InitializeComponent();
        }
        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            IsShapeStart = true;
        }
        private void MainScreen_MouseMove(object sender, MouseEventArgs e)
        {
            //this.Text = Convert.ToString(e.X) + " . " + (e.Y);
            //this.Text = Convert.ToString(e.Location);
            if (rb_Cross.Checked) TempShape = new Cross(e.X,e.Y);
            else if (rb_Line.Checked)
            {
                if (!IsShapeStart)
                {
                    TempShape = new Line(ShapeStart, e.Location);
                }
            }
            else if (rb_circle.Checked)
            {
                if (!IsShapeStart)
                {
                    TempShape = new Circle(ShapeStart, e.Location);
                }
            }
            this.Refresh();
        }
        private void MainScreen_Paint(object sender, PaintEventArgs e)
        {
            if (TempShape != null)
            {
                TempShape.DrawWith(e.Graphics, p2);
            }
            foreach (Shape p in Shapes)
            {
                p.DrawWith(e.Graphics,p1);
            }
        }
        private void MainScreen_MouseDown(object sender, MouseEventArgs e)
        {
            if (rb_Cross.Checked)
            {
                AddShape(TempShape);
            }
            if (rb_Line.Checked)
            {
                if (IsShapeStart) ShapeStart = e.Location;
                else AddShape(TempShape);
                IsShapeStart = !IsShapeStart;
            }
            if (rb_circle.Checked)
            {
                if (IsShapeStart) ShapeStart = e.Location;
                else AddShape(TempShape);
                IsShapeStart = !IsShapeStart;
            }
            this.Refresh();
        }
        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                curFile = saveFileDialog1.FileName;
                StreamWriter sw = new StreamWriter(curFile);
                foreach (Shape p in this.Shapes)
                {
                    p.SaveTo(sw);
                }
                sw.Close();
            }
        }
        private void AddShape(Shape s)
        {
            Shapes.Add(s);
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                curFile = openFileDialog1.FileName;
                Shapes.Clear();
                StreamReader sr = new StreamReader(curFile);
                while (!sr.EndOfStream)
                {
                    string type = sr.ReadLine();
                    switch (type)
                    {
                        case "Cross":
                            {
                                AddShape(new Cross(sr));
                                break;
                            }
                        case "Line":
                            {
                                AddShape(new Line(sr));
                                break;
                            }
                        case "Circle":
                            {
                                AddShape(new Circle(sr));
                                break;
                            }
                    }
                }
                sr.Close();
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
