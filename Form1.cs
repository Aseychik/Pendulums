using System.Windows.Forms;

namespace ManySpringsMatModel
{
    public partial class Form1 : Form
    {
        Model[] models;
        int tick = 0;
        Thread evaluationThread;
        AutoResetEvent waitHandler;
        Graphics g;

        MathModelProperties propGridParams;
        Models currentModel;
        int speedValue = 1;
        int secondsPeriod = 5;
        bool threadIsActive = true;
        bool flagSetPos = false;
        GraphDraw[] draws;

        public Form1()
        {
            InitializeComponent();

            waitHandler = new AutoResetEvent(false);
            evaluationThread = new(EvaluateNextForCurrentModel);
            int n = 5;
            double[] initialPoses = new double[2 * n];
            initialPoses[0] = -0.5;
            initialPoses[n - 1] = 0.3;
            int w = pictureBox1.Width, wallW = 20;
            w -= 20;
            w -= wallW * 2;
            double k = w / (2.0 * n + 4 * (n + 1));

            models = [new OneSpringModel([-0.5, 0], new OneSpringVisualize(100, 100)),
                new TwoSpringsModel([-0.5, 0, 0, 0], new TwoSpringsVisualize(100, 100)),
                new NSpringsModel(n, initialPoses, new NSpringsVisualize(n, 10, 100, (int)(k * 4), (int)(k * 2), wallW)),
                new MathModel([0.2, 0.2, 0.2, -0.1],
                [new MDXYFunc(0), new MDXYFunc(1), new MDVXFunction(), new MDVYFunction()],
                new FoucautVisualization(10, 10, bgColor: pictureBox1.BackColor))
            ];
            currentModel = Models.Foucault;
            if (currentModel != Models.NSprings)
            {
                nLabel.Visible = false;
                nUpDown.Visible = false;
            }


            foreach (Model m in models)
            {
                m.SetPeriod(secondsPeriod, modelTimer.Interval, speedValue);
                m.SetWaitHandler(waitHandler);
            }
            evaluationThread.Name = "Evaluation thread";
            evaluationThread.Start();

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);

            n = UpdateN(n);

            int grWidth = n * 40;
            grWidth = Math.Min(Math.Max(200, grWidth), (pictureBox1.Width - 80 - 100) / 2);

            draws = [new GraphDraw(40, 230, grWidth, 200, paramCount: n), new GraphDraw(40 + grWidth + 100, 230, grWidth, 200, paramCount: n, axisNames: ["v", "t"])];

            propGridParams = new MathModelProperties(GetCurrentModel().GetArr0());
            paramPropertyGrid.SelectedObject = propGridParams;
        }

        public int UpdateN(int n)
        {
            if ((int)currentModel < 2)
                return (int)currentModel + 1;
            if (currentModel == Models.Foucault) return 2;
            return n;
        }

        private Model GetCurrentModel() { return models[(int)currentModel]; }

        private void EvaluateNextForCurrentModel()
        {
            try
            {
                waitHandler.WaitOne();
                while (threadIsActive)
                {
                    GetCurrentModel().Evaluate();
                    waitHandler.WaitOne();
                }
            }
            catch { }
        }


        private void startButton_Click(object sender, EventArgs e)
        {
            modelTimer.Start();

        }

        private void TimerTick(object sender, EventArgs e)
        {
            var res = GetCurrentModel().GetNext();
            if (res != null)
            {
                xLabel.Text = res[0].ToString() + " " + tick.ToString();
                draws[0].AddPoint(res);
                draws[1].AddPoint(res, res.Length / 2);

                if (currentModel == Models.OneSpring && res[0] < -1)
                {
                    Model m = GetCurrentModel();
                    m.Restart([-1], [0], 0, 0.1);
                    m.SetPeriod(secondsPeriod, modelTimer.Interval, speedValue);
                    res[0] = -1;
                }
                tick++;
                g.Clear(pictureBox1.BackColor);
                GetCurrentModel().Draw(g, res, 100, checkBox1.Checked);
                draws[0].Draw(g);
                draws[1].Draw(g);

                pictureBox1.Invalidate();
            }
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            modelTimer.Stop();
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            modelTimer.Stop();
            Model m = GetCurrentModel();
            m.Restart();
            m.GetNext();
            foreach (GraphDraw dr in draws)
                dr.Restart();
            m.GetVisualisation().Restart();
            modelTimer.Start();
        }

