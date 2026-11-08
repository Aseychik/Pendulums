using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManySpringsMatModel
{
    internal interface PropertyInterface
    {
        public void UpdateModelAndRestart(Model model);

        public void UpdateParams(double[] arrParam);
    }

    public class MathModelProperties : PropertyInterface
    {
        [DisplayName("x")]
        public double[] xArr { get; set; }

        [DisplayName("V")]
        public double[] vArr { get; set; }

        private bool isEdited = false;

        public void SetEdited(bool isEdited)
        { this.isEdited = isEdited; }

        public bool GetIsEdited()
        { return this.isEdited; }

        public MathModelProperties(double[] prms)
        {
            int l = prms.Length / 2;
            xArr = new double[l];
            vArr = new double[l];
            for (int i = 0; i < l; i++)
            {
                xArr[i] = prms[i];
                vArr[i] = prms[i + l];
            }

        }

        public void UpdateModelAndRestart(Model model)
        {
            int l = xArr.Length;
            double[] res = new double[2 * l];
            for (int i = 0; i < l; i++)
            {
                res[i] = xArr[i];
                res[i + l] = vArr[i];
            }
            model.Restart(res);
        }

        // хранятся значения по умолчанию и изменяются тоже они, а соответственно просто необходимо при нажатии на кнопку restart вызывать UpdateModelAndRestart() если параметры менялись

        public void UpdateParams(double[] arrParam)
        {
            int shift = arrParam.Length / 2;
            for (int i = 0; i < shift; i++)
            {
                xArr[i] = arrParam[i];
                vArr[i] = arrParam[i + shift];
            }
        }
    }
}
