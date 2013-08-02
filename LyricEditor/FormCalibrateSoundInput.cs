using GameLibrary.Input.Sound;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LyricEditor
{
    public partial class FormCalibrateSoundInput : Form
    {
        private static int MAX_DATA_COUNT = 600;
        public delegate void UpdateUIHandler(int volume);

        private const int NotifyPerSecond = 20;
        private SoundListener soundListener;
        private List<int> data;

        public int AverageVolumeLevel { get; private set; }

        public FormCalibrateSoundInput()
        {
            InitializeComponent();
            ucNoteGraph.PitchRange = 32000;
            data = new List<int>();
            StartListen();
        }

        public void StartListen()
        {
            if (soundListener == null)
            {
                soundListener = new SoundListener(Program.SoundCaptureDevice);
                soundListener.NotifyPointsInSecond = NotifyPerSecond;
                soundListener.Listen += new SoundListener.ListenHandler(soundListener_Listen);
                soundListener.StartListen();
            }
        }

        public void StopListen()
        {
            if (soundListener != null)
            {
                try
                {
                    soundListener.StopListen();
                    soundListener.Listen -= new SoundListener.ListenHandler(soundListener_Listen);
                    soundListener = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in stopping device,\nError Message: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void soundListener_Listen(object sender, double frequency, int volume)
        {
            data.Add(volume);
            if (data.Count > MAX_DATA_COUNT)
                data.RemoveAt(0);
            try
            {
                Invoke(new UpdateUIHandler(UpdateUI), new object[] { volume });
            }
            catch (Exception) { }
        }

        private void UpdateUI(int volume)
        {
            ucNoteGraph.AddNote(volume);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            StopListen();
            AverageVolumeLevel = (int)data.Average();
            DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            StopListen();
            DialogResult = DialogResult.Cancel;
        }
    }
}
