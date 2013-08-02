using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LyricEditor
{
    class SaveManager
    {
        public static bool isDirty;
        public static bool hasSavedToRecent;

        public static void Save(IWin32Window window, List<PitchBar> sortedPitchBar)
        {
            if (Program.FileLocation == null || Program.FileLocation == "")
            {
                SaveAs(window, sortedPitchBar);
            }
            else
            {
                ActualSave(sortedPitchBar);
            }
        }

        public static void SaveAs(IWin32Window window, List<PitchBar> sortedPitchBar)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.Filter = "Lyric File|*.lrc";
            if (saveFileDialog.ShowDialog(window) == DialogResult.OK)
            {
                Program.FileLocation = saveFileDialog.FileName;
                ActualSave(sortedPitchBar);
            }
        }

        private static void ActualSave(List<PitchBar> sortedPitchBar)
        {
            Program.ProcessGroupAndWordIndex(sortedPitchBar);
            FileManager.SaveToFile(Program.FileLocation);
            isDirty = false;
            if (!hasSavedToRecent)
            {
                FileManager.AppendToRecent(Program.FileLocation);
                hasSavedToRecent = true;
            }
        }
    }
}
