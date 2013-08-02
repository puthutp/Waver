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
    public partial class FormWelcome : Form
    {
        public string FileName { get; private set; }
        private string[] RecentFiles;

        public FormWelcome()
        {
            InitializeComponent();
            bool succeedGetRecent = FileManager.GetRecentFile(out RecentFiles);
            if (succeedGetRecent)
            {
                for (int i = RecentFiles.Length - 1; i >= 0; i--)
                    listBoxRecent.Items.Add(FileManager.GetSafeFileName(RecentFiles[i]));
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBoxRecent.SelectedIndex >= 0 && listBoxRecent.SelectedIndex < RecentFiles.Length)
            {
                FileName = RecentFiles[RecentFiles.Length - 1 - listBoxRecent.SelectedIndex];
                DialogResult = DialogResult.OK;
            }
            else
                DialogResult = DialogResult.Abort;
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Lyric File|*.lrc";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileName = openFileDialog.FileName;
                DialogResult = DialogResult.OK;
            }
        }
    }
}
