﻿namespace NTranslate
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
            this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._comment = new System.Windows.Forms.TextBox();
            this._commentLabel = new System.Windows.Forms.Label();
            this._originalSourceLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._name = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._panel = new System.Windows.Forms.Panel();
            this._translated = new NTranslate.SpellCheck.SpellCheckTextBox();
            this._source = new NTranslate.SpellCheck.RichTextBox();
            this._originalSource = new NTranslate.SpellCheck.RichTextBox();
            this._tableLayoutPanel.SuspendLayout();
            this._panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tableLayoutPanel
            // 
            this._tableLayoutPanel.AutoSize = true;
            this._tableLayoutPanel.ColumnCount = 2;
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel.Controls.Add(this._comment, 1, 4);
            this._tableLayoutPanel.Controls.Add(this._commentLabel, 0, 4);
            this._tableLayoutPanel.Controls.Add(this._originalSourceLabel, 0, 2);
            this._tableLayoutPanel.Controls.Add(this.label1, 0, 0);
            this._tableLayoutPanel.Controls.Add(this.label3, 0, 1);
            this._tableLayoutPanel.Controls.Add(this._name, 1, 0);
            this._tableLayoutPanel.Controls.Add(this.label2, 0, 3);
            this._tableLayoutPanel.Controls.Add(this._translated, 1, 3);
            this._tableLayoutPanel.Controls.Add(this._source, 1, 1);
            this._tableLayoutPanel.Controls.Add(this._originalSource, 1, 2);
            this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._tableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this._tableLayoutPanel.Name = "_tableLayoutPanel";
            this._tableLayoutPanel.RowCount = 5;
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.Size = new System.Drawing.Size(385, 126);
            this._tableLayoutPanel.TabIndex = 0;
            this._tableLayoutPanel.SizeChanged += new System.EventHandler(this._tableLayoutPanel_SizeChanged);
            // 
            // _comment
            // 
            this._comment.Dock = System.Windows.Forms.DockStyle.Fill;
            this._comment.Location = new System.Drawing.Point(89, 103);
            this._comment.Name = "_comment";
            this._comment.ReadOnly = true;
            this._comment.Size = new System.Drawing.Size(293, 20);
            this._comment.TabIndex = 9;
            this._comment.TabStop = false;
            // 
            // _commentLabel
            // 
            this._commentLabel.AutoSize = true;
            this._commentLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._commentLabel.Location = new System.Drawing.Point(3, 100);
            this._commentLabel.Name = "_commentLabel";
            this._commentLabel.Size = new System.Drawing.Size(80, 26);
            this._commentLabel.TabIndex = 8;
            this._commentLabel.Text = "Comment:";
            this._commentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _originalSourceLabel
            // 
            this._originalSourceLabel.AutoSize = true;
            this._originalSourceLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._originalSourceLabel.Location = new System.Drawing.Point(3, 46);
            this._originalSourceLabel.Name = "_originalSourceLabel";
            this._originalSourceLabel.Size = new System.Drawing.Size(80, 27);
            this._originalSourceLabel.TabIndex = 4;
            this._originalSourceLabel.Text = "Original source:";
            this._originalSourceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.label3.Size = new System.Drawing.Size(80, 27);
            this.label3.TabIndex = 2;
            this.label3.Text = "Source:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _name
            // 
            this._name.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._name.Dock = System.Windows.Forms.DockStyle.Fill;
            this._name.Location = new System.Drawing.Point(89, 3);
            this._name.Name = "_name";
            this._name.ReadOnly = true;
            this._name.Size = new System.Drawing.Size(293, 13);
            this._name.TabIndex = 1;
            this._name.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 27);
            this.label2.TabIndex = 6;
            this.label2.Text = "Translated:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _panel
            // 
            this._panel.Controls.Add(this._tableLayoutPanel);
            this._panel.Dock = System.Windows.Forms.DockStyle.Top;
            this._panel.Location = new System.Drawing.Point(0, 0);
            this._panel.Name = "_panel";
            this._panel.Padding = new System.Windows.Forms.Padding(3);
            this._panel.Size = new System.Drawing.Size(391, 170);
            this._panel.TabIndex = 1;
            // 
            // _translated
            // 
            this._translated.Dock = System.Windows.Forms.DockStyle.Fill;
            this._translated.Location = new System.Drawing.Point(89, 76);
            this._translated.Name = "_translated";
            this._translated.Size = new System.Drawing.Size(293, 21);
            this._translated.SizeToContent = true;
            this._translated.TabIndex = 7;
            this._translated.Text = "";
            this._translated.TextChanged += new System.EventHandler(this._translated_TextChanged);
            // 
            // _source
            // 
            this._source.BackColor = System.Drawing.SystemColors.Control;
            this._source.Dock = System.Windows.Forms.DockStyle.Fill;
            this._source.Location = new System.Drawing.Point(89, 22);
            this._source.Name = "_source";
            this._source.ReadOnly = true;
            this._source.Size = new System.Drawing.Size(293, 21);
            this._source.TabIndex = 10;
            this._source.TabStop = false;
            this._source.Text = "";
            // 
            // _originalSource
            // 
            this._originalSource.BackColor = System.Drawing.SystemColors.Control;
            this._originalSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this._originalSource.Location = new System.Drawing.Point(89, 49);
            this._originalSource.Name = "_originalSource";
            this._originalSource.ReadOnly = true;
            this._originalSource.Size = new System.Drawing.Size(293, 21);
            this._originalSource.TabIndex = 10;
            this._originalSource.TabStop = false;
            this._originalSource.Text = "";
            // 
            // ResourceNodeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this._panel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ResourceNodeControl";
            this.Size = new System.Drawing.Size(391, 209);
            this._tableLayoutPanel.ResumeLayout(false);
            this._tableLayoutPanel.PerformLayout();
            this._panel.ResumeLayout(false);
            this._panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _name;
        private System.Windows.Forms.Label label2;
        private SpellCheck.SpellCheckTextBox _translated;
        private System.Windows.Forms.Label _originalSourceLabel;
        private System.Windows.Forms.Panel _panel;
        private System.Windows.Forms.TextBox _comment;
        private System.Windows.Forms.Label _commentLabel;
        private SpellCheck.RichTextBox _source;
        private SpellCheck.RichTextBox _originalSource;
    }
}
