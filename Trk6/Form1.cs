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

namespace Trk6
{
    public partial class Form1 : Form
    {
        
        private Circle circle1;
        private Circle circle2;
        public class Circle
        {
            public Vector2 Center;    // Središče kroga
            public float Radius;      // Polmer kroga

            public Circle(Vector2 center, float radius)
            {
                Center = center;
                Radius = radius;
            }

            public static bool AreCirclesColliding(Circle circle1, Circle circle2)
            {
                // Preveri trke med krogi
                float distance = Vector2.Distance(circle1.Center, circle2.Center);
                return distance <= circle1.Radius + circle2.Radius;
            }
        }
        public Form1()
        {
            InitializeComponent();

            this.Paint += new PaintEventHandler(this.Form1_Paint);
            this.Size = new Size(800, 600);


            Random rand = new Random();
            circle1 = new Circle(new Vector2(rand.Next(50, 700), rand.Next(50, 500)), 40);
            circle2 = new Circle(new Vector2(rand.Next(50, 700), rand.Next(50, 500)), 60);

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
            // Nariši kroge
            DrawCircle(e.Graphics, circle1, Brushes.Green);
            DrawCircle(e.Graphics, circle2, Brushes.Yellow);
        }

        private void DrawCircle(Graphics g, Circle circle, Brush brush)
        {
            // Nariši krog
            var rect = new RectangleF(circle.Center.X - circle.Radius, circle.Center.Y - circle.Radius, 2 * circle.Radius, 2 * circle.Radius);
            g.FillEllipse(brush, rect);
        }

        private void CheckCollisionButton_Click(object sender, EventArgs e)
        {
            // Preveri trke med krogi
            bool collision = Circle.AreCirclesColliding(circle1, circle2);

            // Prikaži rezultat preverjanja trkov
            Label collisionLabel = this.Controls.Find("collisionLabel", true).FirstOrDefault() as Label;
            if (collisionLabel != null)
            {
                collisionLabel.Text = $"Trk: {collision}";
            }

            // Generiraj nove kroge za naslednje preverjanje trkov
            Random rand = new Random();
            circle1 = new Circle(new Vector2(rand.Next(50, 700), rand.Next(50, 500)), 40);
            circle2 = new Circle(new Vector2(rand.Next(50, 700), rand.Next(50, 500)), 60);

            this.Invalidate();
        }

        // Metoda za inicializacijo oblike
        private void InitializeForm(object sender, EventArgs e)
        {
            // Tukaj lahko dodamo kodo za inicializacijo oblike
        }
    }
}
