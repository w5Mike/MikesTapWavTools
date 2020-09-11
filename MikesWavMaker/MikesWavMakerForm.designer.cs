namespace MikesWavMaker
{
    partial class MikesWavMakerForm
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
            this.wavMaker.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MikesWavMakerForm));
            this.infoTextBox = new System.Windows.Forms.TextBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.tapFileButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.tapFilenameTextBox = new System.Windows.Forms.TextBox();
            this.wavFilenameTextBox = new System.Windows.Forms.TextBox();
            this.wavFileButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.tapPanel = new System.Windows.Forms.GroupBox();
            this.wavPanel = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tapPanel.SuspendLayout();
            this.wavPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // infoTextBox
            // 
            this.infoTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.infoTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.infoTextBox.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoTextBox.Location = new System.Drawing.Point(8, 155);
            this.infoTextBox.Multiline = true;
            this.infoTextBox.Name = "infoTextBox";
            this.infoTextBox.ReadOnly = true;
            this.infoTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.infoTextBox.Size = new System.Drawing.Size(526, 217);
            this.infoTextBox.TabIndex = 13;
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(427, 452);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(107, 26);
            this.closeButton.TabIndex = 11;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // tapFileButton
            // 
            this.tapFileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.tapFileButton.Location = new System.Drawing.Point(375, 21);
            this.tapFileButton.Name = "tapFileButton";
            this.tapFileButton.Size = new System.Drawing.Size(28, 26);
            this.tapFileButton.TabIndex = 16;
            this.tapFileButton.Text = "...";
            this.tapFileButton.UseVisualStyleBackColor = true;
            this.tapFileButton.Click += new System.EventHandler(this.TapFileButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.Enabled = false;
            this.loadButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.loadButton.Location = new System.Drawing.Point(409, 21);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(108, 26);
            this.loadButton.TabIndex = 18;
            this.loadButton.Text = "Load/Reload";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // tapFilenameTextBox
            // 
            this.tapFilenameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.tapFilenameTextBox.Location = new System.Drawing.Point(6, 24);
            this.tapFilenameTextBox.Name = "tapFilenameTextBox";
            this.tapFilenameTextBox.Size = new System.Drawing.Size(366, 20);
            this.tapFilenameTextBox.TabIndex = 17;
            // 
            // wavFilenameTextBox
            // 
            this.wavFilenameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.wavFilenameTextBox.Location = new System.Drawing.Point(6, 24);
            this.wavFilenameTextBox.Name = "wavFilenameTextBox";
            this.wavFilenameTextBox.Size = new System.Drawing.Size(363, 20);
            this.wavFilenameTextBox.TabIndex = 21;
            this.wavFilenameTextBox.TextChanged += new System.EventHandler(this.WavFilenameTextBox_Changed);
            // 
            // wavFileButton
            // 
            this.wavFileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.wavFileButton.Location = new System.Drawing.Point(375, 21);
            this.wavFileButton.Name = "wavFileButton";
            this.wavFileButton.Size = new System.Drawing.Size(28, 26);
            this.wavFileButton.TabIndex = 19;
            this.wavFileButton.Text = "...";
            this.wavFileButton.UseVisualStyleBackColor = true;
            this.wavFileButton.Click += new System.EventHandler(this.WavButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Enabled = false;
            this.saveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.saveButton.Location = new System.Drawing.Point(409, 21);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(108, 26);
            this.saveButton.TabIndex = 20;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // tapPanel
            // 
            this.tapPanel.Controls.Add(this.loadButton);
            this.tapPanel.Controls.Add(this.tapFilenameTextBox);
            this.tapPanel.Controls.Add(this.tapFileButton);
            this.tapPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tapPanel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tapPanel.Location = new System.Drawing.Point(8, 91);
            this.tapPanel.Name = "tapPanel";
            this.tapPanel.Size = new System.Drawing.Size(526, 58);
            this.tapPanel.TabIndex = 25;
            this.tapPanel.TabStop = false;
            this.tapPanel.Text = "Tap File:";
            // 
            // wavPanel
            // 
            this.wavPanel.Controls.Add(this.saveButton);
            this.wavPanel.Controls.Add(this.wavFileButton);
            this.wavPanel.Controls.Add(this.wavFilenameTextBox);
            this.wavPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wavPanel.Location = new System.Drawing.Point(8, 379);
            this.wavPanel.Name = "wavPanel";
            this.wavPanel.Size = new System.Drawing.Size(526, 58);
            this.wavPanel.TabIndex = 26;
            this.wavPanel.TabStop = false;
            this.wavPanel.Text = "Wav File:";
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::MikesWavMaker.Properties.Resources.MikesWavMaker;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(542, 100);
            this.panel1.TabIndex = 22;
            // 
            // MikesWavMakerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 489);
            this.Controls.Add(this.tapPanel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.wavPanel);
            this.Controls.Add(this.infoTextBox);
            this.Controls.Add(this.closeButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MikesWavMakerForm";
            this.Text = "Mike\'s Wav Maker ";
            this.Load += new System.EventHandler(this.MikesWavMakerForm_Load);
            this.Resize += new System.EventHandler(this.MikesWavMakerForm_Resize);
            this.tapPanel.ResumeLayout(false);
            this.tapPanel.PerformLayout();
            this.wavPanel.ResumeLayout(false);
            this.wavPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox infoTextBox;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.TextBox wavFilenameTextBox;
        private System.Windows.Forms.Button wavFileButton;
        private System.Windows.Forms.Button tapFileButton;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.TextBox tapFilenameTextBox;
        private System.Windows.Forms.GroupBox tapPanel;
        private System.Windows.Forms.GroupBox wavPanel;
        private System.Windows.Forms.Panel panel1;
    }
}

