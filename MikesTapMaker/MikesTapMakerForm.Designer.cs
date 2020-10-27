namespace MikesTapMaker
{
    partial class MikesTapMakerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MikesTapMakerForm));
            this.wavFileButton = new System.Windows.Forms.Button();
            this.tapFileButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.wavFilenameTextBox = new System.Windows.Forms.TextBox();
            this.infoTextBox = new System.Windows.Forms.TextBox();
            this.tapFilenameTextBox = new System.Windows.Forms.TextBox();
            this.reloadButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dumpButton = new System.Windows.Forms.Button();
            this.wavPanel = new System.Windows.Forms.GroupBox();
            this.tapPanel = new System.Windows.Forms.GroupBox();
            this.panel1.SuspendLayout();
            this.wavPanel.SuspendLayout();
            this.tapPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // wavFileButton
            // 
            this.wavFileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.wavFileButton.Location = new System.Drawing.Point(375, 21);
            this.wavFileButton.Name = "wavFileButton";
            this.wavFileButton.Size = new System.Drawing.Size(28, 26);
            this.wavFileButton.TabIndex = 0;
            this.wavFileButton.Text = "...";
            this.wavFileButton.UseVisualStyleBackColor = true;
            this.wavFileButton.Click += new System.EventHandler(this.wavButton_Click);
            // 
            // tapFileButton
            // 
            this.tapFileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.tapFileButton.Location = new System.Drawing.Point(375, 21);
            this.tapFileButton.Name = "tapFileButton";
            this.tapFileButton.Size = new System.Drawing.Size(28, 26);
            this.tapFileButton.TabIndex = 1;
            this.tapFileButton.Text = "...";
            this.tapFileButton.UseVisualStyleBackColor = true;
            this.tapFileButton.Click += new System.EventHandler(this.tapButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(427, 452);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(108, 26);
            this.closeButton.TabIndex = 2;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // wavFilenameTextBox
            // 
            this.wavFilenameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.wavFilenameTextBox.Location = new System.Drawing.Point(6, 24);
            this.wavFilenameTextBox.Name = "wavFilenameTextBox";
            this.wavFilenameTextBox.Size = new System.Drawing.Size(366, 20);
            this.wavFilenameTextBox.TabIndex = 3;
            // 
            // infoTextBox
            // 
            this.infoTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.infoTextBox.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoTextBox.Location = new System.Drawing.Point(8, 155);
            this.infoTextBox.Multiline = true;
            this.infoTextBox.Name = "infoTextBox";
            this.infoTextBox.ReadOnly = true;
            this.infoTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.infoTextBox.Size = new System.Drawing.Size(526, 217);
            this.infoTextBox.TabIndex = 4;
            // 
            // tapFilenameTextBox
            // 
            this.tapFilenameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.tapFilenameTextBox.Location = new System.Drawing.Point(6, 24);
            this.tapFilenameTextBox.Name = "tapFilenameTextBox";
            this.tapFilenameTextBox.Size = new System.Drawing.Size(363, 20);
            this.tapFilenameTextBox.TabIndex = 5;
            this.tapFilenameTextBox.Leave += new System.EventHandler(this.tapFilenameTextBox_Leave);
            // 
            // reloadButton
            // 
            this.reloadButton.Enabled = false;
            this.reloadButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.reloadButton.Location = new System.Drawing.Point(409, 21);
            this.reloadButton.Name = "reloadButton";
            this.reloadButton.Size = new System.Drawing.Size(108, 26);
            this.reloadButton.TabIndex = 6;
            this.reloadButton.Text = "Load/Reload";
            this.reloadButton.UseVisualStyleBackColor = true;
            this.reloadButton.Click += new System.EventHandler(this.ReloadButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Enabled = false;
            this.saveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.saveButton.Location = new System.Drawing.Point(409, 21);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(108, 26);
            this.saveButton.TabIndex = 7;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::MikesTapMaker.Properties.Resources.MikesTapMaker;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel1.Controls.Add(this.dumpButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(542, 100);
            this.panel1.TabIndex = 23;
            // 
            // dumpButton
            // 
            this.dumpButton.Location = new System.Drawing.Point(464, 62);
            this.dumpButton.Name = "dumpButton";
            this.dumpButton.Size = new System.Drawing.Size(75, 23);
            this.dumpButton.TabIndex = 28;
            this.dumpButton.Text = "Dump";
            this.dumpButton.UseVisualStyleBackColor = true;
            this.dumpButton.Click += new System.EventHandler(this.dumpButton_Click);
            // 
            // wavPanel
            // 
            this.wavPanel.Controls.Add(this.wavFilenameTextBox);
            this.wavPanel.Controls.Add(this.wavFileButton);
            this.wavPanel.Controls.Add(this.reloadButton);
            this.wavPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wavPanel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.wavPanel.Location = new System.Drawing.Point(8, 91);
            this.wavPanel.Name = "wavPanel";
            this.wavPanel.Size = new System.Drawing.Size(526, 58);
            this.wavPanel.TabIndex = 26;
            this.wavPanel.TabStop = false;
            this.wavPanel.Text = "Wav File:";
            // 
            // tapPanel
            // 
            this.tapPanel.Controls.Add(this.tapFilenameTextBox);
            this.tapPanel.Controls.Add(this.tapFileButton);
            this.tapPanel.Controls.Add(this.saveButton);
            this.tapPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tapPanel.Location = new System.Drawing.Point(8, 379);
            this.tapPanel.Name = "tapPanel";
            this.tapPanel.Size = new System.Drawing.Size(526, 58);
            this.tapPanel.TabIndex = 27;
            this.tapPanel.TabStop = false;
            this.tapPanel.Text = "Tap File:";
            // 
            // MikesTapMakerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 489);
            this.Controls.Add(this.tapPanel);
            this.Controls.Add(this.wavPanel);
            this.Controls.Add(this.infoTextBox);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.panel1);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MikesTapMakerForm";
            this.Text = "Mike\'s Tap Maker";
            this.Load += new System.EventHandler(this.WavReaderForm_Load);
            this.Resize += new System.EventHandler(this.WavReaderForm_Resize);
            this.panel1.ResumeLayout(false);
            this.wavPanel.ResumeLayout(false);
            this.wavPanel.PerformLayout();
            this.tapPanel.ResumeLayout(false);
            this.tapPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button wavFileButton;
        private System.Windows.Forms.Button tapFileButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.TextBox wavFilenameTextBox;
        private System.Windows.Forms.TextBox infoTextBox;
        private System.Windows.Forms.TextBox tapFilenameTextBox;
        private System.Windows.Forms.Button reloadButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox wavPanel;
        private System.Windows.Forms.GroupBox tapPanel;
        private System.Windows.Forms.Button dumpButton;
    }
}