        private void ClosingForm(object sender, FormClosingEventArgs e)
        {
            threadIsActive = false;
            waitHandler.Set();
            Thread.Sleep(10);
            evaluationThread.Interrupt();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (speedValue < 64)
            {
                speedValue *= 2;
                label1.Text = ((int)speedValue).ToString();
                GetCurrentModel().SetPeriod(secondsPeriod, modelTimer.Interval, speedValue);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (speedValue > 1)
            {
                speedValue /= 2;
                label1.Text = ((int)speedValue).ToString();
                GetCurrentModel().SetPeriod(secondsPeriod, modelTimer.Interval, speedValue);
            }
        }

        private void setPosButton_Click(object sender, EventArgs e)
        {
            flagSetPos = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (flagSetPos && (currentModel == Models.OneSpring))
            {
                modelTimer.Stop();
                double x = pictureBox1.PointToClient(Cursor.Position).X;
                Model m = GetCurrentModel();
                m.Restart([(x - m.GetVisualisation().GetPos()[0] - 200) / 100], [0], 0, 0.1);
                m.SetPeriod(secondsPeriod, modelTimer.Interval, speedValue);
                g.Clear(pictureBox1.BackColor);
                m.Draw(g, [(x - m.GetVisualisation().GetPos()[0] - 200)], 1, checkBox1.Checked);
                foreach (GraphDraw dr in draws)
                    dr.Restart();
                pictureBox1.Invalidate();
                modelTimer.Start();
            }
            if (flagSetPos && (currentModel == Models.Foucault))
            {
                modelTimer.Stop();
                double x = pictureBox1.PointToClient(Cursor.Position).X,
                    y = pictureBox1.PointToClient(Cursor.Position).Y;
                Model m = GetCurrentModel();
                double[] pos = [(x - m.GetVisualisation().GetPos()[0] - 100) / 100, (y - m.GetVisualisation().GetPos()[1] - 100) / 100, 0, 0];
                m.Restart(pos, 0, 0.001);
                m.SetPeriod(secondsPeriod, modelTimer.Interval, speedValue);
                g.Clear(pictureBox1.BackColor);
                m.Draw(g, pos, 100, checkBox1.Checked);
                m.GetVisualisation().Restart();
                foreach (GraphDraw dr in draws)
                    dr.Restart();
                pictureBox1.Invalidate();
                modelTimer.Start();
            }
        }

        private void restartNButton_Click(object sender, EventArgs e)
        {
            modelTimer.Stop();
            String s = modelsComboBox.Text;
            Models? model = s switch
            {
                "one pendulum" => Models.OneSpring,
                "two pendulums" => Models.TwoSprings,
                "N pendulums" => Models.NSprings,
                "Foucault Pendulum" => Models.Foucault,
                _ => null
            };
            if (model == null) return;

            if (currentModel == model && propGridParams.GetIsEdited())
            {
                propGridParams.UpdateModelAndRestart(GetCurrentModel());
            }
            else
            {
                propGridParams = new MathModelProperties(models[(int)model].GetArr0());
                paramPropertyGrid.SelectedObject = propGridParams;
            }
            bool fl = false;

            if (currentModel == Models.NSprings && model == Models.NSprings && (int)nUpDown.Value * 2 != GetCurrentModel().GetArr0().Length)
                fl = true;

            currentModel = model ?? Models.NSprings;


            int n = UpdateN((int)nUpDown.Value);

            if (currentModel == Models.NSprings)
            {
                nUpDown.Visible = true;
                nLabel.Visible = true;
                if (fl)
                {
                    double[] initialPoses = new double[2 * n];
                    initialPoses[0] = -0.5;
                    initialPoses[n - 1] = 0.3;
                    int w = pictureBox1.Width, wallW = 20;
                    w -= 20;
                    w -= wallW * 2;
                    double k = w / (2.0 * n + 4 * (n + 1));
                    models[(int)Models.NSprings] = new NSpringsModel(n, initialPoses, new NSpringsVisualize(n, 10, 100, (int)(k * 4), (int)(k * 2), wallW));
                }

                propGridParams = new MathModelProperties(models[(int)currentModel].GetArr0());
                paramPropertyGrid.SelectedObject = propGridParams;
            }
            else
            {
                nLabel.Visible = false;
                nUpDown.Visible = false;
            }

            Model m = GetCurrentModel();

            m.SetPeriod(secondsPeriod, modelTimer.Interval, speedValue);
            m.SetWaitHandler(waitHandler);

            m.Restart();
            m.GetVisualisation().Restart();

            propGridParams.SetEdited(false);
            propGridParams.UpdateParams(m.GetArr0());
            int grWidth = n * 40;
            grWidth = Math.Min(Math.Max(200, grWidth), (pictureBox1.Width - 40 - 100) / 2);

            draws = [new GraphDraw(40, 230, grWidth, 200, paramCount: n), new GraphDraw(40 + grWidth + 100, 230, grWidth, 200, paramCount: n, axisNames: ["v", "t"])];

            modelTimer.Start();
        }

        private void paramPropertyGrid_Click(object sender, EventArgs e)
        {

        }

        private void ParamChanged(object s, PropertyValueChangedEventArgs e)
        {
            propGridParams.SetEdited(true);

        }

        private void ResizePictureBox(object sender, EventArgs e)
        {
            /*pictureBox1.Image.Dispose();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            g = Graphics.FromImage(pictureBox1.Image);*/
        }
    }
}
