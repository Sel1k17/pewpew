using System;
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
        Pen p1 = new Pen(Color.Black);
        Pen p2 = new Pen(Color.Gray);
        List<Shape> Shapes = new List<Shape>();
        Point ShapeStart;
        bool IsShapeStart = true;
        string curFile;
        Shape TempShape;
        Pen p3 = new Pen(Color.Red);
       
        public MainScreen()
        {
            InitializeComponent();
            progressBar1.Value = 0;
         
        }
        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            IsShapeStart = true;
            TempShape = null;
             
        }

        private void MainScreen_MouseMove(object sender, MouseEventArgs e)
        {
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

           
      
 
            if (progressBar1.Value==100)  label2.Visible = true;
           
            if (rb_Cross.Checked)
            {  progressBar1.Increment(1); 
                AddShape(TempShape);
               
            }
            if (rb_Line.Checked)
            {
                if (IsShapeStart) {ShapeStart = e.Location; progressBar1.Increment(1); }
                else AddShape(TempShape);
                IsShapeStart = !IsShapeStart;
             
            }
            if (rb_circle.Checked)
            {
                if (IsShapeStart) { ShapeStart = e.Location; progressBar1.Increment(1); }
                else AddShape(TempShape);
                IsShapeStart = !IsShapeStart;
          
            }
            this.Refresh();
            label3.Text = "Количество грязи: " + Convert.ToString(progressBar1.Value);
           
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
            Shapes_List.Items.Add(s.ConfString);
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

        public void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Increment(-1);
         
           
            while (Shapes_List.SelectedIndices.Count > 0)
            {
                Shapes.RemoveAt(Shapes_List.SelectedIndices[0]);
                Shapes_List.Items.RemoveAt(Shapes_List.SelectedIndices[0]);
            }
            Pen p3 = new Pen(Color.Red);
            button1.Enabled = false;
            TempShape = null;
            this.Refresh();
        }

        private void Shapes_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

      public void button2_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
   MainScreen.ActiveForm.BackColor= colorDialog1.Color;
     
      }

      private void button3_Click(object sender, EventArgs e)
      {
          MainScreen.ActiveForm.BackColor=MainScreen.DefaultBackColor;
         
      }

      private void button4_Click(object sender, EventArgs e)
      {


          Shapes.Clear();
            Shapes_List.Items.Clear();
            button1.Enabled = false;
            TempShape = null;
            this.Refresh();
            progressBar1.Value = 0;
            label3.Text = "Количество грязи: " + Convert.ToString(progressBar1.Value);


      }

    
    }
}
