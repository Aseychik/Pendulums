using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManySpringsMatModel
{
    internal class FoucaultPendulum
    {
    }

    public class MDXYFunc : MDFunction
    {
        int pos;

        public MDXYFunc(int pos) { this.pos = pos; }

        public double g(double t, double[] x0, double[]? dx = null, double mn = 1)
        {
            if (dx == null) return x0[pos + 2];
            else return x0[pos + 2] + dx[pos + 2] / mn;
        }
    }

    public class MDVXFunction : MDFunction {
        public double w;
        public double L;
        public double gP;

        public MDVXFunction(double w = 0.04, double L = 100, double gP = 9.81)
        {
            this.w = w;
            this.L = L;
            this.gP = gP;
        }

        public double g(double t, double[] x0, double[]? dx = null, double mn = 1)
        {
            return 2 * x0[3] * w + w * w * x0[0] - gP * x0[0] / L + (dx == null ? 0 : 2 * dx[3] * w / mn + w * w * dx[0] / mn - gP * dx[0] / L / mn);
        }
    }

    public class MDVYFunction : MDFunction
    {
        public double w;
        public double L;
        public double gP;

        public MDVYFunction(double w = 0.04, double L = 100, double gP = 9.81)
        {
            this.w = w;
            this.L = L;
            this.gP = gP;
        }

        public double g(double t, double[] x0, double[]? dx = null, double mn = 1)
        {
            return -2 * x0[2] * w + w * w * x0[1] - gP * x0[1] / L + (dx == null ? 0 : (-2 * dx[2] * w + w * w * dx[1] - gP * dx[1] / L) / mn);
        }
    }

    public class FoucautVisualization : Visualisation
    {
        public double x;
        public double y;
        public int w;
        public int h;
        public Bitmap image;
        public Color bgColor;
        public Color lineColor;
        private Pen pen;
        public Graphics grImage;
        public double[]? lastPos;
        public SolidBrush cBrush;

        public FoucautVisualization(double x, double y, double[]? initialPos = null, Color? bgColor = null, Color? lineColor = null, int w = 200, int h = 200)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.bgColor = bgColor ?? Color.White;
            this.lineColor = lineColor ?? Color.Red;
            this.pen = new Pen(this.lineColor);
            lastPos = initialPos;
            image = new Bitmap(w, h);
            cBrush = new SolidBrush(Color.LightGray);
            grImage = Graphics.FromImage(image);
            grImage.Clear(this.bgColor);
            grImage.FillEllipse(cBrush, 0, 0, w, h);


        }

        public double[] GetPos() { return [x, y]; }

        public void Restart()
        {
            image.Dispose();
            grImage.Dispose();
            image = new Bitmap(w, h);
            grImage = Graphics.FromImage(image);
            grImage.Clear(bgColor);
            grImage.FillEllipse(cBrush, 0, 0, w, h);
            lastPos = null;
        }

        public void Draw(Graphics g, double[] pos, double c, bool flag)
        {
            if (lastPos == null)
            {
                lastPos = pos[0..2];
                return;
            }
            grImage.DrawLine(pen, (float)(lastPos[0] * c + w / 2), (float)(lastPos[1] * c + h / 2), (float)(pos[0] * c + w / 2), (float)(pos[1] * c + h / 2));
            lastPos = pos[0..2];
            g.DrawImage(image, (float)x, (float)y);
        }
    }
}
