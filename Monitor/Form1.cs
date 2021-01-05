using OpenHardwareMonitor.Hardware;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;


namespace Monitor
{
    public partial class Form1 : Form
    {

        static string data;
        Computer c = new Computer()
        {

            CPUEnabled = true,
            GPUEnabled = true

        };

        float value1, value2;

        private SerialPort port = new SerialPort();
        private ContextMenu contextMenu1;
        private MenuItem menuItem1, menuItem2, menuItem3, menuItem4;

        public Form1()
        {
            InitializeComponent();
            Init();
            if (AnotherInstanceExists())
            {
                MessageBox.Show("Another instance of Arduino GPU Monitor is running.", "Arduino GPU Monitor");
                this.Close();
            }
            this.Text = "Arduino GPU Monitor";

            this.contextMenu1 = new ContextMenu();
            this.menuItem1 = new MenuItem();
            this.menuItem2 = new MenuItem();
            this.menuItem3 = new MenuItem();
            this.menuItem4 = new MenuItem();

            this.contextMenu1.MenuItems.AddRange(new MenuItem[] {
                
                this.menuItem4,
                new MenuItem("-"), //divider
                this.menuItem3,
                this.menuItem2,
                new MenuItem("-"), //divider
                this.menuItem1
            });
            this.menuItem1.Text = "Exit";
            this.menuItem2.Text = "Disconnect";
            this.menuItem3.Text = "Connect";
            this.menuItem4.Text = "Status";
            this.menuItem1.Click += new EventHandler(this.menuItem1_Click);
            this.menuItem2.Click += new EventHandler(this.button3_Click);
            this.menuItem3.Click += (o, i) => { menuItem4.Text = "Connected"; };
            this.menuItem3.Click += new EventHandler(this.button5_Click);
            this.menuItem2.Click += (o, i) => { menuItem4.Text = "Disconnected"; };

            notifyIcon1.ContextMenu = this.contextMenu1;

        }

        public static bool AnotherInstanceExists()
        {
            Process currentRunningProcess = Process.GetCurrentProcess();
            Process[] listOfProcs = Process.GetProcessesByName(currentRunningProcess.ProcessName);

            foreach (Process proc in listOfProcs)
            {
                if ((proc.MainModule.FileName == currentRunningProcess.MainModule.FileName) && (proc.Id != currentRunningProcess.Id))
                    return true;
            }
            return false;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 25);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Set port:";
            this.toolTip2.SetToolTip(this.label1, "The COM port the arduino is conneceted to the pc through");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Interval";
            this.toolTip1.SetToolTip(this.label2, "Sets the report rate to the arduino. Defualt 100ms");
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.ForeColor = System.Drawing.Color.DarkRed;
            this.button3.Location = new System.Drawing.Point(237, 23);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(80, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "Disconnect";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button5
            // 
            this.button5.ForeColor = System.Drawing.Color.DarkGreen;
            this.button5.Location = new System.Drawing.Point(151, 23);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(80, 23);
            this.button5.TabIndex = 5;
            this.button5.Text = "Connect";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 112);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(342, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Arduino GPU Monitor";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(159, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Disconnected";
            // 
            // comboBox2
            // 
            this.comboBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.comboBox2.Location = new System.Drawing.Point(12, 76);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 20);
            this.comboBox2.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(111, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "ms";
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.Color.DarkRed;
            this.progressBar1.Enabled = false;
            this.progressBar1.ForeColor = System.Drawing.Color.DarkRed;
            this.progressBar1.Location = new System.Drawing.Point(12, 117);
            this.progressBar1.MarqueeAnimationSpeed = 1;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(121, 13);
            this.progressBar1.Step = 0;
            this.progressBar1.TabIndex = 11;
            this.progressBar1.Value = 100;
            // 
            // Form1
            // 
            this.AcceptButton = this.button5;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.button3;
            this.ClientSize = new System.Drawing.Size(342, 134);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(358, 173);
            this.MinimumSize = new System.Drawing.Size(358, 173);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        string[] ports = SerialPort.GetPortNames();

        private void menuItem1_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            this.Close();
        }

        private void Init()
        {
            try
            {
                notifyIcon1.Visible = false;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.DataBits = 8;
                port.Handshake = Handshake.None;
                port.RtsEnable = true;
                foreach (string port in ports)
                {
                    comboBox1.Items.Add(port);
                }
                port.BaudRate = 9600;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Arduino GPU Monitor");
            }
            progressBar1.Value = 0;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            try
            {
                port.Write("DIS*");
                port.Close();
            }
            catch { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                port.Write("DIS*");
                port.Close();
            }
            catch (Exception ex)
            {
            }
            timer1.Enabled = false;
            label3.Text = "Disconnected";
            data = "";
            progressBar1.Value = 0;
            MessageBox.Show($"Disconnected from {comboBox1.Text}", "Arduino GPU Monitor");

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (!port.IsOpen)
                {
                    int outInterval;
                    if (comboBox2.Text == "")
                    {
                        outInterval = 100;
                        comboBox2.Text = "100";
                    }
                    else
                    {
                        outInterval = Convert.ToInt32(comboBox2.Text);

                    }
                    port.PortName = comboBox1.Text;
                    port.Open();
                    timer1.Interval = outInterval;
                    timer1.Enabled = true;
                    label3.Text = "Connected";
                    progressBar1.Value = 100;
                    MessageBox.Show($"Connected to: {comboBox1.Text}. \n Report rate: {outInterval}ms", "Arduino GPU Monitor");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Arduino GPU Monitor");
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            Status();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            c.Open();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //if the form is minimized  
            //hide it from the task bar  
            //and show the system tray icon (represented by the NotifyIcon control)  
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void Status()
        {
            foreach (var hardwadre in c.Hardware)
            {

                if (hardwadre.HardwareType == HardwareType.GpuNvidia)
                {
                    hardwadre.Update();
                    foreach (var sensor in hardwadre.Sensors)
                        if (sensor.SensorType == SensorType.Temperature)
                        {

                            value1 = sensor.Value.GetValueOrDefault();
                        }

                }
                foreach (ISensor gpu in hardwadre.Sensors)
                {
                    if (gpu.SensorType == SensorType.Clock && gpu.Name == "GPU Core")
                    {
                        value2 = gpu.Value.GetValueOrDefault();
                    }
                }

            }
            try
            {
                port.Write(value1 + "*" + value2 + "#");
            }
            catch (Exception ex)
            {
                timer1.Stop();
                MessageBox.Show(ex.Message, "Arduino GPU Monitor");
                label3.Text = "Arduino's not responding...";
            }
        }
    }
}
