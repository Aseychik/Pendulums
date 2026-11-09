using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManySpringsMatModel
{
    public class MDOneSpringFuncX : MDFunction
    {
        public double g(double t, double[] x, double[]? dx = null, double mn = 1)
        {
            if (dx == null)
                return x[1];
            else
                return x[1] + dx[1] / mn;
        }
    }

    public class MDOneSpringFuncV : MDFunction
    {
        private double w;

        public MDOneSpringFuncV(double w)
        {
            this.w = w;
        }

        public double g(double t, double[] x, double[]? dx = null, double mn = 1)
        {
            if (dx == null)
                return -w * w * x[0];
            else
                return -w * w * (x[0] + dx[0] / mn);
        }
    }
}
