namespace NTranslate.App
{
    partial class ResourceNodeControl
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._source = new System.Windows.Forms.TextBox();
            this._originalSourceLabel = new System.Windows.Forms.Label();
            this._translated = new System.Windows.Forms.TextBox();
            this._name = new System.Windows.Forms.TextBox();
            this._originalSource = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this._originalSourceLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._source, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this._translated, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this._name, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this._originalSource, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(192, 97);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 26);
            this.label3.TabIndex = 2;
            this.label3.Text = "Source:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _source
            // 
            this._source.Dock = System.Windows.Forms.DockStyle.Fill;
            this._source.Location = new System.Drawing.Point(89, 22);
            this._source.Name = "_source";
            this._source.ReadOnly = true;
            this._source.Size = new System.Drawing.Size(100, 20);
            this._source.TabIndex = 3;
            this._source.TabStop = false;
            // 
            // _originalSourceLabel
            // 
            this._originalSourceLabel.AutoSize = true;
            this._originalSourceLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._originalSourceLabel.Location = new System.Drawing.Point(3, 45);
            this._originalSourceLabel.Name = "_originalSourceLabel";
            this._originalSourceLabel.Size = new System.Drawing.Size(80, 26);
            this._originalSourceLabel.TabIndex = 4;
            this._originalSourceLabel.Text = "Original source:";
            this._originalSourceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _translated
            // 
            this._translated.Dock = System.Windows.Forms.DockStyle.Fill;
            this._translated.Location = new System.Drawing.Point(89, 74);
            this._translated.Name = "_translated";
            this._translated.Size = new System.Drawing.Size(100, 20);
            this._translated.TabIndex = 5;
            // 
            // _name
            // 
            this._name.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._name.Dock = System.Windows.Forms.DockStyle.Fill;
            this._name.Location = new System.Drawing.Point(89, 3);
            this._name.Name = "_name";
            this._name.ReadOnly = true;
            this._name.Size = new System.Drawing.Size(100, 13);
            this._name.TabIndex = 1;
            this._name.TabStop = false;
            // 
            // _originalSource
            // 
            this._originalSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this._originalSource.Location = new System.Drawing.Point(89, 48);
            this._originalSource.Name = "_originalSource";
            this._originalSource.ReadOnly = true;
            this._originalSource.Size = new System.Drawing.Size(100, 20);
            this._originalSource.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 26);
            this.label2.TabIndex = 7;
            this.label2.Text = "Translated:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ResourceNodeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ResourceNodeControl";
            this.Size = new System.Drawing.Size(195, 100);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label _originalSourceLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _source;
        private System.Windows.Forms.TextBox _translated;
        private System.Windows.Forms.TextBox _name;
        private System.Windows.Forms.TextBox _originalSource;
        private System.Windows.Forms.Label label2;
    }
}
