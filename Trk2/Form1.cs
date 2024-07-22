using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trk2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MakeObject();
            this.ClientSize = new Size(2000,1000);
        }
        public Point[] pentagonPoints;
        public Rectangle pentagonPointsBoundingBox;

        public Rectangle rect1BoundingBox;

        public int pentagonPointsVelocityX = 4;
        public int rect1VelocityX = -5;







        public void MakeObject()
        {
            pentagonPoints = new Point[5];
            double angle = Math.PI / 2.5; // Angle between each vertex
            double startAngle = Math.PI / 10; // Starting angle
            int radius = 100; // Distance from center to each vertex
            for (int i = 0; i < 5; i++)
            {
                int x = (int)(150 + radius * Math.Cos(startAngle + i * angle));
                int y = (int)(150 - radius * Math.Sin(startAngle + i * angle)) + 200;
                pentagonPoints[i] = new Point(x, y);
            }
            // Calculate bounding box
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;
            foreach (var point in pentagonPoints)
            {
                minX = Math.Min(minX, point.X);
                minY = Math.Min(minY, point.Y);
                maxX = Math.Max(maxX, point.X);
                maxY = Math.Max(maxY, point.Y);
            }
            pentagonPointsBoundingBox = new Rectangle(minX , minY , maxX - minX, maxY - minY);


            rect1BoundingBox = new Rectangle(1100, 350, 300, 150);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.FillPolygon(Brushes.Red, pentagonPoints);
            g.DrawRectangle(Pens.Black, pentagonPointsBoundingBox);

            g.DrawRectangle(Pens.Black, rect1BoundingBox);// naša omejevalna škatla za krog
            g.DrawEllipse(Pens.Black, rect1BoundingBox);  // z to škatlo narišemo krog 
            g.FillEllipse(Brushes.Blue, rect1BoundingBox);
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            
        }

        private void timer_Tick_1(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                pentagonPoints[i].X += pentagonPointsVelocityX;
            }
            pentagonPointsBoundingBox.X += pentagonPointsVelocityX;

            rect1BoundingBox.X += rect1VelocityX;

            //////////////////////////////////////////////////////////////////////

            if (rect1BoundingBox.IntersectsWith(pentagonPointsBoundingBox))
            {
                rect1VelocityX *= -1;

                pentagonPointsVelocityX *= -1;
            }

            if (rect1BoundingBox.Left <= 0 || rect1BoundingBox.Right >= ClientSize.Width)
            {
                rect1VelocityX *= -1;
            }

            if (pentagonPointsBoundingBox.Left <= 0 || pentagonPointsBoundingBox.Right >= ClientSize.Width)
            {
                pentagonPointsVelocityX *= -1;
            }

            // Redraw the form
            Invalidate();

        }
    }
}
