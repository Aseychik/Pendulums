using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManySpringsMatModel
{
    internal class NSpringsVisualize : Visualisation
    {
        BoxVisualisation wall1;
        BoxVisualisation wall2;
        BoxVisualisation[] boxes;
        int distance;
        int boxWidth;
        int wallWidth;
        Spring[] springs;
        double x;
        double y;
        int n;

        public NSpringsVisualize(int n, double x, double y, int d = 150, int w = 60, int wallWidth = 20)
        {
            this.n = n;
            this.x = x;
            this.y = y;
            this.distance = d;
            this.boxWidth = w;
            this.wallWidth = wallWidth;
            
            wall1 = new BoxVisualisation(this.x, this.y - 10, wallWidth, 100);
            boxes = new BoxVisualisation[n];
            for (int i = 0; i < n; i++)
            {
                boxes[i] = new BoxVisualisation(x + distance * (i + 1) + boxWidth * i, y, boxWidth, 80);
            }
            wall2 = new BoxVisualisation(x + distance * (n + 1) + boxWidth * n, y, wallWidth, 80);
            springs = new Spring[n + 1];
            springs[0] = new Spring(wall1, boxes[0], initialLength: distance);
            for (int i = 1; i < springs.Length - 1; i++)
                springs[i] = new Spring(boxes[i - 1], boxes[i], initialLength: distance);

            springs[n] = new Spring(boxes[n - 1], wall2, initialLength: distance);
        }

        public double[] GetPos()
        {
            return [x, y];
        }

        public void Restart()
        { }

        public void Draw(Graphics g, double[] pos, double c, bool flag)
        {
            wall1.Draw(g);
            for (int i = 0; i < n; i++) {
                boxes[i].x = pos[i] * c;
                boxes[i].Draw(g);
            }
            for (int i = 0; i < springs.Length; i++)
                springs[i].Draw(g, flag);
            
            wall2.Draw(g);
        }
    }
}
