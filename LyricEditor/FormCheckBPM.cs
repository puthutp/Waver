using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LyricEditor
{
    public partial class FormCheckBPM : Form
    {
        public double bpm = 60;
        public double delayLength = 0;
        private List<double> clicks;

        public FormCheckBPM(string mediaLocation)
        {
            InitializeComponent();
            clicks = new List<double>();
            windowsMediaPlayer.settings.autoStart = false;
            windowsMediaPlayer.settings.volume = Program.Volume;
            windowsMediaPlayer.URL = mediaLocation;
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (windowsMediaPlayer.playState == WMPLib.WMPPlayState.wmppsReady || windowsMediaPlayer.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                buttonPlay.Enabled = false;
                buttonStop.Enabled = true;
                buttonTapBeat.Enabled = true;
                windowsMediaPlayer.Ctlcontrols.play();
                textBox1.Clear();
                clicks = new List<double>();
            }
            else if (windowsMediaPlayer.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                windowsMediaPlayer.Ctlcontrols.play();
            }
            else if (windowsMediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                windowsMediaPlayer.Ctlcontrols.pause();
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            windowsMediaPlayer.Ctlcontrols.stop();
            buttonPlay.Enabled = true;
            buttonStop.Enabled = false;
            buttonTapBeat.Enabled = false;
        }

        private void buttonCheckBPM_Click(object sender, EventArgs e)
        {
            double totalBPM = 0;
            for (int i = 1; i < clicks.Count; i++)
                totalBPM += clicks[i] - clicks[i - 1];
            if (clicks.Count > 1)
            {
                bpm = 60 / (totalBPM / (clicks.Count - 1));
                textBoxBPM.Text = bpm.ToString() + " " + totalBPM + " " + (clicks.Count - 1);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            windowsMediaPlayer.Ctlcontrols.stop();
            base.OnClosing(e);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        int x = 0;

        private void buttonTapBeat_Click(object sender, EventArgs e)
        {            
            clicks.Add(windowsMediaPlayer.Ctlcontrols.currentPosition);
            textBox1.AppendText(windowsMediaPlayer.Ctlcontrols.currentPosition + "\n");
            if (clicks.Count > 1)
            {
                // bpm
                double totalBPM = 0;
                for (int i = 1; i < clicks.Count; i++)
                    totalBPM += clicks[i] - clicks[i - 1];
                bpm = Math.Round(60 / (totalBPM / (clicks.Count - 1)), 3);
                textBoxBPM.Text = bpm.ToString();

                // delay
                double freq = 60 / bpm;
                double nearestBeat = GetNearestBeatFromZero(clicks[0], freq);
                double standardDeviations = 0;
                for (int i = 0; i < clicks.Count; i++)
                {
                    standardDeviations += clicks[i] - nearestBeat;
                    nearestBeat += freq;
                }
                delayLength = Math.Round(standardDeviations / clicks.Count, 3);
                textBoxDelay.Text = delayLength.ToString();
                x++;
            }
        }

        private double GetNearestBeatFromZero(double input, double currentFreq)
        {
            double milestone = 0;
            while (input > milestone)
                milestone += currentFreq;
            return milestone;
        }
    }
}
