using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trk4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MakeObjects();
            this.ClientSize = new Size(2000, 1000);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
        public Rectangle rect1BoundingBox;
        public List<Rectangle> rect2BoundingBoxes;

        

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            // Risanje prvega omejevalnega pravokotnika in črte
            g.DrawRectangle(Pens.Black, rect1BoundingBox);
            g.DrawLine(Pens.Black, 100, 100, 500, 500);

            // Risanje druge črte
            g.DrawLine(Pens.Black, rect2BoundingBoxes[0].Left + rect2BoundingBoxes[0].Width / 2, rect2BoundingBoxes[0].Top + rect2BoundingBoxes[0].Height / 2,
                                    rect2BoundingBoxes[9].Left + rect2BoundingBoxes[9].Width / 2, rect2BoundingBoxes[9].Top + rect2BoundingBoxes[9].Height / 2);

            // Risanje manjših kvadratkov vzdolž druge črte
            foreach (var rect in rect2BoundingBoxes)
            {
                g.DrawRectangle(Pens.Blue, rect);
            }
        }

        public void MakeObjects()
        {
            // Prvi omejevalni pravokotnik
            rect1BoundingBox = new Rectangle(100, 100, 400, 400);

            // Začetna točka kvadratkov
            Point startPoint = new Point(800, 200);

            // Velikost kvadratka
            int squareSize = 40; // Velikost posameznega kvadratka

            // Ustvari kvadratke vzdolž črte
            rect2BoundingBoxes = new List<Rectangle>();
            for (int i = 0; i < 10; i++)
            {
                int x = startPoint.X + i * squareSize;
                int y = startPoint.Y + i * squareSize;
                rect2BoundingBoxes.Add(new Rectangle(x, y, squareSize, squareSize));
            }
        }
    }
}
