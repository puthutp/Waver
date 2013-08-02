using GameLibrary.Input.Sound;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace LyricEditor
{
    public partial class FormSong : Form
    {
        private static int MAXIMUM_MIDI_VOLUME = 127;//255;//
        
        private int gridInterval;
        private double delayLength;
        private double bpm;
        private double length;
        private string title;
        private string artist;
        private string genre;
        private string difficulty;
        private string mediaLocation;
        private string minus1Location;
        private string pictureLocation;
        private int unlock;

        private double minimumLength;
        private double midiOffset;
        private SoundCaptureDevice[] soundCaptureDevices;

        private BackgroundWorker bgWorker;

        public FormSong()
        {
            InitializeComponent();
            InitializeObjects();
        }

        public void InitializeObjects()
        {
            // properties
            windowsMediaPlayer.settings.volume = 0;
            windowsMediaPlayer.OpenStateChange += new AxWMPLib._WMPOCXEvents_OpenStateChangeEventHandler(axWindowsMediaPlayer1_OpenStateChange);
            comboBoxGridInterval.Items.AddRange(Program.GridIntervals);
            comboBoxGridInterval.SelectedItem = Program.GridInterval;
            if (Program.MediaLocation != null && Program.MediaLocation != "")
            {
                //textBoxFile.Text = FileManager.GetSafeFileName(Program.MediaLocation);
                textBoxDir.Text = Program.MediaLocation;
            }
            if (Program.Minus1Location != null && Program.Minus1Location != "")
            {
                //textBoxFile.Text = FileManager.GetSafeFileName(Program.MediaLocation);
                textBoxMinus.Text = Program.Minus1Location;
            }
            textBoxArtist.Text = Program.ArtistString;
            textBoxTitle.Text = Program.TitleString;
            textBoxBPM.Text = Program.BeatPerMinute.ToString();
            comboBoxGenre.Text = Program.GenreString;
            textBoxLength.Text = Program.TimelineLength.ToString();
            mediaLocation = Program.MediaLocation;
            minus1Location = Program.Minus1Location;
            textBoxAudioDelay.Text = Program.DelayLength.ToString();
            windowsMediaPlayer.settings.volume = Program.Volume;
            comboBoxDifficulty.Text = Program.Difficulty;
            textBoxUnlock.Text = Program.Unlock.ToString();

            // options
            checkBoxIsAutoApply.Checked = Program.IsAutoApply;
            textBoxMinimumLength.Text = Program.MinimumLength.ToString();
            textBoxMIDIOffset.Text = Program.MIDIPlaybackOffset.ToString();
            trackBar1.Value = Program.Volume;
            trackBarMidiVolume.Value = (int)((double)Program.MIDIVolume / MAXIMUM_MIDI_VOLUME * 100);
            trackBar1_Scroll(trackBar1, EventArgs.Empty);
            trackBarMidiVolume_Scroll(trackBarMidiVolume, EventArgs.Empty);
            //soundCaptureDevices = SoundCaptureDevice.GetDevices();
            //for (int i = 0; i < soundCaptureDevices.Length; i++)
            //    comboBoxDevices.Items.Add(soundCaptureDevices[i].Name);
            //if (soundCaptureDevices.Length > 0 && Program.SoundCaptureDevice != null)
            //    comboBoxDevices.SelectedIndex = comboBoxDevices.Items.IndexOf(Program.SoundCaptureDevice.Name);

            //upload
            pictureLocation = Program.PictureLocation;
            if (Program.PictureLocation != null && Program.PictureLocation != "")
            {
                textBoxPicture.Text = Program.PictureLocation;
            }
            textBoxDBServer.Text = Program.DBServer;
            textBoxFTPServer.Text = Program.FTPServer;
            textBoxFTPUsername.Text = Program.FTPUsername;
            textBoxFTPPassword.Text = Program.FTPPassword;

            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);

        }

        void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                MessageBox.Show("Upload canceled");
            }
            else
            {
                // Finally, handle the case where the operation  
                // succeeded.
                
            }

            if (buttonSubmit.InvokeRequired)
            {
                buttonSubmit.Invoke(new MethodInvoker(delegate { buttonSubmit.Enabled = true; }));
            }
            else
            {
                buttonSubmit.Enabled = true;
            }
        }

        void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (buttonSubmit.InvokeRequired)
            {
                buttonSubmit.Invoke(new MethodInvoker(delegate { buttonSubmit.Enabled = false; }));
            }
            else
            {
                buttonSubmit.Enabled = false;
            }

            int songId = GetSongId();

            bool uploadOk = true;

            bool mediaOk = true;
            bool minus1Ok = true;
            bool pictureOk = true;
            bool songOk = true;

            string message = "";
            if (Program.MediaLocation == "" || Program.MediaLocation == "-")
            {
                mediaOk = false;
                message += "media location is empty\n";
            }
            if (Program.Minus1Location == "" || Program.Minus1Location == "-")
            {
                minus1Ok = false;
                message += "minus1 location is empty\n";
            }
            if (Program.PictureLocation == "" || Program.PictureLocation == "-")
            {
                pictureOk = false;
                message += "thumbnail location is empty\n";
            }
            else
            {
                using (Image img = Image.FromFile(Program.PictureLocation))
                {
                    if (img.Width > 50 || img.Height > 50)
                    {
                        uploadOk = false;
                        MessageBox.Show("max image height and width is 50", "invalid image size", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            if (songId <= 0)
            {
                songOk = false;
                message += "database update failed";
            }


            if (!mediaOk || !minus1Ok || !pictureOk || !songOk)
            {
                uploadOk = false;
                MessageBox.Show(message, "upload failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if (uploadOk)
            {
                int i = 0;
                //for (i = 0; i < 10; i++)
                //{
                try
                {
                    System.Diagnostics.Debug.WriteLine("start upload");
                    FileTransfer.UploadFile(Program.FileLocation, Program.FTPServer, "/assets/songs/", Program.FTPUsername, Program.FTPPassword, (songId + i).ToString(), FileTransfer.EXT_TXT);
                    System.Diagnostics.Debug.WriteLine("txt");
                    string fileExt = Path.GetExtension(Program.MediaLocation);
                    FileTransfer.UploadFile(Program.MediaLocation, Program.FTPServer, "/assets/songs/", Program.FTPUsername, Program.FTPPassword, (songId + i).ToString(), fileExt);
                    System.Diagnostics.Debug.WriteLine("mp31");
                    fileExt = Path.GetExtension(Program.Minus1Location);
                    FileTransfer.UploadFile(Program.Minus1Location, Program.FTPServer, "/assets/songs/", Program.FTPUsername, Program.FTPPassword, (songId + i).ToString(), "-1" + fileExt);
                    System.Diagnostics.Debug.WriteLine("mp32");
                    fileExt = Path.GetExtension(Program.PictureLocation);
                    FileTransfer.UploadFile(Program.PictureLocation, Program.FTPServer, "/assets/songs/", Program.FTPUsername, Program.FTPPassword, (songId + i).ToString(), fileExt);
                    System.Diagnostics.Debug.WriteLine("jpg");
                    MessageBox.Show("Song '" + title + "' Uploaded");
                }
                catch (WebException wex)
                {
                    MessageBox.Show(wex.Message, "Upload File Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //}
            }
        }

        public void SelectPropertiesTab()
        {
            tabControl.SelectedIndex = 0;
        }

        public void SelectOptionsTab()
        {
            tabControl.SelectedIndex = 1;
        }

        public void SelectUploadTab()
        {
            tabControl.SelectedIndex = 2;
        }

        public void SelectIndexTab(int idx)
        {
            tabControl.SelectedIndex = idx;
        }

        public void SetProperties()
        {
            Program.MediaLocation = mediaLocation;
            Program.Minus1Location = minus1Location;
            Program.TimelineLength = length;
            Program.ArtistString = artist;
            Program.TitleString = title;
            Program.GenreString = genre;
            Program.Difficulty = difficulty;
            Program.DelayLength = delayLength;
            Program.Unlock = unlock;
            bpm = Math.Round(bpm, 3);
            Program.SetBeat(bpm, gridInterval);

            Program.IsAutoApply = checkBoxIsAutoApply.Checked;
            Program.MinimumLength = minimumLength;
            Program.AudioRate = (int)numericUpDownAudioRate.Value;
            Program.Volume = trackBar1.Value;
            Program.MIDIVolume = (byte)((double)trackBarMidiVolume.Value / 100 * MAXIMUM_MIDI_VOLUME);
            Program.MIDIPlayer.Volume = Program.MIDIVolume;
            Program.MIDIPlaybackOffset = midiOffset;

            Program.PictureLocation = pictureLocation;
            Program.DBServer = textBoxDBServer.Text;
            Program.FTPServer = textBoxFTPServer.Text;
            Program.FTPUsername = textBoxFTPUsername.Text;
            Program.FTPPassword = textBoxFTPPassword.Text;

            FileManager.CreateSettingFile();

            //if (soundCaptureDevices.Length > 0)
            //    Program.SoundCaptureDevice = soundCaptureDevices[comboBoxDevices.SelectedIndex];
        }

        private bool ValidateAllControls()
        {
            if (!double.TryParse(textBoxBPM.Text, out bpm))
            {
                MessageBox.Show("Beat Per Minute is not a valid double", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                if (bpm <= 0)
                {
                    MessageBox.Show("Beat Per Minute must be larger than 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            if (!double.TryParse(textBoxLength.Text, out length))
            {
                MessageBox.Show("Length is not a valid double", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!double.TryParse(textBoxAudioDelay.Text, out delayLength))
            {
                MessageBox.Show("Audio Delay is not a valid double", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!double.TryParse(textBoxMinimumLength.Text, out minimumLength))
            {
                MessageBox.Show("Minimum timespan is not a valid double", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!double.TryParse(textBoxMIDIOffset.Text, out midiOffset))
            {
                MessageBox.Show("MIDI Playback Offset is not a valid double", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!int.TryParse(textBoxUnlock.Text, out unlock))
            {
                MessageBox.Show("Level Unlock is not a valid number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            title = textBoxTitle.Text;
            artist = textBoxArtist.Text;
            mediaLocation = textBoxDir.Text;
            minus1Location = textBoxMinus.Text;
            genre = comboBoxGenre.Text;
            difficulty = comboBoxDifficulty.Text;
            gridInterval = (int)comboBoxGridInterval.SelectedItem;
            pictureLocation = textBoxPicture.Text;

            return true;
        }

        private void axWindowsMediaPlayer1_OpenStateChange(object sender, AxWMPLib._WMPOCXEvents_OpenStateChangeEvent e)
        {
            if (e.newState == 13)
            {
                length = Math.Round((double)windowsMediaPlayer.Ctlcontrols.currentItem.duration, 2);
                textBoxLength.Text = length.ToString();
                mediaLocation = textBoxDir.Text;
                windowsMediaPlayer.Ctlcontrols.stop();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (ValidateAllControls())
                DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Supported Formats|*.mp3;*.mid;*.wav;*.wma|MP3 File(*.mp3)|*.mp3|MIDI File(*.mid)|*.mid|WAV File(*.wav)|*.wav|WMA File(*.wma)|*.wma";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                windowsMediaPlayer.URL = openFileDialog.FileName;
                //textBoxFile.Text = FileManager.GetSafeFileName(openFileDialog.FileName);
                if (windowsMediaPlayer.Ctlcontrols.currentItem != null)
                    textBoxDir.Text = openFileDialog.FileName;
            }
        }

        private void buttonBrowseMinus_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Supported Formats|*.mp3;*.mid;*.wav;*.wma|MP3 File(*.mp3)|*.mp3|MIDI File(*.mid)|*.mid|WAV File(*.wav)|*.wav|WMA File(*.wma)|*.wma";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //windowsMediaPlayer.URL = openFileDialog.FileName;
                ////textBoxFile.Text = FileManager.GetSafeFileName(openFileDialog.FileName);
                //if (windowsMediaPlayer.Ctlcontrols.currentItem != null)
                    textBoxMinus.Text = openFileDialog.FileName;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            labelMusicVolume.Text = trackBar1.Value.ToString();
        }

        private void trackBarMidiVolume_Scroll(object sender, EventArgs e)
        {
            labelMIDIVolume.Text = trackBarMidiVolume.Value.ToString();
        }

        private void buttonCheckBPM_Click(object sender, EventArgs e)
        {
            if (mediaLocation != null && mediaLocation.Length > 0)
            {
                FormCheckBPM formCheckBPM = new FormCheckBPM(mediaLocation);
                if (formCheckBPM.ShowDialog() == DialogResult.OK)
                {
                    bpm = formCheckBPM.bpm;
                    textBoxBPM.Text = bpm.ToString();
                    delayLength = formCheckBPM.delayLength;
                    textBoxAudioDelay.Text = delayLength.ToString();
                }
            }
        }

        private void buttonBrowse2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JPG File|*.jpg";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxPicture.Text = openFileDialog.FileName;
                Program.PictureLocation = textBoxPicture.Text;
            }
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            if (ValidateAllControls())
            {
                SetProperties();

                SaveManager.Save(this, Program.PitchBars);

                //EditorDBConnect dbConnect = new EditorDBConnect(Program.DBServer, Program.DBName, Program.DBUsername, Program.DBPassword);

                //int songId = dbConnect.GetSongID(Program.ArtistString, Program.TitleString);
                //if (songId > 0)
                //{
                //    dbConnect.ExecuteNonQuery("UPDATE songs SET artist=\"" + Program.ArtistString + "\", title=\"" +
                //        Program.TitleString + "\", genre=\"" + Program.GenreString + "\", duration=\"" + (Program.TimelineLength * 1000) +
                //        "\", rating=\"" + Program.Difficulty + "\" " +
                //        "WHERE id=\"" + songId + "\"");
                //}
                //else
                //{
                //    songId = dbConnect.GetNextID();
                //    if (songId > 0)
                //    {
                //        dbConnect.ExecuteNonQuery("INSERT INTO songs (id, artist, title, genre, duration, rating) VALUES (\"" + songId +
                //            "\", \"" + Program.ArtistString + "\", \"" + Program.TitleString + "\", \"" +
                //            Program.GenreString + "\", \"" + (Program.TimelineLength * 1000) + "\", \"" + 
                //            Program.Difficulty + "\")");
                //    }
                //}

                if (!bgWorker.IsBusy)
                {
                    bgWorker.RunWorkerAsync();
                }

                //int songId = 20;//GetSongId();

                //bool uploadOk = true;

                //bool mediaOk = true;
                //bool minus1Ok = true;
                //bool pictureOk = true;
                //bool songOk = true;

                //string message = "";
                //if (Program.MediaLocation == "" || Program.MediaLocation == "-")
                //{
                //    mediaOk = false;
                //    message += "media location is empty\n";
                //}
                //if (Program.Minus1Location == "" || Program.Minus1Location == "-")
                //{
                //    minus1Ok = false;
                //    message += "minus1 location is empty\n";
                //}
                //if (Program.PictureLocation == "" || Program.PictureLocation == "-")
                //{
                //    pictureOk = false;
                //    message += "thumbnail location is empty\n";
                //}
                //else
                //{
                //    using (Image img = Image.FromFile(Program.PictureLocation))
                //    {
                //        if (img.Width > 50 || img.Height > 50)
                //        {
                //            uploadOk = false;
                //            MessageBox.Show("max image height and width is 50", "invalid image size", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        }
                //    }
                //}

                //if (songId <= 0)
                //{
                //    songOk = false;
                //    message += "database update failed";
                //}
                

                //if (!mediaOk || !minus1Ok || !pictureOk || !songOk)
                //{
                //    uploadOk = false;
                //    MessageBox.Show(message, "upload failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //}
                
                //if (uploadOk)
                //{
                //    //for (int i = 0; i < 10; i++)
                //    //{
                //    //    System.Diagnostics.Debug.WriteLine("start upload");
                //    //    FileTransfer.UploadFile(Program.FileLocation, Program.FTPServer, "/assets/songs/", Program.FTPUsername, Program.FTPPassword, (songId + i).ToString(), FileTransfer.EXT_TXT);
                //    //    System.Diagnostics.Debug.WriteLine("txt");
                //    //    string fileExt = Path.GetExtension(Program.MediaLocation);
                //    //    FileTransfer.UploadFile(Program.MediaLocation, Program.FTPServer, "/assets/songs/", Program.FTPUsername, Program.FTPPassword, (songId + i).ToString(), fileExt);
                //    //    System.Diagnostics.Debug.WriteLine("mp31");
                //    //    fileExt = Path.GetExtension(Program.Minus1Location);
                //    //    FileTransfer.UploadFile(Program.Minus1Location, Program.FTPServer, "/assets/songs/", Program.FTPUsername, Program.FTPPassword, (songId + i).ToString(), "-1" + fileExt);
                //    //    System.Diagnostics.Debug.WriteLine("mp32");
                //    //    fileExt = Path.GetExtension(Program.PictureLocation);
                //    //    FileTransfer.UploadFile(Program.PictureLocation, Program.FTPServer, "/assets/songs/", Program.FTPUsername, Program.FTPPassword, (songId + i).ToString(), fileExt);
                //    //    System.Diagnostics.Debug.WriteLine("jpg");
                //    //    //MessageBox.Show("Uploaded");
                //    //}

                //    if (!bgWorker.IsBusy)
                //    {
                //        bgWorker.RunWorkerAsync(songId);
                //    }

                //}
            }
        }

        //private void UploadFile(string fileName)
        //{
        //    FtpClient ftpClient = new FtpClient(Program.FTPServer, Program.FTPUsername, Program.FTPPassword);
        //    ftpClient.Login();
        //    ftpClient.ChangeDir("assets/songs/");
        //    ftpClient.Upload(fileName);
        //    ftpClient.Close();
        //}

        private int GetSongId()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://" + Program.DBServer + "/index.php/editor/insertSong2/");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            byte[] byteArray = Encoding.ASCII.GetBytes("artist=" + artist + "&title=" + title + "&genre=" + genre + "&duration=" + (Program.TimelineLength * 1000) + "&rating=" + difficulty + "&unlock=" + unlock);

            request.ContentLength = byteArray.Length;

            Stream stream = request.GetRequestStream();
            stream.Write(byteArray, 0, byteArray.Length);
            stream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Decoder d;
            int charLen;
            String szData;

            int length = (int)response.ContentLength;

            byte[] bytes = new byte[length];
            char[] chars = new char[length];

            response.GetResponseStream().Read(bytes, 0, length);

            d = Encoding.UTF8.GetDecoder();
            charLen = d.GetChars(bytes, 0, length, chars, 0);
            szData = new String(chars);

            int retval;
            try
            {
                retval = int.Parse(szData);
            }
            catch
            {
                retval = -1;
            }

            return retval;
        }
    }
}
