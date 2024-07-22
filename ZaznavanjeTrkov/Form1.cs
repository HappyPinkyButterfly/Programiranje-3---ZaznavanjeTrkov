using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZaznavanjeTrkov
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MakeObject();
            this.ClientSize = new Size(2000, 1000);
        }
        public Rectangle rect1;
        public Rectangle rect2;

        public Rectangle rect1BoundingBox;
        public Rectangle rect2BoundingBox;

        public int rect1VelocityX = 5;
        public int rect2VelocityX = -5;
        




        public void MakeObject()
        {
            rect1 = new Rectangle(100, 350, 300, 150);
            rect2 = new Rectangle(1100, 350, 300, 150);

            rect1BoundingBox = new Rectangle(rect1.X - 5, rect1.Y - 5, rect1.Width + 10, rect1.Height + 10);
            rect2BoundingBox = new Rectangle(rect2.X - 5, rect2.Y - 5, rect2.Width + 10, rect2.Height + 10);


        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;


            g.FillRectangle(Brushes.Red, rect1);
            g.FillRectangle(Brushes.Blue, rect2);

            g.DrawRectangle(Pens.Black, rect1BoundingBox);
            g.DrawRectangle(Pens.Black, rect2BoundingBox);



        }

        private void timer_Tick(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            rect1.X += rect1VelocityX;
            rect1BoundingBox.X = rect1.X - 5;

            rect2.X += rect2VelocityX;
            rect2BoundingBox.X = rect2.X - 5;


            if (rect1BoundingBox.IntersectsWith(rect2BoundingBox))
            {
                rect1VelocityX *= -1;

                rect2VelocityX *= -1;
            }

            if (rect1.Left <= 0 || rect1.Right >= ClientSize.Width)
            {
                rect1VelocityX *= -1;
            }

            if (rect2.Left <= 0 || rect2.Right >= ClientSize.Width)
            {
                rect2VelocityX *= -1;
            }

            //Redraw the form
            Invalidate();

        }
    }
}
