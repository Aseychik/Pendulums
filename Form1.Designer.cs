namespace ManySpringsMatModel
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            xLabel = new Label();
            startButton = new Button();
            pauseButton = new Button();
            restartButton = new Button();
            pictureBox1 = new PictureBox();
            modelTimer = new System.Windows.Forms.Timer(components);
            label1 = new Label();
            button1 = new Button();
            button2 = new Button();
            modelsComboBox = new ComboBox();
            nUpDown = new NumericUpDown();
            nLabel = new Label();
            restartNButton = new Button();
            paramPropertyGrid = new PropertyGrid();
            checkBox1 = new CheckBox();
            checkBox2 = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nUpDown).BeginInit();
            SuspendLayout();
            // 
            // xLabel
            // 
            xLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            xLabel.AutoSize = true;
            xLabel.Location = new Point(1290, 9);
            xLabel.Name = "xLabel";
            xLabel.Size = new Size(17, 20);
            xLabel.TabIndex = 0;
            xLabel.Text = "0";
            // 
            // startButton
            // 
            startButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            startButton.Location = new Point(12, 539);
            startButton.Name = "startButton";
            startButton.Size = new Size(52, 34);
            startButton.TabIndex = 1;
            startButton.Text = "start";
            startButton.UseVisualStyleBackColor = true;
            startButton.Click += startButton_Click;
            // 
            // pauseButton
            // 
            pauseButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            pauseButton.Location = new Point(70, 539);
            pauseButton.Name = "pauseButton";
            pauseButton.Size = new Size(76, 34);
            pauseButton.TabIndex = 2;
            pauseButton.Text = "pause";
            pauseButton.UseVisualStyleBackColor = true;
            pauseButton.Click += pauseButton_Click;
            // 
            // restartButton
            // 
            restartButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            restartButton.Location = new Point(152, 539);
            restartButton.Name = "restartButton";
            restartButton.Size = new Size(64, 34);
            restartButton.TabIndex = 3;
            restartButton.Text = "restart";
            restartButton.UseVisualStyleBackColor = true;
            restartButton.Click += restartButton_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.BackColor = Color.White;
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1272, 521);
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            pictureBox1.Resize += ResizePictureBox;
            // 
            // modelTimer
            // 
            modelTimer.Interval = 30;
            modelTimer.Tick += TimerTick;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new Point(320, 546);
            label1.Name = "label1";
            label1.Size = new Size(17, 20);
            label1.TabIndex = 5;
            label1.Text = "1";
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button1.Location = new Point(343, 544);
            button1.Name = "button1";
            button1.Size = new Size(30, 29);
            button1.TabIndex = 6;
            button1.Text = ">";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button2.Location = new Point(287, 544);
            button2.Name = "button2";
            button2.Size = new Size(27, 29);
            button2.TabIndex = 7;
            button2.Text = "<";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // modelsComboBox
            // 
            modelsComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            modelsComboBox.FlatStyle = FlatStyle.Flat;
            modelsComboBox.FormattingEnabled = true;
            modelsComboBox.Items.AddRange(new object[] { "one pendulum", "two pendulums", "N pendulums", "Foucault Pendulum" });
            modelsComboBox.Location = new Point(1290, 48);
            modelsComboBox.Name = "modelsComboBox";
            modelsComboBox.Size = new Size(151, 28);
            modelsComboBox.TabIndex = 9;
            modelsComboBox.Text = "Foucault Pendulum";
            // 
            // nUpDown
            // 
            nUpDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            nUpDown.Location = new Point(1340, 79);
            nUpDown.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            nUpDown.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
            nUpDown.Name = "nUpDown";
            nUpDown.Size = new Size(52, 27);
            nUpDown.TabIndex = 11;
            nUpDown.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // nLabel
            // 
            nLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            nLabel.AutoSize = true;
            nLabel.Location = new Point(1296, 81);
            nLabel.Name = "nLabel";
            nLabel.Size = new Size(38, 20);
            nLabel.TabIndex = 12;
            nLabel.Text = "N = ";
            // 
            // restartNButton
            // 
            restartNButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            restartNButton.Location = new Point(1441, 504);
            restartNButton.Name = "restartNButton";
            restartNButton.Size = new Size(94, 29);
            restartNButton.TabIndex = 13;
            restartNButton.Text = "Restart";
            restartNButton.UseVisualStyleBackColor = true;
            restartNButton.Click += restartNButton_Click;
            // 
            // paramPropertyGrid
            // 
            paramPropertyGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            paramPropertyGrid.HelpVisible = false;
            paramPropertyGrid.Location = new Point(1296, 112);
            paramPropertyGrid.Name = "paramPropertyGrid";
            paramPropertyGrid.Size = new Size(245, 386);
            paramPropertyGrid.TabIndex = 14;
            paramPropertyGrid.PropertyValueChanged += ParamChanged;
            paramPropertyGrid.Click += paramPropertyGrid_Click;
            // 
            // checkBox1
            // 
            checkBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(591, 547);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(117, 24);
            checkBox1.TabIndex = 15;
            checkBox1.Text = "sprigs as rect";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(447, 546);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(101, 24);
            checkBox2.TabIndex = 16;
            checkBox2.Text = "checkBox2";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1547, 585);
            Controls.Add(checkBox2);
            Controls.Add(checkBox1);
            Controls.Add(paramPropertyGrid);
            Controls.Add(restartNButton);
            Controls.Add(nLabel);
            Controls.Add(nUpDown);
            Controls.Add(modelsComboBox);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(restartButton);
            Controls.Add(pauseButton);
            Controls.Add(startButton);
            Controls.Add(xLabel);
            Name = "Form1";
            Text = "Form1";
            FormClosing += ClosingForm;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)nUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label xLabel;
        private Button startButton;
        private Button pauseButton;
        private Button restartButton;
        private PictureBox pictureBox1;
        private System.Windows.Forms.Timer modelTimer;
        private Label label1;
        private Button button1;
        private Button button2;
        private ComboBox modelsComboBox;
        private NumericUpDown nUpDown;
        private Label nLabel;
        private Button restartNButton;
        private PropertyGrid paramPropertyGrid;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
    }
}
