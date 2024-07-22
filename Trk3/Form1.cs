using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trk3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeObjects();
            MakeObject();
        }
        private Random random = new Random();
        private GraphicsPath randomShapePath;
        private Brush brush;
        

        public Point[] tocke;

        public Rectangle omejevalniPravokotnik;
        public int hitrostLikaX = 4;


        private void timer_Tick(object sender, EventArgs e)
        {
            
        }


        public void MakeObject()
        {
            // Create a random shape path
            int numPoints = random.Next(8, 12); // Random number of points (between 8 and 12)
            tocke = new Point[numPoints];

            for (int i = 0; i < numPoints; i++)
            {
                tocke[i] = new Point(random.Next(ClientSize.Width - 500), random.Next(ClientSize.Height - 250));
            }

            randomShapePath = new GraphicsPath();
            randomShapePath.AddPolygon(tocke);

            // Calculate bounding box
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;
            foreach (var point in tocke)
            {
                minX = Math.Min(minX, point.X);
                minY = Math.Min(minY, point.Y);
                maxX = Math.Max(maxX, point.X);
                maxY = Math.Max(maxY, point.Y);
            }
            omejevalniPravokotnik = new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            // Fill the random shape with the brush
            if (randomShapePath != null)
            {
                g.FillPath(brush, randomShapePath);
                g.DrawRectangle(Pens.Black, omejevalniPravokotnik);
            }

        }
        private void InitializeObjects()
        {
            brush = new SolidBrush(Color.Red); // Izberi poljubno barvo
        }
        private void UpdateGraphicsPath()
        {
            // Posodobi randomShapePath z novimi točkami
            randomShapePath = new GraphicsPath();
            randomShapePath.AddPolygon(tocke);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < tocke.Length; i++)
            {
                tocke[i].X += hitrostLikaX;
            }
            omejevalniPravokotnik.X += hitrostLikaX;

            if (omejevalniPravokotnik.Left <= 0 || omejevalniPravokotnik.Right >= ClientSize.Width)
            {
                hitrostLikaX *= -1;
            }


            UpdateGraphicsPath();
            Invalidate();

        }
    }
}
