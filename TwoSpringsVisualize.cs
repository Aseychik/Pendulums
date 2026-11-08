using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManySpringsMatModel
{
    internal class TwoSpringsVisualize : Visualisation
    {
        BoxVisualisation wall1;
        BoxVisualisation wall2;
        BoxVisualisation box1;
        BoxVisualisation box2;
        Spring spring1;
        Spring spring2;
        Spring spring3;
        double x;
        double y;

        public TwoSpringsVisualize(double x, double y)
        {
            this.x = x;
            this.y = y;
            wall1 = new BoxVisualisation(this.x, this.y, 20, 100);
            box1 = new BoxVisualisation(this.x + 150, this.y + 10, 60, 80);
            spring1 = new Spring(wall1, box1);

            box2 = new BoxVisualisation(this.x + 300, this.y + 10, 60, 80);
            spring2 = new Spring(box1, box2);
            wall2 = new BoxVisualisation(this.x + 450, this.y, 20, 100);
            spring3 = new Spring(box2, wall2);
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
            box1.x = pos[0] * c;
            box1.Draw(g);
            box2.x = pos[1] * c;
            box2.Draw(g);
            spring1.Draw(g, flag);
            spring2.Draw(g, flag);
            spring3.Draw(g, flag);
            wall2.Draw(g);
        }
    }
}
