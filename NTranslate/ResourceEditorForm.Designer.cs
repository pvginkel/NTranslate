namespace NTranslate
{
    partial class ResourceEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResourceEditorForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._showAll = new System.Windows.Forms.ToolStripButton();
            this._showPending = new System.Windows.Forms.ToolStripButton();
            this._showMissing = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._showHidden = new System.Windows.Forms.ToolStripButton();
            this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._showAll,
            this._showPending,
            this._showMissing,
            this.toolStripSeparator1,
            this._showHidden});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(558, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // _showAll
            // 
            this._showAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._showAll.Image = ((System.Drawing.Image)(resources.GetObject("_showAll.Image")));
            this._showAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._showAll.Name = "_showAll";
            this._showAll.Size = new System.Drawing.Size(57, 22);
            this._showAll.Text = "Show All";
            this._showAll.Click += new System.EventHandler(this._showAll_Click);
            // 
            // _showPending
            // 
            this._showPending.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._showPending.Image = ((System.Drawing.Image)(resources.GetObject("_showPending.Image")));
            this._showPending.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._showPending.Name = "_showPending";
            this._showPending.Size = new System.Drawing.Size(87, 22);
            this._showPending.Text = "Show Pending";
            this._showPending.Click += new System.EventHandler(this._showPending_Click);
            // 
            // _showMissing
            // 
            this._showMissing.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._showMissing.Image = ((System.Drawing.Image)(resources.GetObject("_showMissing.Image")));
            this._showMissing.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._showMissing.Name = "_showMissing";
            this._showMissing.Size = new System.Drawing.Size(84, 22);
            this._showMissing.Text = "Show Missing";
            this._showMissing.Click += new System.EventHandler(this._showMissing_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // _showHidden
            // 
            this._showHidden.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._showHidden.Image = ((System.Drawing.Image)(resources.GetObject("_showHidden.Image")));
            this._showHidden.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._showHidden.Name = "_showHidden";
            this._showHidden.Size = new System.Drawing.Size(82, 22);
            this._showHidden.Text = "Show Hidden";
            this._showHidden.Click += new System.EventHandler(this._showHidden_Click);
            // 
            // _tableLayoutPanel
            // 
            this._tableLayoutPanel.AutoSize = true;
            this._tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._tableLayoutPanel.ColumnCount = 1;
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel.Name = "_tableLayoutPanel";
            this._tableLayoutPanel.RowCount = 1;
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.Size = new System.Drawing.Size(558, 0);
            this._tableLayoutPanel.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this._tableLayoutPanel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(558, 417);
            this.panel1.TabIndex = 0;
            // 
            // ResourceEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 442);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ResourceEditorForm";
            this.Text = "Resource Editor";
            this.Shown += new System.EventHandler(this.ResourceEditorForm_Shown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
        private System.Windows.Forms.ToolStripButton _showAll;
        private System.Windows.Forms.ToolStripButton _showPending;
        private System.Windows.Forms.ToolStripButton _showMissing;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton _showHidden;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}