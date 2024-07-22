using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace Trk7
{
    public partial class Form1 : Form
    {
        private List<Vector2> polygon1;
        private List<Vector2> polygon2;
        private Random rand;

        public Form1()
        {
            InitializeComponent();

            this.Paint += new PaintEventHandler(this.Form1_Paint);
            this.Size = new Size(800, 600);

            rand = new Random();

            // Ustvari naključne konveksne poligone z 8 ogljišči
            GenerateRandomConvexPolygons();

            // Gumb za preverjanje trkov
            Button checkCollisionButton = new Button();
            checkCollisionButton.Text = "Preveri trk";
            checkCollisionButton.Location = new Point(10, 10);
            checkCollisionButton.Click += new EventHandler(this.CheckCollisionButton_Click);
            this.Controls.Add(checkCollisionButton);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Nariši poligone
            DrawPolygon(e.Graphics, polygon1, Brushes.Blue);
            DrawPolygon(e.Graphics, polygon2, Brushes.Red);
        }

        private void DrawPolygon(Graphics g, List<Vector2> polygon, Brush brush)
        {
            // Nariši poligon
            var points = polygon.Select(v => new PointF(v.X, v.Y)).ToArray();
            g.FillPolygon(brush, points);
        }

        private void CheckCollisionButton_Click(object sender, EventArgs e)
        {
            // Preveri trk med poligoni
            bool collision = ArePolygonsColliding(polygon1, polygon2);

            // Prikaži rezultat preverjanja trkov
            MessageBox.Show($"Trk: {collision}");

            // Ustvari nove naključne konveksne poligone na naključnih lokacijah znotraj forme
            GenerateRandomConvexPolygons();

            // Ponovno nariši
            this.Invalidate();
        }

        private void GenerateRandomConvexPolygons()
        {
            // Določi velikost območja za naključno lokacijo poligonov (80% velikosti forme)
            int areaWidth = (int)(this.ClientSize.Width * 0.8);
            int areaHeight = (int)(this.ClientSize.Height * 0.8);

            // Določi naključne lokacije za središča poligonov
            int centerX1 = rand.Next(areaWidth) + (int)(this.ClientSize.Width * 0.1);
            int centerY1 = rand.Next(areaHeight) + (int)(this.ClientSize.Height * 0.1);
            int centerX2 = rand.Next(areaWidth) + (int)(this.ClientSize.Width * 0.1);
            int centerY2 = rand.Next(areaHeight) + (int)(this.ClientSize.Height * 0.1);

            // Ustvari prvi konveksen poligon z naključnimi ogljišči okoli določene lokacije
            polygon1 = GenerateRandomConvexPolygon(centerX1, centerY1, 200, 250);

            // Ustvari drugi konveksen poligon z naključnimi ogljišči okoli druge določene lokacije
            polygon2 = GenerateRandomConvexPolygon(centerX2, centerY2, 200, 250);
        }

        private List<Vector2> GenerateRandomConvexPolygon(int centerX, int centerY, int width, int height)
        {
            // Generiraj naključne točke okoli določene lokacije
            List<Vector2> points = new List<Vector2>();
            for (int i = 0; i < 8; i++)
            {
                // Generiraj naključne koordinate znotraj pravokotnika okoli določene lokacije
                int x = rand.Next(centerX - width / 2, centerX + width / 2);
                int y = rand.Next(centerY - height / 2, centerY + height / 2);

                points.Add(new Vector2(x, y));
            }

            // Izberi konveksno ovojnico z Graham Scan
            List<Vector2> convexHull = GrahamScan(points);

            return convexHull;
        }

        private List<Vector2> GrahamScan(List<Vector2> points)
        {
            // Poišči začetno točko
            Vector2 startPoint = points.OrderBy(p => p.Y).ThenBy(p => p.X).First();

            // Uredi točke po kotu glede na začetno točko
            List<Vector2> sortedPoints = points.OrderBy(p => Math.Atan2(p.Y - startPoint.Y, p.X - startPoint.X)).ToList();

            // Ustvari zunanjo konveksno ovojnico
            Stack<Vector2> convexHull = new Stack<Vector2>();
            convexHull.Push(sortedPoints[0]);
            convexHull.Push(sortedPoints[1]);

            for (int i = 2; i < sortedPoints.Count; i++)
            {
                Vector2 top = convexHull.Pop();
                while (Orientation(convexHull.Peek(), top, sortedPoints[i]) != 2)
                {
                    top = convexHull.Pop();
                }
                convexHull.Push(top);
                convexHull.Push(sortedPoints[i]);
            }

            return convexHull.ToList();
        }

        private int Orientation(Vector2 p, Vector2 q, Vector2 r)
        {
            float val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
            if (val == 0)
            {
                return 0;
            }
            return (val > 0) ? 1 : 2; // desni zavoj, 2->levi zavoj
        }

        private bool ArePolygonsColliding(List<Vector2> polygon1, List<Vector2> polygon2)
        {
            // Poišči vse možne osi projekcije
            List<Vector2> axes = GetAxes(polygon1);
            axes.AddRange(GetAxes(polygon2));

            // Preveri trke po vseh osih
            foreach (Vector2 axis in axes)
            {
                if (!IsOverlapOnAxis(polygon1, polygon2, axis))
                {
                    return false;
                }
            }

            return true;
        }

        private List<Vector2> GetAxes(List<Vector2> polygon)
        {
            List<Vector2> axes = new List<Vector2>();

            for (int i = 0; i < polygon.Count; i++)
            {
                Vector2 p1 = polygon[i];
                Vector2 p2 = polygon[(i + 1) % polygon.Count];
                // Izračunaj vektor normalen na rob
                Vector2 edge = p2 - p1;
                Vector2 normal = new Vector2(-edge.Y, edge.X);
                normal = Vector2.Normalize(normal);

                axes.Add(normal);
            }

            return axes;
        }

        private bool IsOverlapOnAxis(List<Vector2> polygon1, List<Vector2> polygon2, Vector2 axis)
        {
            // Projektiraj poligone na os in poišči prekrivanje
            (float min1, float max1) = ProjectPolygon(polygon1, axis);
            (float min2, float max2) = ProjectPolygon(polygon2, axis);

            // Preveri, če se projekcije prekrivajo
            return max1 >= min2 && max2 >= min1;
        }

        private (float min, float max) ProjectPolygon(List<Vector2> polygon, Vector2 axis)
        {
            float min = Vector2.Dot(polygon[0], axis);
            float max = min;

            for (int i = 1; i < polygon.Count; i++)
            {
                float dotProduct = Vector2.Dot(polygon[i], axis);
                if (dotProduct < min)
                {
                    min = dotProduct;
                }
                else if (dotProduct > max)
                {
                    max = dotProduct;
                }
            }

            return (min, max);



        }
    }
}
