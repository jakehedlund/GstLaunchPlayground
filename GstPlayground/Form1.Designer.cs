
namespace GstPlayground
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;



        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSnap = new System.Windows.Forms.Button();
            this.pbxSnap = new System.Windows.Forms.PictureBox();
            this.txtPipe = new System.Windows.Forms.TextBox();
            this.gbxControls = new System.Windows.Forms.GroupBox();
            this.nudFlashOnTime = new System.Windows.Forms.NumericUpDown();
            this.chkLatencyEnable2 = new System.Windows.Forms.CheckBox();
            this.lblLatency = new System.Windows.Forms.Label();
            this.lblColor = new System.Windows.Forms.Label();
            this.btnDumpGraph = new System.Windows.Forms.Button();
            this.cmbLaunch = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlGst = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listDetectedPluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportAllLaunchLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.latencyTesterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblSsCursor = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSsColor = new System.Windows.Forms.ToolStripStatusLabel();
            this.importLaunchLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.pbxSnap)).BeginInit();
            this.gbxControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFlashOnTime)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(6, 16);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.toolTip1.SetToolTip(this.btnStart, "Try to parse what\'s in the launch line box. ");
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(94, 16);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "Reset";
            this.toolTip1.SetToolTip(this.btnReset, "Stop pipeline and reset.");
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSnap
            // 
            this.btnSnap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSnap.Location = new System.Drawing.Point(94, 45);
            this.btnSnap.Name = "btnSnap";
            this.btnSnap.Size = new System.Drawing.Size(75, 23);
            this.btnSnap.TabIndex = 4;
            this.btnSnap.Text = "Snapshot";
            this.toolTip1.SetToolTip(this.btnSnap, "Name your sink \"prevSink\"");
            this.btnSnap.UseVisualStyleBackColor = true;
            this.btnSnap.Click += new System.EventHandler(this.btnSnap_Click);
            // 
            // pbxSnap
            // 
            this.pbxSnap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbxSnap.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pbxSnap.Location = new System.Drawing.Point(8, 135);
            this.pbxSnap.Name = "pbxSnap";
            this.pbxSnap.Size = new System.Drawing.Size(161, 92);
            this.pbxSnap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxSnap.TabIndex = 5;
            this.pbxSnap.TabStop = false;
            this.toolTip1.SetToolTip(this.pbxSnap, "Last snapshot");
            // 
            // txtPipe
            // 
            this.txtPipe.AcceptsReturn = true;
            this.txtPipe.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPipe.Font = new System.Drawing.Font("Courier New", 10F);
            this.txtPipe.Location = new System.Drawing.Point(12, 429);
            this.txtPipe.Multiline = true;
            this.txtPipe.Name = "txtPipe";
            this.txtPipe.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPipe.Size = new System.Drawing.Size(760, 83);
            this.txtPipe.TabIndex = 6;
            this.txtPipe.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPipe_KeyDown);
            // 
            // gbxControls
            // 
            this.gbxControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxControls.Controls.Add(this.nudFlashOnTime);
            this.gbxControls.Controls.Add(this.chkLatencyEnable2);
            this.gbxControls.Controls.Add(this.lblLatency);
            this.gbxControls.Controls.Add(this.lblColor);
            this.gbxControls.Controls.Add(this.btnDumpGraph);
            this.gbxControls.Controls.Add(this.cmbLaunch);
            this.gbxControls.Controls.Add(this.btnStart);
            this.gbxControls.Controls.Add(this.btnReset);
            this.gbxControls.Controls.Add(this.pbxSnap);
            this.gbxControls.Controls.Add(this.btnSnap);
            this.gbxControls.Location = new System.Drawing.Point(637, 27);
            this.gbxControls.Name = "gbxControls";
            this.gbxControls.Size = new System.Drawing.Size(175, 396);
            this.gbxControls.TabIndex = 7;
            this.gbxControls.TabStop = false;
            this.gbxControls.Text = "GST Control";
            // 
            // nudFlashOnTime
            // 
            this.nudFlashOnTime.Location = new System.Drawing.Point(94, 113);
            this.nudFlashOnTime.Maximum = new decimal(new int[] {
            750,
            0,
            0,
            0});
            this.nudFlashOnTime.Name = "nudFlashOnTime";
            this.nudFlashOnTime.Size = new System.Drawing.Size(75, 20);
            this.nudFlashOnTime.TabIndex = 12;
            this.toolTip1.SetToolTip(this.nudFlashOnTime, "Flash time.");
            this.nudFlashOnTime.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.nudFlashOnTime.ValueChanged += new System.EventHandler(this.nudFlashOnTime_ValueChanged);
            // 
            // chkLatencyEnable2
            // 
            this.chkLatencyEnable2.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkLatencyEnable2.AutoSize = true;
            this.chkLatencyEnable2.Location = new System.Drawing.Point(94, 74);
            this.chkLatencyEnable2.Name = "chkLatencyEnable2";
            this.chkLatencyEnable2.Size = new System.Drawing.Size(79, 23);
            this.chkLatencyEnable2.TabIndex = 11;
            this.chkLatencyEnable2.Text = "Latency Test";
            this.chkLatencyEnable2.UseVisualStyleBackColor = true;
            this.chkLatencyEnable2.CheckedChanged += new System.EventHandler(this.chkLatencyEnable_CheckedChanged);
            // 
            // lblLatency
            // 
            this.lblLatency.AutoSize = true;
            this.lblLatency.Location = new System.Drawing.Point(6, 117);
            this.lblLatency.Name = "lblLatency";
            this.lblLatency.Size = new System.Drawing.Size(45, 13);
            this.lblLatency.TabIndex = 10;
            this.lblLatency.Text = "Latency";
            // 
            // lblColor
            // 
            this.lblColor.AutoSize = true;
            this.lblColor.Location = new System.Drawing.Point(8, 100);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(30, 13);
            this.lblColor.TabIndex = 9;
            this.lblColor.Text = "color";
            // 
            // btnDumpGraph
            // 
            this.btnDumpGraph.Location = new System.Drawing.Point(6, 45);
            this.btnDumpGraph.Name = "btnDumpGraph";
            this.btnDumpGraph.Size = new System.Drawing.Size(75, 23);
            this.btnDumpGraph.TabIndex = 7;
            this.btnDumpGraph.Text = "Dump Graph";
            this.toolTip1.SetToolTip(this.btnDumpGraph, "Saved to \"C:\\gstreamer\\dotfiles\"");
            this.btnDumpGraph.UseVisualStyleBackColor = true;
            this.btnDumpGraph.Click += new System.EventHandler(this.btnDumpGraph_Click);
            // 
            // cmbLaunch
            // 
            this.cmbLaunch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLaunch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.cmbLaunch.FormattingEnabled = true;
            this.cmbLaunch.Location = new System.Drawing.Point(8, 233);
            this.cmbLaunch.Name = "cmbLaunch";
            this.cmbLaunch.Size = new System.Drawing.Size(161, 155);
            this.cmbLaunch.Sorted = true;
            this.cmbLaunch.TabIndex = 6;
            this.toolTip1.SetToolTip(this.cmbLaunch, "List of your saved launch lines here. ");
            this.cmbLaunch.SelectedIndexChanged += new System.EventHandler(this.cmbLaunch_SelectedIndexChanged);
            this.cmbLaunch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbLaunch_KeyDown);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(778, 430);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(43, 83);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.toolTip1.SetToolTip(this.btnSave, "Save launch line to associated name.");
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlGst
            // 
            this.pnlGst.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGst.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pnlGst.Location = new System.Drawing.Point(12, 27);
            this.pnlGst.Name = "pnlGst";
            this.pnlGst.Size = new System.Drawing.Size(619, 396);
            this.pnlGst.TabIndex = 1;
            this.pnlGst.DoubleClick += new System.EventHandler(this.pnlGst_DoubleClick);
            this.pnlGst.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlGst_MouseClick);
            this.pnlGst.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pnlGst_MouseDoubleClick);
            this.pnlGst.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlGst_MouseDown);
            this.pnlGst.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlGst_MouseMove);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(824, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.listDetectedPluginsToolStripMenuItem,
            this.latencyTesterToolStripMenuItem,
            this.openConsoleToolStripMenuItem,
            this.toolStripSeparator1,
            this.exportAllLaunchLinesToolStripMenuItem,
            this.importLaunchLinesToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            this.toolsToolStripMenuItem.Click += new System.EventHandler(this.toolsToolStripMenuItem_Click);
            // 
            // listDetectedPluginsToolStripMenuItem
            // 
            this.listDetectedPluginsToolStripMenuItem.Name = "listDetectedPluginsToolStripMenuItem";
            this.listDetectedPluginsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.listDetectedPluginsToolStripMenuItem.Text = "List detected plugins";
            this.listDetectedPluginsToolStripMenuItem.Click += new System.EventHandler(this.listDetectedPluginsToolStripMenuItem_Click);
            // 
            // exportAllLaunchLinesToolStripMenuItem
            // 
            this.exportAllLaunchLinesToolStripMenuItem.Name = "exportAllLaunchLinesToolStripMenuItem";
            this.exportAllLaunchLinesToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.exportAllLaunchLinesToolStripMenuItem.Text = "Export launch lines...";
            this.exportAllLaunchLinesToolStripMenuItem.Click += new System.EventHandler(this.exportAllLaunchLinesToolStripMenuItem_Click);
            // 
            // latencyTesterToolStripMenuItem
            // 
            this.latencyTesterToolStripMenuItem.Name = "latencyTesterToolStripMenuItem";
            this.latencyTesterToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.latencyTesterToolStripMenuItem.Text = "Latency tester";
            this.latencyTesterToolStripMenuItem.Click += new System.EventHandler(this.latencyTesterToolStripMenuItem_Click);
            // 
            // openConsoleToolStripMenuItem
            // 
            this.openConsoleToolStripMenuItem.Name = "openConsoleToolStripMenuItem";
            this.openConsoleToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.openConsoleToolStripMenuItem.Text = "Open Console";
            this.openConsoleToolStripMenuItem.Click += new System.EventHandler(this.openConsoleToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblSsCursor,
            this.lblSsColor});
            this.statusStrip1.Location = new System.Drawing.Point(0, 516);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(824, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblSsCursor
            // 
            this.lblSsCursor.Name = "lblSsCursor";
            this.lblSsCursor.Size = new System.Drawing.Size(42, 17);
            this.lblSsCursor.Text = "Cursor";
            // 
            // lblSsColor
            // 
            this.lblSsColor.Name = "lblSsColor";
            this.lblSsColor.Size = new System.Drawing.Size(118, 17);
            this.lblSsColor.Text = "toolStripStatusLabel1";
            // 
            // importLaunchLinesToolStripMenuItem
            // 
            this.importLaunchLinesToolStripMenuItem.Name = "importLaunchLinesToolStripMenuItem";
            this.importLaunchLinesToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.importLaunchLinesToolStripMenuItem.Text = "Import launch lines...";
            this.importLaunchLinesToolStripMenuItem.Click += new System.EventHandler(this.importLaunchLinesToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(182, 6);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 538);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnlGst);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbxControls);
            this.Controls.Add(this.txtPipe);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "GST Launch Playground";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbxSnap)).EndInit();
            this.gbxControls.ResumeLayout(false);
            this.gbxControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFlashOnTime)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnSnap;
        private System.Windows.Forms.PictureBox pbxSnap;
        private System.Windows.Forms.TextBox txtPipe;
        private System.Windows.Forms.GroupBox gbxControls;
        private System.Windows.Forms.ComboBox cmbLaunch;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnDumpGraph;
        private System.Windows.Forms.Panel pnlGst;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listDetectedPluginsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportAllLaunchLinesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem latencyTesterToolStripMenuItem;
        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.Label lblLatency;
        private System.Windows.Forms.CheckBox chkLatencyEnable2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblSsCursor;
        private System.Windows.Forms.ToolStripStatusLabel lblSsColor;
        private System.Windows.Forms.NumericUpDown nudFlashOnTime;
        private System.Windows.Forms.ToolStripMenuItem openConsoleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importLaunchLinesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}

