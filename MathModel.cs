using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManySpringsMatModel
{
    internal class MathModel : Model
    {
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
        public Visualisation visualize;

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

        public MathModel(double[] now, MDFunction[] funcs, Visualisation visualize, double w = 1, double dt = 0.1)
        {
            this.funcs = funcs;

            
            this.visualize = visualize;


            this.now = new double[now.Length];
            this.arr0 = new double[now.Length];
            for (int i = 0; i < now.Length; i++)
            {
                this.now[i] = now[i];
                this.arr0[i] = now[i];
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
    }
}