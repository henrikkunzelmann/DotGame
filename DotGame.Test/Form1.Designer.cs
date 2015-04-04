namespace DotGame.Test
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnStreamPlay = new System.Windows.Forms.Button();
            this.btnStreamPause = new System.Windows.Forms.Button();
            this.btnStreamStop = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbStreamQueued = new System.Windows.Forms.Label();
            this.lbStreamProcessed = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.tbStreamGain = new System.Windows.Forms.TrackBar();
            this.tbStreamPitch = new System.Windows.Forms.TrackBar();
            this.cbStreamLoop = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btn3DStop = new System.Windows.Forms.Button();
            this.btn3DPause = new System.Windows.Forms.Button();
            this.btn3DPlay = new System.Windows.Forms.Button();
            this.cb3DLoop = new System.Windows.Forms.CheckBox();
            this.tb3DGain = new System.Windows.Forms.TrackBar();
            this.tb3DPitch = new System.Windows.Forms.TrackBar();
            this.tb3Dx = new System.Windows.Forms.TrackBar();
            this.tb3Dy = new System.Windows.Forms.TrackBar();
            this.tb3Dz = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbStreamGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbStreamPitch)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tb3DGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb3DPitch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb3Dx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb3Dy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb3Dz)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(613, 418);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.btnStreamStop, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnStreamPause, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnStreamPlay, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 19);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(291, 29);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnStreamPlay
            // 
            this.btnStreamPlay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStreamPlay.Location = new System.Drawing.Point(3, 3);
            this.btnStreamPlay.Name = "btnStreamPlay";
            this.btnStreamPlay.Size = new System.Drawing.Size(90, 23);
            this.btnStreamPlay.TabIndex = 0;
            this.btnStreamPlay.Text = "Play";
            this.btnStreamPlay.UseVisualStyleBackColor = true;
            this.btnStreamPlay.Click += new System.EventHandler(this.btnStreamPlay_Click);
            // 
            // btnStreamPause
            // 
            this.btnStreamPause.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStreamPause.Location = new System.Drawing.Point(99, 3);
            this.btnStreamPause.Name = "btnStreamPause";
            this.btnStreamPause.Size = new System.Drawing.Size(90, 23);
            this.btnStreamPause.TabIndex = 1;
            this.btnStreamPause.Text = "Pause";
            this.btnStreamPause.UseVisualStyleBackColor = true;
            this.btnStreamPause.Click += new System.EventHandler(this.btnStreamPause_Click);
            // 
            // btnStreamStop
            // 
            this.btnStreamStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStreamStop.Location = new System.Drawing.Point(195, 3);
            this.btnStreamStop.Name = "btnStreamStop";
            this.btnStreamStop.Size = new System.Drawing.Size(93, 23);
            this.btnStreamStop.TabIndex = 2;
            this.btnStreamStop.Text = "Stop";
            this.btnStreamStop.UseVisualStyleBackColor = true;
            this.btnStreamStop.Click += new System.EventHandler(this.btnStreamStop_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cbStreamLoop);
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Controls.Add(this.tbStreamPitch);
            this.groupBox1.Controls.Add(this.tbStreamGain);
            this.groupBox1.Controls.Add(this.lbStreamProcessed);
            this.groupBox1.Controls.Add(this.lbStreamQueued);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(303, 152);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Stream";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Queued:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Processed:";
            // 
            // lbStreamQueued
            // 
            this.lbStreamQueued.AutoSize = true;
            this.lbStreamQueued.Location = new System.Drawing.Point(72, 55);
            this.lbStreamQueued.Name = "lbStreamQueued";
            this.lbStreamQueued.Size = new System.Drawing.Size(10, 13);
            this.lbStreamQueued.TabIndex = 3;
            this.lbStreamQueued.Text = " ";
            // 
            // lbStreamProcessed
            // 
            this.lbStreamProcessed.AutoSize = true;
            this.lbStreamProcessed.Location = new System.Drawing.Point(72, 68);
            this.lbStreamProcessed.Name = "lbStreamProcessed";
            this.lbStreamProcessed.Size = new System.Drawing.Size(10, 13);
            this.lbStreamProcessed.TabIndex = 4;
            this.lbStreamProcessed.Text = " ";
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 40;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // tbStreamGain
            // 
            this.tbStreamGain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbStreamGain.Location = new System.Drawing.Point(6, 105);
            this.tbStreamGain.Maximum = 100;
            this.tbStreamGain.Name = "tbStreamGain";
            this.tbStreamGain.Size = new System.Drawing.Size(291, 45);
            this.tbStreamGain.TabIndex = 5;
            this.tbStreamGain.Value = 100;
            this.tbStreamGain.Scroll += new System.EventHandler(this.tbStreamGain_Scroll);
            // 
            // tbStreamPitch
            // 
            this.tbStreamPitch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbStreamPitch.Location = new System.Drawing.Point(105, 54);
            this.tbStreamPitch.Maximum = 100;
            this.tbStreamPitch.Name = "tbStreamPitch";
            this.tbStreamPitch.Size = new System.Drawing.Size(192, 45);
            this.tbStreamPitch.TabIndex = 6;
            this.tbStreamPitch.Value = 25;
            this.tbStreamPitch.Scroll += new System.EventHandler(this.tbStreamPitch_Scroll);
            // 
            // cbStreamLoop
            // 
            this.cbStreamLoop.AutoSize = true;
            this.cbStreamLoop.Location = new System.Drawing.Point(15, 84);
            this.cbStreamLoop.Name = "cbStreamLoop";
            this.cbStreamLoop.Size = new System.Drawing.Size(50, 17);
            this.cbStreamLoop.TabIndex = 7;
            this.cbStreamLoop.Text = "Loop";
            this.cbStreamLoop.UseVisualStyleBackColor = true;
            this.cbStreamLoop.CheckedChanged += new System.EventHandler(this.cbStreamLoop_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.tb3Dz);
            this.groupBox2.Controls.Add(this.tb3Dy);
            this.groupBox2.Controls.Add(this.tb3Dx);
            this.groupBox2.Controls.Add(this.tb3DPitch);
            this.groupBox2.Controls.Add(this.tb3DGain);
            this.groupBox2.Controls.Add(this.cb3DLoop);
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.Location = new System.Drawing.Point(9, 160);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(297, 255);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "3D Sound";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.btn3DStop, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btn3DPause, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btn3DPlay, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 19);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(285, 30);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // btn3DStop
            // 
            this.btn3DStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn3DStop.Location = new System.Drawing.Point(191, 3);
            this.btn3DStop.Name = "btn3DStop";
            this.btn3DStop.Size = new System.Drawing.Size(91, 24);
            this.btn3DStop.TabIndex = 2;
            this.btn3DStop.Text = "Stop";
            this.btn3DStop.UseVisualStyleBackColor = true;
            this.btn3DStop.Click += new System.EventHandler(this.btn3DStop_Click);
            // 
            // btn3DPause
            // 
            this.btn3DPause.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn3DPause.Location = new System.Drawing.Point(97, 3);
            this.btn3DPause.Name = "btn3DPause";
            this.btn3DPause.Size = new System.Drawing.Size(88, 24);
            this.btn3DPause.TabIndex = 1;
            this.btn3DPause.Text = "Pause";
            this.btn3DPause.UseVisualStyleBackColor = true;
            this.btn3DPause.Click += new System.EventHandler(this.btn3DPause_Click);
            // 
            // btn3DPlay
            // 
            this.btn3DPlay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn3DPlay.Location = new System.Drawing.Point(3, 3);
            this.btn3DPlay.Name = "btn3DPlay";
            this.btn3DPlay.Size = new System.Drawing.Size(88, 24);
            this.btn3DPlay.TabIndex = 0;
            this.btn3DPlay.Text = "Play";
            this.btn3DPlay.UseVisualStyleBackColor = true;
            this.btn3DPlay.Click += new System.EventHandler(this.btn3DPlay_Click);
            // 
            // cb3DLoop
            // 
            this.cb3DLoop.AutoSize = true;
            this.cb3DLoop.Location = new System.Drawing.Point(9, 55);
            this.cb3DLoop.Name = "cb3DLoop";
            this.cb3DLoop.Size = new System.Drawing.Size(50, 17);
            this.cb3DLoop.TabIndex = 8;
            this.cb3DLoop.Text = "Loop";
            this.cb3DLoop.UseVisualStyleBackColor = true;
            this.cb3DLoop.CheckedChanged += new System.EventHandler(this.cb3DLoop_CheckedChanged);
            // 
            // tb3DGain
            // 
            this.tb3DGain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb3DGain.Location = new System.Drawing.Point(3, 204);
            this.tb3DGain.Maximum = 100;
            this.tb3DGain.Name = "tb3DGain";
            this.tb3DGain.Size = new System.Drawing.Size(291, 45);
            this.tb3DGain.TabIndex = 8;
            this.tb3DGain.Value = 100;
            this.tb3DGain.Scroll += new System.EventHandler(this.tb3DGain_Scroll);
            // 
            // tb3DPitch
            // 
            this.tb3DPitch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tb3DPitch.Location = new System.Drawing.Point(242, 55);
            this.tb3DPitch.Maximum = 100;
            this.tb3DPitch.Name = "tb3DPitch";
            this.tb3DPitch.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tb3DPitch.Size = new System.Drawing.Size(46, 143);
            this.tb3DPitch.TabIndex = 8;
            this.tb3DPitch.Value = 25;
            this.tb3DPitch.Scroll += new System.EventHandler(this.tb3DPitch_Scroll);
            // 
            // tb3Dx
            // 
            this.tb3Dx.Location = new System.Drawing.Point(6, 78);
            this.tb3Dx.Maximum = 100;
            this.tb3Dx.Minimum = -100;
            this.tb3Dx.Name = "tb3Dx";
            this.tb3Dx.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tb3Dx.Size = new System.Drawing.Size(45, 120);
            this.tb3Dx.TabIndex = 9;
            this.tb3Dx.Scroll += new System.EventHandler(this.tb3Dx_Scroll);
            // 
            // tb3Dy
            // 
            this.tb3Dy.Location = new System.Drawing.Point(57, 78);
            this.tb3Dy.Maximum = 100;
            this.tb3Dy.Minimum = -100;
            this.tb3Dy.Name = "tb3Dy";
            this.tb3Dy.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tb3Dy.Size = new System.Drawing.Size(45, 120);
            this.tb3Dy.TabIndex = 10;
            this.tb3Dy.Scroll += new System.EventHandler(this.tb3Dy_Scroll);
            // 
            // tb3Dz
            // 
            this.tb3Dz.Location = new System.Drawing.Point(108, 78);
            this.tb3Dz.Maximum = 100;
            this.tb3Dz.Minimum = -100;
            this.tb3Dz.Name = "tb3Dz";
            this.tb3Dz.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tb3Dz.Size = new System.Drawing.Size(45, 120);
            this.tb3Dz.TabIndex = 11;
            this.tb3Dz.Scroll += new System.EventHandler(this.tb3Dz_Scroll);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 418);
            this.Controls.Add(this.splitContainer1);
            this.MaximumSize = new System.Drawing.Size(1000, 456);
            this.MinimumSize = new System.Drawing.Size(0, 456);
            this.Name = "Form1";
            this.Text = "DotGame Test";
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbStreamGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbStreamPitch)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tb3DGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb3DPitch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb3Dx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb3Dy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb3Dz)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnStreamStop;
        private System.Windows.Forms.Button btnStreamPause;
        private System.Windows.Forms.Button btnStreamPlay;
        private System.Windows.Forms.Label lbStreamProcessed;
        private System.Windows.Forms.Label lbStreamQueued;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.TrackBar tbStreamGain;
        private System.Windows.Forms.TrackBar tbStreamPitch;
        private System.Windows.Forms.CheckBox cbStreamLoop;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cb3DLoop;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btn3DStop;
        private System.Windows.Forms.Button btn3DPause;
        private System.Windows.Forms.Button btn3DPlay;
        private System.Windows.Forms.TrackBar tb3Dz;
        private System.Windows.Forms.TrackBar tb3Dy;
        private System.Windows.Forms.TrackBar tb3Dx;
        private System.Windows.Forms.TrackBar tb3DPitch;
        private System.Windows.Forms.TrackBar tb3DGain;
    }
}

