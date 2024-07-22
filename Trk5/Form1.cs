using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics; // Add a reference to System.Numerics.Vectors

namespace Trk5
{
    public partial class Form1 : Form
    {
        

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
        private MyRectangle rect1;
        private MyRectangle rect2;


        public Form1()
        {
            InitializeComponent();

            this.Paint += new PaintEventHandler(this.Form1_Paint);
            this.Size = new Size(800, 600);

            // Ustvari prve pravokotnike
            Random rand = new Random();
            rect1 = new MyRectangle(new Vector2(rand.Next(50, 700), rand.Next(50, 500)), 100, 50, (float)rand.NextDouble() * (float)Math.PI);
            rect2 = new MyRectangle(new Vector2(rand.Next(50, 700), rand.Next(50, 500)), 150, 75, (float)rand.NextDouble() * (float)Math.PI);

            // Gumb za preverjanje trkov
            Button checkCollisionButton = new Button();
            checkCollisionButton.Text = "Preveri trk";
            checkCollisionButton.Location = new Point(10, 10);
            checkCollisionButton.Click += new EventHandler(this.CheckCollisionButton_Click);
            this.Controls.Add(checkCollisionButton);

            // Oznaka za rezultat preverjanja trkov
            Label collisionLabel = new Label();
            collisionLabel.Name = "collisionLabel";
            collisionLabel.Text = "";
            collisionLabel.Location = new Point(10, 50);
            collisionLabel.AutoSize = true;
            this.Controls.Add(collisionLabel);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawRectangle(e.Graphics, rect1, Brushes.Blue);
            DrawRectangle(e.Graphics, rect2, Brushes.Red);
        }

        private void DrawRectangle(Graphics g, MyRectangle rect, Brush brush)
        {
            var vertices = rect.GetVertices().Select(v => new PointF(v.X, v.Y)).ToArray();
            g.FillPolygon(brush, vertices);
        }

        private void CheckCollisionButton_Click(object sender, EventArgs e)
        {
            bool collision = MyRectangle.AreRectanglesColliding(rect1, rect2);

            Label collisionLabel = this.Controls.Find("collisionLabel", true).FirstOrDefault() as Label;
            if (collisionLabel != null)
            {
                collisionLabel.Text = $"Trk: {collision}";
            }

            // Generiraj nove pravokotnike za naslednje preverjanje trkov
            Random rand = new Random();
            rect1 = new MyRectangle(new Vector2(rand.Next(50, 700), rand.Next(50, 500)), 100, 50, (float)rand.NextDouble() * (float)Math.PI);
            rect2 = new MyRectangle(new Vector2(rand.Next(50, 700), rand.Next(50, 500)), 150, 75, (float)rand.NextDouble() * (float)Math.PI);

            this.Invalidate();
        }

        public class MyRectangle
        {
            public Vector2 Center;
            public float Width;
            public float Height;
            public float Rotation;

            public MyRectangle(Vector2 center, float width, float height, float rotation)
            {
                Center = center;
                Width = width;
                Height = height;
                Rotation = rotation;
            }

            public Vector2[] GetVertices()
            {
                float cos = (float)Math.Cos(Rotation);
                float sin = (float)Math.Sin(Rotation);
                float halfWidth = Width / 2;
                float halfHeight = Height / 2;

                return new Vector2[]
                {
                    new Vector2(Center.X + halfWidth * cos - halfHeight * sin, Center.Y + halfWidth * sin + halfHeight * cos),
                    new Vector2(Center.X - halfWidth * cos - halfHeight * sin, Center.Y - halfWidth * sin + halfHeight * cos),
                    new Vector2(Center.X - halfWidth * cos + halfHeight * sin, Center.Y - halfWidth * sin - halfHeight * cos),
                    new Vector2(Center.X + halfWidth * cos + halfHeight * sin, Center.Y + halfWidth * sin - halfHeight * cos)
                };
            }


            // pogledamo vse osi in jih damo v isti seznam ter preverimo prekrivanje
            public static bool AreRectanglesColliding(MyRectangle rect1, MyRectangle rect2)
            {
                Vector2[] axes = GetAxes(rect1).Concat(GetAxes(rect2)).ToArray();
                foreach (var axis in axes)
                {
                    if (!IsProjectionOverlap(rect1, rect2, axis))
                    {
                        return false;
                    }
                }
                return true;
            }

            private static Vector2[] GetAxes(MyRectangle rect)
            {
                Vector2[] vertices = rect.GetVertices();
                return new Vector2[]
                {
                    Vector2.Normalize(vertices[1] - vertices[0]),
                    Vector2.Normalize(vertices[3] - vertices[0])
                };
            }

            private static bool IsProjectionOverlap(MyRectangle rect1, MyRectangle rect2, Vector2 axis)
            {
                var (min1, max1) = Project(rect1, axis);
                var (min2, max2) = Project(rect2, axis);

                return max1 >= min2 && max2 >= min1;
            }

            private static (float min, float max) Project(MyRectangle rect, Vector2 axis)
            {
                Vector2[] vertices = rect.GetVertices();
                float min = Vector2.Dot(vertices[0], axis);
                float max = min;
                for (int i = 1; i < vertices.Length; i++)
                {
                    float projection = Vector2.Dot(vertices[i], axis);
                    if (projection < min) min = projection;
                    if (projection > max) max = projection;
                }
                return (min, max);
            }
        }
    }
}
