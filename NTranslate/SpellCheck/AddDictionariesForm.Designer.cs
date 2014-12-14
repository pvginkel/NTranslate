using System;
using System.Collections.Generic;
using System.Text;

namespace NTranslate.SpellCheck
{
    partial class AddDictionariesForm
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
            this.formHeader1 = new SystemEx.Windows.Forms.FormHeader();
            this._webBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // formHeader1
            // 
            this.formHeader1.Location = new System.Drawing.Point(0, 0);
            this.formHeader1.Name = "formHeader1";
            this.formHeader1.Size = new System.Drawing.Size(781, 47);
            this.formHeader1.SubText = "NTranslate uses Firefox dictionaries. Browse to the dictionary you would like to " +
    "add and download it. NTranslate will take care of the rest.";
            this.formHeader1.TabIndex = 0;
            this.formHeader1.Text = "Add a new dictionary to NTranslate";
            // 
            // _webBrowser
            // 
            this._webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this._webBrowser.Location = new System.Drawing.Point(0, 47);
            this._webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this._webBrowser.Name = "_webBrowser";
            this._webBrowser.ScriptErrorsSuppressed = true;
            this._webBrowser.Size = new System.Drawing.Size(781, 568);
            this._webBrowser.TabIndex = 1;
            this._webBrowser.Url = new System.Uri("https://addons.mozilla.org/en-US/firefox/dictionaries/", System.UriKind.Absolute);
            this._webBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this._webBrowser_Navigating);
            // 
            // AddDictionariesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 615);
            this.Controls.Add(this._webBrowser);
            this.Controls.Add(this.formHeader1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddDictionariesForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Add Dictionaries";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SystemEx.Windows.Forms.FormHeader formHeader1;
        private System.Windows.Forms.WebBrowser _webBrowser;

    }
}