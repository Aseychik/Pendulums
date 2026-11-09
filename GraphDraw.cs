using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManySpringsMatModel
{
    internal class GraphDraw
    {
        public int paramCount;
        public double[][] points;
        public int width;
        public int height;
        public int pos;
        public Pen[] linePens;
        public Pen axisPen;
        public int dx;
        public double min;
        public double max;
        public String[] axisNames;
        public int numDist;
        public int x;
        public int y;
        public int fill;
        public Font font;
        public SolidBrush brush;
        private SolidBrush[] brushes;
        private Pen zeroPen;
        public int numCount;

        public GraphDraw(int x, int y, int width, int height, int pos = 0, int dx = 1, int paramCount = 1, double[][]? points = null, Pen[]? linePen = null, string[]? axisNames = null, int? numCount = null, int numDist = 15)
        {
            if (points == null)
                fill = 0;
            else fill = points.Length - 1;
            this.x = x;
            this.y = y;
            this.axisPen = new Pen(Color.Black);
            this.zeroPen = new Pen(Color.Gray);

            this.linePens = new Pen[paramCount];
            brushes = new SolidBrush[paramCount];
            for (int i = 0; i < paramCount; i++)
            {
                var c = HsvToRgb(360.0 * i / paramCount, 0.75, 0.85);
                this.linePens[i] = new Pen(c);
                brushes[i] = new SolidBrush(c);
            }
            
            if (linePen != null) this.linePens = linePen;

            this.width = width;
            if (dx < 1) dx = 1;
            if (dx >= width) dx = width;
            this.dx = dx;
            this.height = height;
            this.pos = pos;
            this.paramCount = paramCount;
            this.points = points ?? new double[this.paramCount][];
            for (int i = 0; i < this.paramCount; i++) 
                this.points[i] = new double[(int)(width / dx)];

            if (this.pos >= this.points[0].Length) this.pos = this.points[0].Length - 1;
            this.axisNames = axisNames ?? ["x", "t"];
            if (numDist < 1) numDist = 1;
            this.numDist = numDist;
            font = new Font("Arial", 8);
            brush = new SolidBrush(Color.Black);

            this.numCount = numCount ?? height / 35;
        }

        public void Reset(int x, int y, int width, int height, int pos = 0, int dx = 1, int paramCount = 1)
        {
            fill = 0;
            this.x = x;
            this.y = y;
            
            foreach (var pen in linePens)
                pen.Dispose();

            foreach (var brush in brushes)
                brush.Dispose();

            linePens = new Pen[paramCount];
            brushes = new SolidBrush[paramCount];
            for (int i = 0; i < paramCount; i++)
            {
                var c = HsvToRgb(360.0 * i / paramCount, 0.75, 0.85);
                linePens[i] = new Pen(c);
                brushes[i] = new SolidBrush(c);
            }

            this.width = width;
            if (dx < 1) dx = 1;
            if (dx >= width) dx = width;
            this.dx = dx;
            this.height = height;
            this.pos = pos;
            this.paramCount = paramCount;
            this.points = new double[this.paramCount][];
            for (int i = 0; i < this.paramCount; i++)
                this.points[i] = new double[(int)(width / dx)];

            this.numCount = height / 35;
        }

        public static Color HsvToRgb(double h, double s, double v)
        {
            h = ((h % 360) + 360) % 360 / 60.0;
            double c = v * s, x = c * (1 - Math.Abs(h % 2 - 1)), m = v - c;
            var (r, g, b) = (int)h switch
            {
                0 => (c, x, 0.0),
                1 => (x, c, 0.0),
                2 => (0.0, c, x),
                3 => (0.0, x, c),
                4 => (x, 0.0, c),
                _ => (c, 0.0, x)
            };
            return Color.FromArgb((int)((r + m) * 255), (int)((g + m) * 255), (int)((b + m) * 255));
        }

        public void AddPoint(double[] x, int shift = 0)
        {
            if (fill < points[0].Length) fill++;
            if (fill == 1)
            {
                min = x[shift];
                max = x[shift];
            }
            for (int i = 0; i < paramCount; i++)
            {
                double t = x[i + shift];
                points[i][pos] = t;
                min = t < min ? t : min;
                max = t > max ? t : max;
            }
            pos = (pos + 1) % points[0].Length;
        }

        public void Restart()
        {
            fill = 0;
            this.points = new double[this.paramCount][];
            for (int i = 0; i < this.paramCount; i++)
                this.points[i] = new double[(int)(width / dx)];

            pos = 0;
        }

        public void Draw(Graphics g, bool drawNumbers = true) 
        {
            if (fill > 2)
            {
                int pos = 0;
                if (fill >= points[0].Length) pos = this.pos;
                double[] last = new double[paramCount], current = new double[paramCount];
                for (int i = 0; i < paramCount; i++)
                    last[i] = points[i][pos];

                if (max == min) return;
                double c = height * 18 / 20 / (max - min);
                if (c < 1e-6) c = 1e-6;

                if (drawNumbers)
                {
                    float dnum = (float)((max - min) / numCount);

                    float zeroY = (float)(y + height / 20 + c * max);
                    if (zeroY > y + height / 20 && zeroY < y + height * 19 / 20)
                    {
                        g.DrawLine(zeroPen, x, zeroY, x + width, zeroY);
                        g.DrawString("0", font, brush, x - 10, zeroY - 7);
                    }

                    if (numCount > 0)
                    {
                        double yn = dnum;
                        while (yn < max)
                        {
                            if (yn > min)
                                g.DrawString(string.Format("{0:N2}", yn), font, brush, x - 35, (float)(y + height / 20 + c * (max - yn) - 7));
                            yn += dnum;
                        }
                        yn = -dnum;
                        while (yn > min)
                        {
                            if (yn < max)
                                g.DrawString(string.Format("{0:N2}", yn), font, brush, x - 35, (float)(y + height / 20 + c * (max - yn) - 7));
                            yn -= dnum;
                        }
                    }
                }

                for (int i = 1, t = (pos + 1) % points[0].Length; i < fill; i++, t = (t + 1) % points[0].Length) {
                    for (int j = 0; j < paramCount; j++)
                    {
                        current[j] = points[j][t];

                        g.DrawLine(linePens[j], x + (i - 1) * dx, (float)(y + height / 20 + c * (max - last[j])), x + i * dx, (float)(y + height / 20 + c * (max - current[j])));
                        last[j] = current[j];
                    }
                }
            }
            g.DrawLine(axisPen, x, y + height, x + width, y + height);
            g.DrawLine(axisPen, x, y, x, y + height);
            g.DrawString(axisNames[0], font, brush, x - 4, y - 20);
            g.DrawString(axisNames[1], font, brush, x + width + 8, y + height - 4);


            int legendW = Math.Min(width / paramCount, 60);
            for (int i = 0; i < paramCount; i++)
            {
                g.DrawString((i + 1).ToString(), font, brush, x + (legendW * i) + 3, y + height + 10);
                g.FillRectangle(brushes[i], (float)(x + (legendW * i) + legendW / 2), y + height + 10, legendW / 2, legendW / 2);
            }
        }

    }
}
