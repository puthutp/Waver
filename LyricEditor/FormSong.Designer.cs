namespace LyricEditor
{
    partial class FormSong
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSong));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageProperties = new System.Windows.Forms.TabPage();
            this.buttonBrowseMinus = new System.Windows.Forms.Button();
            this.textBoxMinus = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxAudioDelay = new System.Windows.Forms.TextBox();
            this.comboBoxGridInterval = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonCheckBPM = new System.Windows.Forms.Button();
            this.textBoxLength = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxBPM = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBoxDifficulty = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.comboBoxGenre = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxArtist = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDir = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.tabPageOptions = new System.Windows.Forms.TabPage();
            this.labelMIDIVolume = new System.Windows.Forms.Label();
            this.labelMusicVolume = new System.Windows.Forms.Label();
            this.textBoxMIDIOffset = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.trackBarMidiVolume = new System.Windows.Forms.TrackBar();
            this.label14 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.numericUpDownAudioRate = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.textBoxMinimumLength = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.checkBoxIsAutoApply = new System.Windows.Forms.CheckBox();
            this.tabPageUpload = new System.Windows.Forms.TabPage();
            this.buttonBrowseThumb = new System.Windows.Forms.Button();
            this.textBoxPicture = new System.Windows.Forms.TextBox();
            this.labelThumbnail = new System.Windows.Forms.Label();
            this.buttonSubmit = new System.Windows.Forms.Button();
            this.textBoxFTPPassword = new System.Windows.Forms.TextBox();
            this.textBoxFTPUsername = new System.Windows.Forms.TextBox();
            this.textBoxFTPServer = new System.Windows.Forms.TextBox();
            this.textBoxDBServer = new System.Windows.Forms.TextBox();
            this.labelFTPPassword = new System.Windows.Forms.Label();
            this.labelFTPUsername = new System.Windows.Forms.Label();
            this.labelFTPServer = new System.Windows.Forms.Label();
            this.labelDBServer = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.windowsMediaPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            this.textBoxUnlock = new System.Windows.Forms.TextBox();
            this.tabControl.SuspendLayout();
            this.tabPageProperties.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPageOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMidiVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAudioRate)).BeginInit();
            this.tabPageUpload.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.windowsMediaPlayer)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageProperties);
            this.tabControl.Controls.Add(this.tabPageOptions);
            this.tabControl.Controls.Add(this.tabPageUpload);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(515, 345);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageProperties
            // 
            this.tabPageProperties.BackColor = System.Drawing.Color.Gray;
            this.tabPageProperties.Controls.Add(this.buttonBrowseMinus);
            this.tabPageProperties.Controls.Add(this.textBoxMinus);
            this.tabPageProperties.Controls.Add(this.label9);
            this.tabPageProperties.Controls.Add(this.groupBox2);
            this.tabPageProperties.Controls.Add(this.groupBox1);
            this.tabPageProperties.Controls.Add(this.label1);
            this.tabPageProperties.Controls.Add(this.textBoxDir);
            this.tabPageProperties.Controls.Add(this.buttonBrowse);
            this.tabPageProperties.Location = new System.Drawing.Point(4, 22);
            this.tabPageProperties.Name = "tabPageProperties";
            this.tabPageProperties.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProperties.Size = new System.Drawing.Size(507, 319);
            this.tabPageProperties.TabIndex = 0;
            this.tabPageProperties.Text = "Properties";
            // 
            // buttonBrowseMinus
            // 
            this.buttonBrowseMinus.Location = new System.Drawing.Point(416, 41);
            this.buttonBrowseMinus.Name = "buttonBrowseMinus";
            this.buttonBrowseMinus.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowseMinus.TabIndex = 25;
            this.buttonBrowseMinus.Text = "Browse";
            this.buttonBrowseMinus.UseVisualStyleBackColor = true;
            this.buttonBrowseMinus.Click += new System.EventHandler(this.buttonBrowseMinus_Click);
            // 
            // textBoxMinus
            // 
            this.textBoxMinus.Enabled = false;
            this.textBoxMinus.Location = new System.Drawing.Point(95, 43);
            this.textBoxMinus.Name = "textBoxMinus";
            this.textBoxMinus.Size = new System.Drawing.Size(315, 20);
            this.textBoxMinus.TabIndex = 24;
            this.textBoxMinus.Text = "-";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 46);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 13);
            this.label9.TabIndex = 23;
            this.label9.Text = "Minus One";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.textBoxAudioDelay);
            this.groupBox2.Controls.Add(this.comboBoxGridInterval);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.buttonCheckBPM);
            this.groupBox2.Controls.Add(this.textBoxLength);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBoxBPM);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(16, 82);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(233, 199);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Property";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 130);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "AudioDelay";
            // 
            // textBoxAudioDelay
            // 
            this.textBoxAudioDelay.Location = new System.Drawing.Point(127, 127);
            this.textBoxAudioDelay.Name = "textBoxAudioDelay";
            this.textBoxAudioDelay.Size = new System.Drawing.Size(100, 20);
            this.textBoxAudioDelay.TabIndex = 11;
            this.textBoxAudioDelay.Text = "0.00";
            // 
            // comboBoxGridInterval
            // 
            this.comboBoxGridInterval.FormattingEnabled = true;
            this.comboBoxGridInterval.Location = new System.Drawing.Point(127, 100);
            this.comboBoxGridInterval.Name = "comboBoxGridInterval";
            this.comboBoxGridInterval.Size = new System.Drawing.Size(100, 21);
            this.comboBoxGridInterval.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "GridInterval";
            // 
            // buttonCheckBPM
            // 
            this.buttonCheckBPM.Location = new System.Drawing.Point(152, 71);
            this.buttonCheckBPM.Name = "buttonCheckBPM";
            this.buttonCheckBPM.Size = new System.Drawing.Size(75, 23);
            this.buttonCheckBPM.TabIndex = 7;
            this.buttonCheckBPM.Text = "CheckBPM";
            this.buttonCheckBPM.UseVisualStyleBackColor = true;
            this.buttonCheckBPM.Click += new System.EventHandler(this.buttonCheckBPM_Click);
            // 
            // textBoxLength
            // 
            this.textBoxLength.BackColor = System.Drawing.SystemColors.HighlightText;
            this.textBoxLength.Location = new System.Drawing.Point(127, 19);
            this.textBoxLength.Name = "textBoxLength";
            this.textBoxLength.ReadOnly = true;
            this.textBoxLength.Size = new System.Drawing.Size(100, 20);
            this.textBoxLength.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "BeatPerMinute";
            // 
            // textBoxBPM
            // 
            this.textBoxBPM.Location = new System.Drawing.Point(127, 45);
            this.textBoxBPM.Name = "textBoxBPM";
            this.textBoxBPM.Size = new System.Drawing.Size(100, 20);
            this.textBoxBPM.TabIndex = 5;
            this.textBoxBPM.Text = "60";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Length (secs)";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxUnlock);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.comboBoxDifficulty);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.comboBoxGenre);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBoxTitle);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBoxArtist);
            this.groupBox1.Location = new System.Drawing.Point(255, 82);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(236, 199);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Song Info";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(21, 103);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(56, 13);
            this.label11.TabIndex = 16;
            this.label11.Text = "Lv Unlock";
            // 
            // comboBoxDifficulty
            // 
            this.comboBoxDifficulty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDifficulty.FormattingEnabled = true;
            this.comboBoxDifficulty.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.comboBoxDifficulty.Location = new System.Drawing.Point(79, 127);
            this.comboBoxDifficulty.Name = "comboBoxDifficulty";
            this.comboBoxDifficulty.Size = new System.Drawing.Size(151, 21);
            this.comboBoxDifficulty.TabIndex = 15;
            this.comboBoxDifficulty.TabStop = false;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(21, 130);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(47, 13);
            this.label17.TabIndex = 14;
            this.label17.Text = "Difficulty";
            // 
            // comboBoxGenre
            // 
            this.comboBoxGenre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGenre.FormattingEnabled = true;
            this.comboBoxGenre.Items.AddRange(new object[] {
            "Unknown",
            "Pop",
            "Rock",
            "Hip Hop",
            "Jazz",
            "Blues",
            "RnB",
            "Reggae",
            "Other"});
            this.comboBoxGenre.Location = new System.Drawing.Point(79, 71);
            this.comboBoxGenre.Name = "comboBoxGenre";
            this.comboBoxGenre.Size = new System.Drawing.Size(151, 21);
            this.comboBoxGenre.TabIndex = 13;
            this.comboBoxGenre.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Genre";
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Location = new System.Drawing.Point(80, 19);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(150, 20);
            this.textBoxTitle.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Artist";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Title";
            // 
            // textBoxArtist
            // 
            this.textBoxArtist.Location = new System.Drawing.Point(80, 45);
            this.textBoxArtist.Name = "textBoxArtist";
            this.textBoxArtist.Size = new System.Drawing.Size(150, 20);
            this.textBoxArtist.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Location";
            // 
            // textBoxDir
            // 
            this.textBoxDir.Enabled = false;
            this.textBoxDir.Location = new System.Drawing.Point(95, 17);
            this.textBoxDir.Name = "textBoxDir";
            this.textBoxDir.Size = new System.Drawing.Size(315, 20);
            this.textBoxDir.TabIndex = 16;
            this.textBoxDir.Text = "-";
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(416, 15);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowse.TabIndex = 15;
            this.buttonBrowse.Text = "Browse";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // tabPageOptions
            // 
            this.tabPageOptions.BackColor = System.Drawing.Color.Gray;
            this.tabPageOptions.Controls.Add(this.labelMIDIVolume);
            this.tabPageOptions.Controls.Add(this.labelMusicVolume);
            this.tabPageOptions.Controls.Add(this.textBoxMIDIOffset);
            this.tabPageOptions.Controls.Add(this.label10);
            this.tabPageOptions.Controls.Add(this.label13);
            this.tabPageOptions.Controls.Add(this.trackBarMidiVolume);
            this.tabPageOptions.Controls.Add(this.label14);
            this.tabPageOptions.Controls.Add(this.trackBar1);
            this.tabPageOptions.Controls.Add(this.numericUpDownAudioRate);
            this.tabPageOptions.Controls.Add(this.label15);
            this.tabPageOptions.Controls.Add(this.textBoxMinimumLength);
            this.tabPageOptions.Controls.Add(this.label16);
            this.tabPageOptions.Controls.Add(this.checkBoxIsAutoApply);
            this.tabPageOptions.Location = new System.Drawing.Point(4, 22);
            this.tabPageOptions.Name = "tabPageOptions";
            this.tabPageOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOptions.Size = new System.Drawing.Size(507, 319);
            this.tabPageOptions.TabIndex = 1;
            this.tabPageOptions.Text = "Options";
            // 
            // labelMIDIVolume
            // 
            this.labelMIDIVolume.AutoSize = true;
            this.labelMIDIVolume.Location = new System.Drawing.Point(321, 146);
            this.labelMIDIVolume.Name = "labelMIDIVolume";
            this.labelMIDIVolume.Size = new System.Drawing.Size(13, 13);
            this.labelMIDIVolume.TabIndex = 36;
            this.labelMIDIVolume.Text = "0";
            // 
            // labelMusicVolume
            // 
            this.labelMusicVolume.AutoSize = true;
            this.labelMusicVolume.Location = new System.Drawing.Point(321, 119);
            this.labelMusicVolume.Name = "labelMusicVolume";
            this.labelMusicVolume.Size = new System.Drawing.Size(13, 13);
            this.labelMusicVolume.TabIndex = 35;
            this.labelMusicVolume.Text = "0";
            // 
            // textBoxMIDIOffset
            // 
            this.textBoxMIDIOffset.Location = new System.Drawing.Point(213, 63);
            this.textBoxMIDIOffset.Name = "textBoxMIDIOffset";
            this.textBoxMIDIOffset.Size = new System.Drawing.Size(86, 20);
            this.textBoxMIDIOffset.TabIndex = 34;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(17, 66);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(162, 13);
            this.label10.TabIndex = 33;
            this.label10.Text = "MIDIPlaybackOffset (in seconds)";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(17, 146);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(68, 13);
            this.label13.TabIndex = 31;
            this.label13.Text = "MIDI Volume";
            // 
            // trackBarMidiVolume
            // 
            this.trackBarMidiVolume.LargeChange = 10;
            this.trackBarMidiVolume.Location = new System.Drawing.Point(114, 146);
            this.trackBarMidiVolume.Maximum = 100;
            this.trackBarMidiVolume.Name = "trackBarMidiVolume";
            this.trackBarMidiVolume.Size = new System.Drawing.Size(201, 45);
            this.trackBarMidiVolume.TabIndex = 30;
            this.trackBarMidiVolume.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarMidiVolume.Value = 100;
            this.trackBarMidiVolume.Scroll += new System.EventHandler(this.trackBarMidiVolume_Scroll);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(17, 119);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(73, 13);
            this.label14.TabIndex = 29;
            this.label14.Text = "Music Volume";
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 10;
            this.trackBar1.Location = new System.Drawing.Point(114, 114);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(201, 45);
            this.trackBar1.TabIndex = 28;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.Value = 100;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // numericUpDownAudioRate
            // 
            this.numericUpDownAudioRate.Location = new System.Drawing.Point(213, 37);
            this.numericUpDownAudioRate.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.numericUpDownAudioRate.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownAudioRate.Name = "numericUpDownAudioRate";
            this.numericUpDownAudioRate.Size = new System.Drawing.Size(86, 20);
            this.numericUpDownAudioRate.TabIndex = 27;
            this.numericUpDownAudioRate.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(17, 39);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(83, 13);
            this.label15.TabIndex = 26;
            this.label15.Text = "MusicSpeed (%)";
            // 
            // textBoxMinimumLength
            // 
            this.textBoxMinimumLength.Location = new System.Drawing.Point(213, 10);
            this.textBoxMinimumLength.Name = "textBoxMinimumLength";
            this.textBoxMinimumLength.Size = new System.Drawing.Size(86, 20);
            this.textBoxMinimumLength.TabIndex = 25;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(17, 13);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(181, 13);
            this.label16.TabIndex = 24;
            this.label16.Text = "PitchBarMinimumLength (in seconds)";
            // 
            // checkBoxIsAutoApply
            // 
            this.checkBoxIsAutoApply.AutoSize = true;
            this.checkBoxIsAutoApply.Location = new System.Drawing.Point(417, 13);
            this.checkBoxIsAutoApply.Name = "checkBoxIsAutoApply";
            this.checkBoxIsAutoApply.Size = new System.Drawing.Size(82, 17);
            this.checkBoxIsAutoApply.TabIndex = 23;
            this.checkBoxIsAutoApply.Text = "IsAutoApply";
            this.checkBoxIsAutoApply.UseVisualStyleBackColor = true;
            // 
            // tabPageUpload
            // 
            this.tabPageUpload.BackColor = System.Drawing.Color.Gray;
            this.tabPageUpload.Controls.Add(this.buttonBrowseThumb);
            this.tabPageUpload.Controls.Add(this.textBoxPicture);
            this.tabPageUpload.Controls.Add(this.labelThumbnail);
            this.tabPageUpload.Controls.Add(this.buttonSubmit);
            this.tabPageUpload.Controls.Add(this.textBoxFTPPassword);
            this.tabPageUpload.Controls.Add(this.textBoxFTPUsername);
            this.tabPageUpload.Controls.Add(this.textBoxFTPServer);
            this.tabPageUpload.Controls.Add(this.textBoxDBServer);
            this.tabPageUpload.Controls.Add(this.labelFTPPassword);
            this.tabPageUpload.Controls.Add(this.labelFTPUsername);
            this.tabPageUpload.Controls.Add(this.labelFTPServer);
            this.tabPageUpload.Controls.Add(this.labelDBServer);
            this.tabPageUpload.Location = new System.Drawing.Point(4, 22);
            this.tabPageUpload.Name = "tabPageUpload";
            this.tabPageUpload.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUpload.Size = new System.Drawing.Size(507, 319);
            this.tabPageUpload.TabIndex = 2;
            this.tabPageUpload.Text = "Upload";
            // 
            // buttonBrowseThumb
            // 
            this.buttonBrowseThumb.Location = new System.Drawing.Point(403, 224);
            this.buttonBrowseThumb.Name = "buttonBrowseThumb";
            this.buttonBrowseThumb.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowseThumb.TabIndex = 17;
            this.buttonBrowseThumb.Text = "Browse";
            this.buttonBrowseThumb.UseVisualStyleBackColor = true;
            this.buttonBrowseThumb.Click += new System.EventHandler(this.buttonBrowse2_Click);
            // 
            // textBoxPicture
            // 
            this.textBoxPicture.Enabled = false;
            this.textBoxPicture.Location = new System.Drawing.Point(161, 226);
            this.textBoxPicture.Name = "textBoxPicture";
            this.textBoxPicture.Size = new System.Drawing.Size(236, 20);
            this.textBoxPicture.TabIndex = 16;
            // 
            // labelThumbnail
            // 
            this.labelThumbnail.AutoSize = true;
            this.labelThumbnail.Location = new System.Drawing.Point(35, 229);
            this.labelThumbnail.Name = "labelThumbnail";
            this.labelThumbnail.Size = new System.Drawing.Size(56, 13);
            this.labelThumbnail.TabIndex = 15;
            this.labelThumbnail.Text = "Thumbnail";
            // 
            // buttonSubmit
            // 
            this.buttonSubmit.Location = new System.Drawing.Point(215, 253);
            this.buttonSubmit.Name = "buttonSubmit";
            this.buttonSubmit.Size = new System.Drawing.Size(101, 31);
            this.buttonSubmit.TabIndex = 14;
            this.buttonSubmit.Text = "Submit";
            this.buttonSubmit.UseVisualStyleBackColor = true;
            this.buttonSubmit.Click += new System.EventHandler(this.buttonSubmit_Click);
            // 
            // textBoxFTPPassword
            // 
            this.textBoxFTPPassword.Location = new System.Drawing.Point(161, 142);
            this.textBoxFTPPassword.Name = "textBoxFTPPassword";
            this.textBoxFTPPassword.Size = new System.Drawing.Size(236, 20);
            this.textBoxFTPPassword.TabIndex = 13;
            // 
            // textBoxFTPUsername
            // 
            this.textBoxFTPUsername.Location = new System.Drawing.Point(161, 117);
            this.textBoxFTPUsername.Name = "textBoxFTPUsername";
            this.textBoxFTPUsername.Size = new System.Drawing.Size(236, 20);
            this.textBoxFTPUsername.TabIndex = 12;
            // 
            // textBoxFTPServer
            // 
            this.textBoxFTPServer.Location = new System.Drawing.Point(161, 92);
            this.textBoxFTPServer.Name = "textBoxFTPServer";
            this.textBoxFTPServer.Size = new System.Drawing.Size(236, 20);
            this.textBoxFTPServer.TabIndex = 11;
            // 
            // textBoxDBServer
            // 
            this.textBoxDBServer.Location = new System.Drawing.Point(161, 42);
            this.textBoxDBServer.Name = "textBoxDBServer";
            this.textBoxDBServer.Size = new System.Drawing.Size(236, 20);
            this.textBoxDBServer.TabIndex = 7;
            // 
            // labelFTPPassword
            // 
            this.labelFTPPassword.AutoSize = true;
            this.labelFTPPassword.Location = new System.Drawing.Point(35, 145);
            this.labelFTPPassword.Name = "labelFTPPassword";
            this.labelFTPPassword.Size = new System.Drawing.Size(76, 13);
            this.labelFTPPassword.TabIndex = 6;
            this.labelFTPPassword.Text = "FTP Password";
            // 
            // labelFTPUsername
            // 
            this.labelFTPUsername.AutoSize = true;
            this.labelFTPUsername.Location = new System.Drawing.Point(35, 120);
            this.labelFTPUsername.Name = "labelFTPUsername";
            this.labelFTPUsername.Size = new System.Drawing.Size(78, 13);
            this.labelFTPUsername.TabIndex = 5;
            this.labelFTPUsername.Text = "FTP Username";
            // 
            // labelFTPServer
            // 
            this.labelFTPServer.AutoSize = true;
            this.labelFTPServer.Location = new System.Drawing.Point(35, 95);
            this.labelFTPServer.Name = "labelFTPServer";
            this.labelFTPServer.Size = new System.Drawing.Size(61, 13);
            this.labelFTPServer.TabIndex = 4;
            this.labelFTPServer.Text = "FTP Server";
            // 
            // labelDBServer
            // 
            this.labelDBServer.AutoSize = true;
            this.labelDBServer.Location = new System.Drawing.Point(35, 45);
            this.labelDBServer.Name = "labelDBServer";
            this.labelDBServer.Size = new System.Drawing.Size(56, 13);
            this.labelDBServer.TabIndex = 0;
            this.labelDBServer.Text = "DB Server";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(428, 351);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(347, 351);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // windowsMediaPlayer
            // 
            this.windowsMediaPlayer.Enabled = true;
            this.windowsMediaPlayer.Location = new System.Drawing.Point(283, 351);
            this.windowsMediaPlayer.Name = "windowsMediaPlayer";
            this.windowsMediaPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("windowsMediaPlayer.OcxState")));
            this.windowsMediaPlayer.Size = new System.Drawing.Size(58, 23);
            this.windowsMediaPlayer.TabIndex = 19;
            this.windowsMediaPlayer.Visible = false;
            // 
            // textBoxUnlock
            // 
            this.textBoxUnlock.Location = new System.Drawing.Point(79, 100);
            this.textBoxUnlock.Name = "textBoxUnlock";
            this.textBoxUnlock.Size = new System.Drawing.Size(150, 20);
            this.textBoxUnlock.TabIndex = 17;
            // 
            // FormSong
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(515, 386);
            this.Controls.Add(this.windowsMediaPlayer);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormSong";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.tabControl.ResumeLayout(false);
            this.tabPageProperties.ResumeLayout(false);
            this.tabPageProperties.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPageOptions.ResumeLayout(false);
            this.tabPageOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMidiVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAudioRate)).EndInit();
            this.tabPageUpload.ResumeLayout(false);
            this.tabPageUpload.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.windowsMediaPlayer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageProperties;
        private System.Windows.Forms.TabPage tabPageOptions;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label8;
        internal System.Windows.Forms.TextBox textBoxAudioDelay;
        private System.Windows.Forms.ComboBox comboBoxGridInterval;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonCheckBPM;
        internal System.Windows.Forms.TextBox textBoxLength;
        private System.Windows.Forms.Label label2;
        internal System.Windows.Forms.TextBox textBoxBPM;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        internal System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        internal System.Windows.Forms.TextBox textBoxArtist;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDir;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Label labelMIDIVolume;
        private System.Windows.Forms.Label labelMusicVolume;
        private System.Windows.Forms.TextBox textBoxMIDIOffset;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TrackBar trackBarMidiVolume;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.NumericUpDown numericUpDownAudioRate;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBoxMinimumLength;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.CheckBox checkBoxIsAutoApply;
        private AxWMPLib.AxWindowsMediaPlayer windowsMediaPlayer;
        private System.Windows.Forms.TabPage tabPageUpload;
        private System.Windows.Forms.TextBox textBoxDBServer;
        private System.Windows.Forms.Label labelFTPPassword;
        private System.Windows.Forms.Label labelFTPUsername;
        private System.Windows.Forms.Label labelFTPServer;
        private System.Windows.Forms.Label labelDBServer;
        private System.Windows.Forms.TextBox textBoxFTPPassword;
        private System.Windows.Forms.TextBox textBoxFTPUsername;
        private System.Windows.Forms.TextBox textBoxFTPServer;
        private System.Windows.Forms.Button buttonSubmit;
        private System.Windows.Forms.TextBox textBoxPicture;
        private System.Windows.Forms.Label labelThumbnail;
        private System.Windows.Forms.Button buttonBrowseThumb;
        private System.Windows.Forms.ComboBox comboBoxGenre;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button buttonBrowseMinus;
        private System.Windows.Forms.TextBox textBoxMinus;
        private System.Windows.Forms.ComboBox comboBoxDifficulty;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label11;
        internal System.Windows.Forms.TextBox textBoxUnlock;
    }
}