using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManySpringsMatModel
{
    public class MDNSpringFuncX : MDFunction
    {
        private int pos;
        private int n;

        public MDNSpringFuncX(int n, int pos)
        {
            this.pos = pos;
            this.n = n;
        }

        public double g(double t, double[] x, double[]? dx = null, double c = 1)
        {
            if (dx == null)
                return x[pos + n];
            else
                return x[pos + n] + dx[pos + n] / c;
        }

    }

    public class MDNSpringFuncVLeft : MDFunction
    {
        private double k;
        private double kg;

        public MDNSpringFuncVLeft(double k = 0.5, double kg = 0.5)
        {
            this.k = k;
            this.kg = kg;
        }

        public double g(double t, double[] x, double[]? dx = null, double c = 1)
        {
            if (dx == null)
                return k * (x[1] - x[0]) - kg * x[0];
            else
                return k * (x[1] - x[0]) - kg * x[0] + (k * (dx[1] - dx[0]) - kg * dx[0]) / c;
        }
    }

    public class MDNSpringFuncVRight : MDFunction
    {
        private int n;
        private double k;
        private double kg;

        public MDNSpringFuncVRight(int n, double k = 0.5, double kg = 0.5)
        {
            this.n = n;
            this.k = k;
            this.kg = kg;
        }

        public double g(double t, double[] x, double[]? dx = null, double c = 1)
        {
            if (dx == null)
                return k * (x[n - 2] - x[n - 1]) - kg * x[n - 1];
            else
                return k * (x[n - 2] - x[n - 1]) - kg * x[n - 1] + (k * (dx[n - 2] - dx[n - 1]) - kg * dx[n - 1]) / c;
        }
    }

    public class MDNSpringFuncV : MDFunction
    {
        private int pos;
        private double k;
        private double kg;
        private int[] p = [1, 0];

        public MDNSpringFuncV(int pos, double k = 0.5, double kg = 0.5)
        {
            this.pos = pos;
            this.k = k;
            this.kg = kg;
        }

        public double g(double t, double[] x, double[]? dx = null, double c = 1)
        {
            if (dx == null)
                return k * (x[pos + 1] - 2 * x[pos] + x[pos - 1]);
            else
                return k * (x[pos + 1] - 2 * x[pos] + x[pos - 1]) + k * (dx[pos + 1] - 2 * dx[pos] + dx[pos - 1]) / c;
        }
    }
}

