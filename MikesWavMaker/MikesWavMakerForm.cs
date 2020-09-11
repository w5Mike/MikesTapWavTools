using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace MikesWavMaker
{
    partial class MikesWavMakerForm : Form
    {
        private int tapPanelWidthDiff = 0;
        private int tapFilenameTextBoxWidthDiff = 0;
        private int tapFileButtonLeftDiff = 0;
        private int loadButtonLeftDiff = 0;
        private int infoTextBoxWidthDiff = 0;
        private int infoTextBoxHeightDiff = 0;
        private int closeButtonLeftDiff = 0;
        private int closeButtonTopDiff = 0;
        private int wavPanelWidthDiff = 0;
        private int wavPanelTopDiff = 0;
        private int wavFileButtonLeftDiff = 0;
        private int wavFilenameTextBoxWidthDiff = 0;
        private int saveButtonLeftDiff = 0;
        private ArrayList tapList = new ArrayList();

        private TapReader tapReader;
        private WavMaker wavMaker;

        public MikesWavMakerForm()
        {
            InitializeComponent();
            this.wavMaker = new WavMaker((float)0.8, 22050, 16);
        }

        private void WavFilenameTextBox_Changed(object sender, EventArgs e)
        {
            if (wavFilenameTextBox.Text.Trim() != "")
                saveButton.Enabled = true;
        }

        private void WavButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = chooseWavFileDialog();
        }

        private DialogResult chooseWavFileDialog()
        {
            SaveFileDialog wavFileDialog = new SaveFileDialog();
            if (wavFilenameTextBox.Text.Trim() != "")
            {
                wavFileDialog.InitialDirectory = Path.GetFullPath(wavFilenameTextBox.Text);
                wavFileDialog.FileName = Path.GetFileName(wavFilenameTextBox.Text);
            }
            wavFileDialog.Filter = "Wave File (*.wav)|*.wav;";
            DialogResult dr = wavFileDialog.ShowDialog(this);

            if (dr == DialogResult.OK)
            {
                wavFilenameTextBox.Text = wavFileDialog.FileName;
                loadButton.Enabled = true;
                saveButton.Enabled = true;
            }
            return dr;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MikesWavMakerForm_Load(object sender, EventArgs e)
        {
            tapFilenameTextBoxWidthDiff = tapPanel.Width - tapFilenameTextBox.Width;
            tapFileButtonLeftDiff = tapPanel.Width - tapFileButton.Left;
            loadButtonLeftDiff = tapPanel.Width - loadButton.Left;
            infoTextBoxWidthDiff = Width - infoTextBox.Width;
            infoTextBoxHeightDiff = Height - infoTextBox.Height;
            closeButtonLeftDiff = Width - closeButton.Left;
            closeButtonTopDiff = Height - closeButton.Top;
            tapPanelWidthDiff = Width - tapPanel.Width;
            wavPanelWidthDiff = Width - wavPanel.Width;
            wavPanelTopDiff = Height - wavPanel.Top;
            wavFileButtonLeftDiff = tapPanel.Width - wavFileButton.Left;
            wavFilenameTextBoxWidthDiff = Width - wavFilenameTextBox.Width;
            saveButtonLeftDiff = Width - saveButton.Left;
        }

        private void MikesWavMakerForm_Resize(object sender, EventArgs e)
        {
            tapPanel.Width = Width - tapPanelWidthDiff;
            tapFilenameTextBox.Width = tapPanel.Width - tapFilenameTextBoxWidthDiff;
            tapFileButton.Left = tapPanel.Width - tapFileButtonLeftDiff;
            loadButton.Left = tapPanel.Width - loadButtonLeftDiff;
            infoTextBox.Width = Width - infoTextBoxWidthDiff;
            infoTextBox.Height = Height - infoTextBoxHeightDiff;
            closeButton.Left = Width - closeButtonLeftDiff;
            closeButton.Top = Height - closeButtonTopDiff;
            wavPanel.Width = Width - wavPanelWidthDiff;
            wavPanel.Top = Height - wavPanelTopDiff;
            wavFilenameTextBox.Width = Width - wavFilenameTextBoxWidthDiff;
            wavFileButton.Left = wavPanel.Width - wavFileButtonLeftDiff;
            saveButton.Left = Width - saveButtonLeftDiff;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            bool isValidFilename = false;
            MessageBoxButtons buttons;

            while (!isValidFilename)
            {
                if (File.Exists(wavFilenameTextBox.Text))
                {
                    buttons = MessageBoxButtons.YesNoCancel;
                    dr = MessageBox.Show(Path.GetFileName(wavFilenameTextBox.Text) + " already exists. Do you wish to overwrite this file?", "Tap File", buttons, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                        isValidFilename = true;
                    else if (dr == DialogResult.Cancel)
                        return;
                    if (dr == DialogResult.No)
                    {
                        DialogResult chooseResult = chooseWavFileDialog();
                        if (chooseResult == DialogResult.Cancel)
                            return;
                    }
                }
                else
                    isValidFilename = true;
            }

            TapFile[] tapList = (TapFile[])this.tapList.ToArray(typeof(MikesWavMaker.TapFile));

            buttons = MessageBoxButtons.OK;
            if (this.wavMaker.MakeWav(tapList, wavFilenameTextBox.Text))
                MessageBox.Show("Wav file saved successfully.", "Wav File", buttons, MessageBoxIcon.Information);
            else
                MessageBox.Show("Failed - Wav file failed to save.", "Wav File", buttons, MessageBoxIcon.Error);
        }

        private void TapFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog tapFileDialog = new OpenFileDialog();
            tapFileDialog.Filter = "Tap File (*.tap)|*.tap;";
            if (tapFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                tapFilenameTextBox.Text = tapFileDialog.FileName;
                string path = Path.GetFullPath(tapFileDialog.FileName);
                string filename = Path.GetFileName(tapFileDialog.FileName);
                wavFilenameTextBox.Text = tapFileDialog.FileName.Substring(0, tapFileDialog.FileName.ToLower().IndexOf(".tap")) + ".wav";
                loadButton.Enabled = true;
                saveButton.Enabled = true;
                LoadTaps();
            }
        }

        private void LoadTaps()
        {
            infoTextBox.Clear();
            this.tapList.Clear();
            if (!File.Exists(tapFilenameTextBox.Text.Trim()))
            {
                MessageBox.Show("Please supply a valid Tap filename.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                this.tapReader = new TapReader(tapFilenameTextBox.Text);
                string output = tapReader.Read();
                infoTextBox.AppendText(output);
                if (!this.tapReader.failed)
                {
                    this.tapList.Add((TapFile)this.tapReader);
                    while ((!this.tapReader.isLastTap) && (!this.tapReader.failed))
                    {
                        this.tapReader = new TapReader(this.tapReader.nextBytes);
                        output = this.tapReader.Read();
                        infoTextBox.AppendText(output);
                        this.tapList.Add((TapFile)this.tapReader);
                    }
                }

                infoTextBox.AppendText("Number of taps found in file: " + this.tapList.Count + Environment.NewLine);

                // Scroll to top
                infoTextBox.SelectionStart = 0;
                infoTextBox.SelectionLength = 1;
                infoTextBox.ScrollToCaret();

            }
            catch (IOException err)
            {
                infoTextBox.AppendText(err.Message + Environment.NewLine + "Error reading tap file." + Environment.NewLine);
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            LoadTaps();
        }
    }
}
