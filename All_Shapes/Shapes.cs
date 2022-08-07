using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace All_Shapes
{
    [Serializable]
    public abstract class Shape
    {
        protected int x;
        protected int y;
        protected int x_mouse;
        protected int y_mouse;
        protected static uint radius;
        protected static Color C;
        protected bool moving;
        protected bool drawnline;

        public abstract void draw(Graphics G);

        static Shape()
        {
            radius = 25;
            C = Color.Red;
        }
        public Shape(int x, int y)
        {
            this.x = x;
            this.y = y;
            x_mouse = 0;
            y_mouse = 0;
            moving = false;
            drawnline = false;
        }
        public static Color PointColor { get { return C; } set { C = value; } }
        public static uint Radius { get { return radius; } set { radius = value; } }
        public abstract int X { get; set; }
        public abstract int Y { get; set; }
        public abstract int X_mouse { get; set; }
        public abstract int Y_mouse { get; set; }
        public abstract bool Moving { get; set; }
        public abstract bool IsInside(int x, int y);
        public abstract bool Drawline { get; set; }
    }

    [Serializable]
    public class Circle : Shape
    {
        public Circle(int x, int y) : base(x, y) { }

        public override void draw(Graphics G)
        {
            SolidBrush brush = new SolidBrush(C);
            G.FillEllipse(brush, x - radius, y - radius, radius * 2, radius * 2);
        }
        public override bool IsInside(int x, int y)     // стоит ли мышка в фигуре?
        {
            return (x - this.x) * (x - this.x) + (y - this.y) * (y - this.y) <= radius * radius;
        }

        public override int X { get { return x; } set { x = value; } }
        public override int Y { get { return y; } set { y = value; } }
        public override int X_mouse { get { return x_mouse; } set { x_mouse = value; } }
        public override int Y_mouse { get { return y_mouse; } set { y_mouse = value; } }
        public override bool Moving { get { return moving; } set { moving = value; } }
        public override bool Drawline { get { return drawnline; } set { drawnline = value; } }
    }

    [Serializable]
    public class Square : Shape
    {
        public Square(int x, int y) : base(x, y) { }

        public override void draw(Graphics G)
        {
            SolidBrush brush = new SolidBrush(C);

            G.FillRectangle(brush, new Rectangle(x - (int)(radius / (Math.Sqrt(2) / 2) / 2), y - (int)(radius / (Math.Sqrt(2) / 2) / 2), (int)((radius) / (Math.Sqrt(2) / 2)), (int)(radius / (Math.Sqrt(2) / 2))));
        }
        public override bool IsInside(int x, int y)     // стоит ли мышка в фигуре?
        {
            return x <= this.x + (int)(radius * Math.Sqrt(2) / 2) && y <= this.y + (int)(radius * Math.Sqrt(2) / 2) && x >= this.x - (int)(radius * Math.Sqrt(2) / 2) && y >= this.y - (int)(radius * Math.Sqrt(2) / 2);
        }

        public override int X { get { return x; } set { x = value; } }
        public override int Y { get { return y; } set { y = value; } }
        public override int X_mouse { get { return x_mouse; } set { x_mouse = value; } }
        public override int Y_mouse { get { return y_mouse; } set { y_mouse = value; } }
        public override bool Moving { get { return moving; } set { moving = value; } }
        public override bool Drawline { get { return drawnline; } set { drawnline = value; } }
    }

    [Serializable]
    public class Triangle : Square
    {
        public Point[] points;
        public Triangle(int x, int y) : base(x, y)
        {
            points = new Point[3];
            points[0] = new Point((int)(x - radius * (Math.Sqrt(3) / 2)), (int)(y + radius / 2));
            points[1] = new Point(x, (int)(y - radius));
            points[2] = new Point((int)(x + radius * (Math.Sqrt(3) / 2)), (int)(y + radius / 2));
        }



        public override void draw(Graphics G)
        {
            SolidBrush brush = new SolidBrush(C);
            points[0] = new Point((int)(x - radius * (Math.Sqrt(3) / 2)), (int)(y + radius / 2));
            points[1] = new Point(x, (int)(y - radius));
            points[2] = new Point((int)(x + radius * (Math.Sqrt(3) / 2)), (int)(y + radius / 2));
            G.FillPolygon(brush, points);
        }
        public override bool IsInside(int x, int y)     // стоит ли мышка в фигуре?
        {
            int a = (points[0].X - x) * (points[1].Y - points[0].Y) - (points[1].X - points[0].X) * (points[0].Y - y);
            int b = (points[1].X - x) * (points[2].Y - points[1].Y) - (points[2].X - points[1].X) * (points[1].Y - y);
            int c = (points[2].X - x) * (points[0].Y - points[2].Y) - (points[0].X - points[2].X) * (points[2].Y - y);
            if ((a >= 0 && b >= 0 && c >= 0) || (a <= 0 && b <= 0 && c <= 0)) return true; else return false;
        }

        public override int X
        {
            get { return x; }
            set
            {
                x = value;
                points[0] = new Point((int)(x - radius * (Math.Sqrt(3) / 2)), (int)(y + radius / 2));
                points[1] = new Point(x, (int)(y - radius));
                points[2] = new Point((int)(x + radius * (Math.Sqrt(3) / 2)), (int)(y + radius / 2));
            }
        }
        public override int Y
        {
            get { return y; }
            set
            {
                y = value;
                points[0] = new Point((int)(x - radius * (Math.Sqrt(3) / 2)), (int)(y + radius / 2));
                points[1] = new Point(x, (int)(y - radius));
                points[2] = new Point((int)(x + radius * (Math.Sqrt(3) / 2)), (int)(y + radius / 2));
            }
        }
        public override int X_mouse { get { return x_mouse; } set { x_mouse = value; } }
        public override int Y_mouse { get { return y_mouse; } set { y_mouse = value; } }
        public override bool Moving { get { return moving; } set { moving = value; } }
        public override bool Drawline { get { return drawnline; } set { drawnline = value; } }
    }
}
