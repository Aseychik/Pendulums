using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManySpringsMatModel
{
    internal class RungeKutt
    {
        public static double EvaluateOne(double xNow, double dt, double tNow, DFunction g) { 
            double k1 = dt * g.g(tNow, xNow);
            double k2 = dt * g.g(tNow + dt / 2, xNow + k1 / 2);
            double k3 = dt * g.g(tNow + dt / 2, xNow + k2 / 2);
            double k4 = dt * g.g(tNow + dt, xNow + k3);

            return (k1 + 2 * k2 + 2 * k3 + k4) / 6;
        }

        public static void EvaluateOneForArr(double[] xNow, double dt, double tNow, MDFunction[] gArr)
        {
            int k = xNow.Length;
            double[] k1 = new double[k];
            for (int i = 0; i < k; i++)
                k1[i] = dt * gArr[i].g(tNow, xNow);
 
                double[] k2 = new double[k];
            for (int i = 0; i < k; i++)
                k2[i] = dt * gArr[i].g(tNow + dt / 2, xNow, k1, 2);
            
            double[] k3 = new double[k];
            for (int i = 0; i < k; i++)
                k3[i] = dt * gArr[i].g(tNow + dt / 2, xNow, k2, 2);
            
            double[] k4 = new double[k];
            for (int i = 0; i < k; i++)
                k4[i] = dt * gArr[i].g(tNow + dt, xNow, k3);
            
            for (int i = 0; i < k; i++)
                xNow[i] += (k1[i] + 2 * k2[i] + 2 * k3[i] + k4[i]) / 6;
        }

        public static double[][] EvaluateNArr(int n, double[] now, double dt, double tNow, MDFunction[] funcs)
        {
            int k = now.Length;
            double[][] tempXArr = new double[k][];

            for (int i = 0; i < k; i++)
            {
                tempXArr[i] = new double[n];
                tempXArr[i][0] = now[i];
            }

            for (int i = 1; i < n; i++)
            {
                RungeKutt.EvaluateOneForArr(now, dt, tNow, funcs);
                for (int j = 0; j < k; j++)
                    tempXArr[j][i] = now[j];
                tNow += dt;
            }

            return tempXArr;
        }
    }

    internal interface DFunction
    {
        public double g(double t, double x);
    }

    internal interface MDFunction
    {
        public double g(double t, double[] x0, double[]? dx = null, double mn = 1);
    }
}
