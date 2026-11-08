using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManySpringsMatModel
{
    internal class NSpringsModel : Model
    {
        public int n;
        public double t0;
        public double dt;
        public int seconds = 5;
        public int timerPeriod = 100;
        public int speedValue = 1;
        public int currentPos = 0;
        public double[] now;
        public double[] arr0;
        public double tNow;
        public double[][] xArr;
        public double[][] tempXArr;
        public bool endEvaluation = true;
        public int CountEval = 0;
        private AutoResetEvent? waitHandler;
        public MDFunction[] funcs;
        public NSpringsVisualize visualize;

        public void SetPeriod(int seconds, int timerPeriod, int speedValue)
        {
            this.seconds = seconds;
            this.timerPeriod = timerPeriod;
            this.speedValue = speedValue;
            dt = timerPeriod / 1000.0;
        }

        public void SetWaitHandler(AutoResetEvent waitHandler)
        {
            this.waitHandler = waitHandler;
        }

        public NSpringsModel(int n, double[]? now = null, NSpringsVisualize? visualize = null, double w = 1, double dt = 0.1)
        {
            this.n = n;
            this.visualize = visualize ?? new NSpringsVisualize(n, 10, 100);
            if (now == null)
            {
                now = new double[n * 2];
                for (int  i = 0; i < now.Length; i++)
                    now[i] = 0;
            }
            arr0 = now;
            
            this.now = new double[arr0.Length];
            for (int i = 0; i < arr0.Length; i++)
                this.now[i] = arr0[i];
            funcs = new MDFunction[arr0.Length];
            funcs[0] = new MDNSpringFuncX(n, 0);
            funcs[n - 1] = new MDNSpringFuncX(n, n - 1);
            funcs[n] = new MDNSpringFuncVLeft();
            funcs[2 * n - 1] = new MDNSpringFuncVRight(n);

            for (int i = 1; i < n - 1; i++)
            {
                funcs[i] = new MDNSpringFuncX(n, i);
                funcs[i + n] = new MDNSpringFuncV(i);
            }
            
            this.dt = dt;
            t0 = 0;
            tNow = t0;

            xArr = new double[2][];
            tempXArr = new double[2][];
        }

        public Visualisation GetVisualisation()
        {
            return visualize;
        }

        public void Draw(Graphics g, double[] pos, double c, bool flag)
        {
            visualize.Draw(g, pos, c, flag);
        }

        public void Restart(double[] newX0, double[] newV0, double newT0, double newDt)
        {
            arr0 = new double[newX0.Length * 2];
            for (int i = 0; i < newX0.Length; i++)
            {
                arr0[i] = newX0[i];
                arr0[i + newX0.Length] = newV0[i];
            }
            t0 = newT0;
            dt = newDt;
            Restart();
        }

        public double[] GetArr0()
        {
            return arr0;
        }

        public void Restart(double[] newX0, double? newT0 = null, double? newDt = null)
        {
            arr0 = new double[newX0.Length];
            for (int i = 0; i < newX0.Length; i++)
                arr0[i] = newX0[i];
            
            t0 = newT0 ?? t0;
            dt = newDt ?? dt;
            Restart();
        }

        public void Restart()
        {
            tNow = t0;
            now = new double[arr0.Length];
            for (int i = 0; i < arr0.Length; i++)
                now[i] = arr0[i];
            xArr = new double[arr0.Length][];
            tempXArr = new double[arr0.Length][];
            endEvaluation = true;
        }


        public double[][] EvaluateNArr(int n)
        {
            endEvaluation = false;
            tempXArr = RungeKutt.EvaluateNArr(n, now, dt, tNow, funcs);
            endEvaluation = true;
            return tempXArr;
        }

        public void Evaluate()
        {
            EvaluateNArr(CountEval);
        }


        // вычисление сразу на 5 секунд, в теории стоит асинхронно начинать рассчитывать новый массив и менять их местами, когда первый заканчивается
        // Значит, нужно вычислить 5 * fps * speedValue значений

        public double[]? GetNext()
        {
            int pos = NextPose(currentPos);
            if (pos < 0) return null;
            currentPos = pos;

            double[] result = new double[arr0.Length];
            for (int i = 0; i < arr0.Length; i++)
                result[i] = xArr[i][pos];
            return result;
        }

        public int NextPose(int pos)
        {
            // элементы в массиве закончились, значит нужно свапнуть и запустить вычисление следующих
            if (xArr[0] == null || pos + speedValue >= xArr[0].Length)
            {
                // для изначальной инициализации
                if (xArr[0] == null || xArr[0].Length == 0)
                {
                    int count = (int)(seconds * (1000 / (double)(timerPeriod)) * speedValue);
                    EvaluateNArr(count);
                    xArr = tempXArr;

                    CountEval = count;

                    waitHandler?.Set();

                    return 0;
                }
                // для вычисления следующих при завершении текущего массива
                else
                {
                    if (!endEvaluation) return pos;

                    xArr = tempXArr;

                    int count = (int)(seconds * (1000 / (double)(timerPeriod)) * speedValue);

                    CountEval = count;
                    waitHandler?.Set();

                    return 0;
                }
            }
            return pos + speedValue;
        }


        private class MDNSpringFuncX : MDFunction
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

        private class MDNSpringFuncVLeft : MDFunction
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

        private class MDNSpringFuncVRight : MDFunction
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

        private class MDNSpringFuncV : MDFunction
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
}

