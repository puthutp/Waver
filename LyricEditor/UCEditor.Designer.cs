namespace LyricEditor
{
    partial class UCEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.placeStartPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteStartPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboBoxFrom = new System.Windows.Forms.ToolStripComboBox();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.placeStartPointToolStripMenuItem,
            this.deleteStartPointToolStripMenuItem,
            this.toolStripComboBoxFrom});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(189, 97);
            // 
            // placeStartPointToolStripMenuItem
            // 
            this.placeStartPointToolStripMenuItem.Name = "placeStartPointToolStripMenuItem";
            this.placeStartPointToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.placeStartPointToolStripMenuItem.Text = "&Place Start Point Here";
            this.placeStartPointToolStripMenuItem.Click += new System.EventHandler(this.placeStartPointToolStripMenuItem_Click);
            // 
            // deleteStartPointToolStripMenuItem
            // 
            this.deleteStartPointToolStripMenuItem.Name = "deleteStartPointToolStripMenuItem";
            this.deleteStartPointToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.deleteStartPointToolStripMenuItem.Text = "&Delete Start Point";
            this.deleteStartPointToolStripMenuItem.Click += new System.EventHandler(this.deleteStartPointToolStripMenuItem_Click);
            // 
            // toolStripComboBoxFrom
            // 
            this.toolStripComboBoxFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxFrom.Items.AddRange(new object[] {
            "PlayFromStartPoint",
            "PlayFromZero"});
            this.toolStripComboBoxFrom.Name = "toolStripComboBoxFrom";
            this.toolStripComboBoxFrom.Size = new System.Drawing.Size(121, 23);
            this.toolStripComboBoxFrom.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxFrom_SelectedIndexChanged);
            // 
            // UCEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UCEditor";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.UCEditor_Paint);
            this.SizeChanged += new System.EventHandler(this.UCEditor_SizeChanged);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem placeStartPointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteStartPointToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxFrom;
    }
}
