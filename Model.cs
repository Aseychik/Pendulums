using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManySpringsMatModel
{
    public interface Model
    {
        public void Evaluate();

        public double[][] EvaluateNArr(int n);

        public void Restart();

        public void SetWaitHandler(AutoResetEvent waitHandler);

        public Visualisation GetVisualisation();

        public double[] GetArr0();

        public void Draw(Graphics g, double[] pos, double c, bool flag);

        public void Restart(double[] newX0, double[] newV0, double newT0, double newDt);

        public void Restart(double[] newX0, double? newT0 = null, double? newDt = null);

        public void SetPeriod(int seconds, int timerPeriod, int speedValue);

        public double[]? GetNext();
    }
}
