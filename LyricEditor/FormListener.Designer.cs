namespace LyricEditor
{
    partial class FormListener
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormListener));
            this.buttonPlay = new System.Windows.Forms.Button();
            this.windowsMediaPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            this.buttonStop = new System.Windows.Forms.Button();
            this.trackBarTimeline = new System.Windows.Forms.TrackBar();
            this.labelTimeline = new System.Windows.Forms.Label();
            this.textBox = new System.Windows.Forms.TextBox();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.numericUpDownSpeed = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonListen = new System.Windows.Forms.Button();
            this.ucNoteGraph = new LyricEditor.UCNoteGraph();
            ((System.ComponentModel.ISupportInitialize)(this.windowsMediaPlayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTimeline)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonPlay
            // 
            this.buttonPlay.Location = new System.Drawing.Point(12, 12);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(40, 40);
            this.buttonPlay.TabIndex = 0;
            this.buttonPlay.Text = "P";
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // windowsMediaPlayer
            // 
            this.windowsMediaPlayer.Enabled = true;
            this.windowsMediaPlayer.Location = new System.Drawing.Point(60, 15);
            this.windowsMediaPlayer.Name = "windowsMediaPlayer";
            this.windowsMediaPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("windowsMediaPlayer.OcxState")));
            this.windowsMediaPlayer.Size = new System.Drawing.Size(38, 37);
            this.windowsMediaPlayer.TabIndex = 1;
            this.windowsMediaPlayer.Visible = false;
            this.windowsMediaPlayer.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(this.windowsMediaPlayer_PlayStateChange);
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(58, 12);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(40, 40);
            this.buttonStop.TabIndex = 2;
            this.buttonStop.Text = "S";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // trackBarTimeline
            // 
            this.trackBarTimeline.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarTimeline.Location = new System.Drawing.Point(150, 12);
            this.trackBarTimeline.Name = "trackBarTimeline";
            this.trackBarTimeline.Size = new System.Drawing.Size(422, 45);
            this.trackBarTimeline.TabIndex = 3;
            this.trackBarTimeline.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarTimeline.Value = 9;
            this.trackBarTimeline.ValueChanged += new System.EventHandler(this.trackBarTimeline_ValueChanged);
            // 
            // labelTimeline
            // 
            this.labelTimeline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTimeline.Location = new System.Drawing.Point(499, 34);
            this.labelTimeline.Name = "labelTimeline";
            this.labelTimeline.Size = new System.Drawing.Size(73, 21);
            this.labelTimeline.TabIndex = 4;
            this.labelTimeline.Text = "00:00.000";
            this.labelTimeline.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox
            // 
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox.Location = new System.Drawing.Point(416, 58);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.Size = new System.Drawing.Size(156, 300);
            this.textBox.TabIndex = 5;
            // 
            // timerRefresh
            // 
            this.timerRefresh.Interval = 30;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(497, 364);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(416, 364);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // numericUpDownSpeed
            // 
            this.numericUpDownSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownSpeed.Location = new System.Drawing.Point(60, 367);
            this.numericUpDownSpeed.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericUpDownSpeed.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownSpeed.Name = "numericUpDownSpeed";
            this.numericUpDownSpeed.Size = new System.Drawing.Size(49, 20);
            this.numericUpDownSpeed.TabIndex = 8;
            this.numericUpDownSpeed.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownSpeed.ValueChanged += new System.EventHandler(this.numericUpDownSpeed_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 369);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Speed";
            // 
            // buttonListen
            // 
            this.buttonListen.Enabled = false;
            this.buttonListen.Location = new System.Drawing.Point(104, 12);
            this.buttonListen.Name = "buttonListen";
            this.buttonListen.Size = new System.Drawing.Size(40, 40);
            this.buttonListen.TabIndex = 10;
            this.buttonListen.Text = "L";
            this.buttonListen.UseVisualStyleBackColor = true;
            this.buttonListen.Click += new System.EventHandler(this.buttonListen_Click);
            // 
            // ucNoteGraph
            // 
            this.ucNoteGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ucNoteGraph.Location = new System.Drawing.Point(15, 58);
            this.ucNoteGraph.Margin = new System.Windows.Forms.Padding(0);
            this.ucNoteGraph.Name = "ucNoteGraph";
            this.ucNoteGraph.NoteWidth = 5;
            this.ucNoteGraph.PitchRange = 20;
            this.ucNoteGraph.PitchStart = 0;
            this.ucNoteGraph.Size = new System.Drawing.Size(398, 300);
            this.ucNoteGraph.TabIndex = 11;
            // 
            // FormListener
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 399);
            this.Controls.Add(this.ucNoteGraph);
            this.Controls.Add(this.buttonListen);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownSpeed);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.labelTimeline);
            this.Controls.Add(this.trackBarTimeline);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.windowsMediaPlayer);
            this.Controls.Add(this.buttonPlay);
            this.Name = "FormListener";
            this.ShowInTaskbar = false;
            this.Text = "FormListener";
            this.Load += new System.EventHandler(this.FormListener_Load);
            this.SizeChanged += new System.EventHandler(this.FormListener_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.windowsMediaPlayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTimeline)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonPlay;
        private AxWMPLib.AxWindowsMediaPlayer windowsMediaPlayer;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.TrackBar trackBarTimeline;
        private System.Windows.Forms.Label labelTimeline;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Timer timerRefresh;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.NumericUpDown numericUpDownSpeed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonListen;
        private UCNoteGraph ucNoteGraph;
    }
}