using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManySpringsMatModel
{
    internal class OneSpringVisualize : Visualisation
    {
        BoxVisualisation wall;
        BoxVisualisation box;
        Spring spring;
        double x;
        double y;

        public OneSpringVisualize(double x, double y)
        {
            this.x = x;
            this.y = y;
            wall = new BoxVisualisation(this.x, this.y, 20, 100);
            box = new BoxVisualisation(this.x + 200, this.y + 10, 60, 80);
            spring = new Spring(wall, box);
        }

        public double[] GetPos()
        {
            return [x, y];
        }

        public void Restart()
        { }

        public void Draw(Graphics g, double[] pos, double c, bool flag) 
        {
            wall.Draw(g);
            box.x = pos[0] * c;
            box.Draw(g);
            spring.Draw(g, flag);
        }
    }

    internal class Spring
    {
        SolidBrush brush;
        BoxVisualisation leftBox;
        BoxVisualisation rightBox;
        float height;
        float initialLength;
        int segmentCount = 10;
        int preWidth = 20;
        float wh = 4;
        Pen pen;

        public Spring(BoxVisualisation left, BoxVisualisation right, float height = 20, float initialLength = 150, SolidBrush? brush = null) 
        { 
            leftBox = left;
            rightBox = right;
            this.brush = brush ?? new SolidBrush(Color.Gray);
            pen = new Pen(Color.Gray, wh);
            this.initialLength = initialLength;
            preWidth = (int)(initialLength / 10);
            this.height = height;
        }

        public void Draw(Graphics g, bool springsAsRect) 
        {
            float x = leftBox.GetDrawX() + (float)leftBox.width;
            float dx = rightBox.GetDrawX() - x;

            if (dx < 1) dx = 1;

            float mn = (initialLength / dx);
            if (mn < 0.01) mn = 0.01f;
            if (mn > 2) mn = 2;

            float nHeight = height * mn;
            if (springsAsRect)
            {
                g.FillRectangle(brush, x, leftBox.GetDrawY() + (float)leftBox.height / 2 - nHeight / 2, dx, nHeight);
                return;
            }
            

            if (dx < preWidth * 2)
            {
                g.FillRectangle(brush, x, leftBox.GetDrawY() + (float)leftBox.height / 2 - wh / 2, dx, wh);
                g.FillRectangle(brush, x + preWidth / 3, leftBox.GetDrawY() + (float)leftBox.height / 2 - nHeight / 2, dx - preWidth * 2 / 3, nHeight);
            }
            else
            {
                g.DrawLine(pen, x, leftBox.GetDrawY() + (float)leftBox.height / 2 - wh / 2, x + preWidth, leftBox.GetDrawY() + (float)leftBox.height / 2 - wh / 2);
                g.DrawLine(pen, rightBox.GetDrawX() - preWidth, leftBox.GetDrawY() + (float)leftBox.height / 2 - wh / 2, rightBox.GetDrawX(), leftBox.GetDrawY() + (float)leftBox.height / 2 - wh / 2);

                float len = dx - preWidth * 2;
                float dl = len / segmentCount;

                float ySr = leftBox.GetDrawY() + (float)leftBox.height / 2 - wh / 2;

                float lastx = x + preWidth - wh / 2, lasty = ySr, nextx, nexty;
                float dhy = nHeight / 2;
                int c = 1;
                for (int i = 1; i < segmentCount; i++)
                {
                    nextx = x + preWidth + dl * i;
                    nexty = ySr + dhy * c;
                    c *= -1;
                    g.DrawLine(pen, lastx, lasty, nextx, nexty);
                    lastx = nextx;
                    lasty = nexty;
                }
                g.DrawLine(pen, lastx, lasty, rightBox.GetDrawX() - preWidth + wh / 2, ySr);
            }
        }
    }

    internal class BoxVisualisation
    {
        internal double x;
        internal double y;
        internal double width;
        internal double height;

        internal double screenX;
        internal double screenY;
        String name;
        SolidBrush brushBox;
        Font font;
        SolidBrush brush;

        public BoxVisualisation(double screenX, double screenY, double width, double height, SolidBrush? boxBrush = null, String? name = null, Font? font = null, SolidBrush? brush = null) 
        { 
            this.screenX = screenX;
            this.screenY = screenY;
            this.width = width;
            this.height = height;
            name ??= "box";
            this.name = name;
            this.brushBox = brushBox ?? new SolidBrush(Color.Black);
            font ??= new Font("Arial", 16);
            this.font = font;
            brush ??= new SolidBrush(Color.Black);
            this.brush = brush;
        }

        public float GetDrawX()
        {
            return (float)(screenX + x);
        }

        public float GetDrawY()
        {
            return (float)(screenY + y);
        }

        public void Draw(Graphics g, bool drawName = false) 
        {
            g.FillRectangle(brushBox, GetDrawX(), GetDrawY(), (float)width, (float)height);
            if (drawName)
                g.DrawString(name, font, brush, GetDrawX() - ((float)width / 2), GetDrawY() - 20);
        }
    }
}
