using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManySpringsMatModel
{
    public interface Visualisation
    {
        public double[] GetPos();

        public void Draw(Graphics g, double[] pos, double c, bool flag);

        public void Restart();
    }
}
