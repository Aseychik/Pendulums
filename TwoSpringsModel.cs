using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManySpringsMatModel
{
    public class MDTwoSpringFuncX : MDFunction
    {
        private int pos;

        public MDTwoSpringFuncX(int pos)
        { this.pos = pos; }

        public double g(double t, double[] x, double[]? dx = null, double c = 1)
        {
            if (dx == null)
                return x[pos + 2];
            else
                return x[pos + 2] + dx[pos + 2] / c;
        }

    }

    public class MDTwoSpringFuncV : MDFunction
    {
        private int pos;
        private double k;
        private double kg;
        private int[] p = [1, 0];

        public MDTwoSpringFuncV(int pos, double k = 0.5, double kg = 0.5)
        {
            this.pos = pos;
            this.k = k;
            this.kg = kg;
        }

        public double g(double t, double[] x, double[]? dx = null, double c = 1)
        {
            if (dx == null)
                return k * (x[p[pos]] - x[pos]) - kg * x[pos];
            else
                return k * (x[p[pos]] - x[pos]) - kg * x[pos] + (k * (dx[p[pos]] - dx[pos]) - kg * dx[pos]) / c;
        }
    }
}
