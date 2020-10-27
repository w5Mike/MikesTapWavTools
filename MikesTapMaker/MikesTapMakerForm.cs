using System;
using System.IO;
using System.Windows.Forms;

namespace MikesTapMaker
{
    public partial class MikesTapMakerForm : Form
    {
        private int wavPanelWidthDiff = 0;
        private int wavFilenameTextBoxWidthDiff = 0;
        private int wavFileButtonLeftDiff = 0;
        private int reloadButtonLeftDiff = 0;
        private int infoTextBoxWidthDiff = 0;
        private int infoTextBoxHeightDiff = 0;
        private int closeButtonLeftDiff = 0;
        private int closeButtonTopDiff = 0;
        private int tapPanelWidthDiff = 0;
        private int tapPanelTopDiff = 0;
        private int tapFileButtonLeftDiff = 0;
        private int tapFilenameWidthDiff = 0;
        private int saveButtonLeftDiff = 0;
        private TapFile tapFile;
        private ushort programLength = 0;

        public MikesTapMakerForm()
        {
            InitializeComponent();
        }

        private void WavReaderForm_Load(object sender, EventArgs e)
        {
            // Calculate resizing offsets.
            wavFilenameTextBoxWidthDiff = wavPanel.Width - wavFilenameTextBox.Width;
            wavFileButtonLeftDiff = wavPanel.Width - wavFileButton.Left;
            infoTextBoxWidthDiff = Width - infoTextBox.Width;
            infoTextBoxHeightDiff = Height - infoTextBox.Height;
            reloadButtonLeftDiff = wavPanel.Width - reloadButton.Left;
            closeButtonLeftDiff = Width - closeButton.Left;
            closeButtonTopDiff = Height - closeButton.Top;
            tapPanelWidthDiff = Width - tapPanel.Width;
            wavPanelWidthDiff = Width - wavPanel.Width;
            tapPanelTopDiff = Height - tapPanel.Top;
            tapFileButtonLeftDiff = tapPanel.Width - tapFileButton.Left;
            tapFilenameWidthDiff = Width - tapFilenameTextBox.Width;
            saveButtonLeftDiff = Width - saveButton.Left;
        }


        private void WavReaderForm_Resize(object sender, System.EventArgs e)
        {
            // Move the controls about on resize.
            wavFilenameTextBox.Width = wavPanel.Width - wavFilenameTextBoxWidthDiff;
            wavFileButton.Left = wavPanel.Width - wavFileButtonLeftDiff;
            reloadButton.Left = wavPanel.Width - reloadButtonLeftDiff;
            infoTextBox.Width = Width - infoTextBoxWidthDiff;
            infoTextBox.Height = Height - infoTextBoxHeightDiff;
            closeButton.Left = Width - closeButtonLeftDiff;
            closeButton.Top = Height - closeButtonTopDiff;
            wavPanel.Width = Width - wavPanelWidthDiff;
            tapPanel.Width = Width - tapPanelWidthDiff;
            tapPanel.Top = Height - tapPanelTopDiff;
            tapFilenameTextBox.Width = Width - tapFilenameWidthDiff;
            tapFileButton.Left = tapPanel.Width - tapFileButtonLeftDiff;
            saveButton.Left = Width - saveButtonLeftDiff;
        }

        private void wavButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog wavFileDialog = new OpenFileDialog();
            wavFileDialog.Filter = "Wave File (*.wav)|*.wav;";
            if (wavFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                wavFilenameTextBox.Text = wavFileDialog.FileName;
                string path = Path.GetFullPath(wavFileDialog.FileName);
                string filename = Path.GetFileName(wavFileDialog.FileName);
                tapFilenameTextBox.Text = wavFileDialog.FileName.Substring(0, wavFileDialog.FileName.ToLower().IndexOf(".wav")) + ".tap";
                reloadButton.Enabled = true;
                ReloadButton_Click(sender, e);
            }
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            if (!validateWavFilename()) return;

            Application.DoEvents();

            using (NAudio.Wave.WaveFileReader waveFile = new NAudio.Wave.WaveFileReader(wavFilenameTextBox.Text))
            {
                tapFile = new TapFile();

                WaveSeeker waveSeeker = new WaveSeeker(waveFile);

                // Read Header Information
                if (!readHeaderInfo(waveSeeker)) return;

                // If we don't know the program length
                if (tapFile.programLength == 0)
                    return;

                // Read program itself
                if (!readProgramBytes(waveSeeker)) return;

                // Display program bytes
                displayProgramBytes();

                // Validate Basic terminator
                if ((tapFile.fileType == 'B') && (tapFile.program[tapFile.programLength - 1] != 0x80))
                    infoTextBox.AppendText("Warning: 'End-of-program' terminator missing (&80)" + Environment.NewLine);

                // Actual length
                infoTextBox.AppendText(Environment.NewLine + "Length: " + tapFile.programLength + " (agreed)" + Environment.NewLine);

                infoTextBox.AppendText(Environment.NewLine + "Trailing Information:" + Environment.NewLine + "---------------------" + Environment.NewLine);

                // Read checksum
                if ((tapFile.fileType == 'M') || (tapFile.fileType == 'D') || (tapFile.fileType == '9'))
                {
                    if (!waveSeeker.readChecksum(out tapFile.checksum))
                        infoTextBox.AppendText("Warning: Checksum wrong/not found (ignored)" + Environment.NewLine);

                    infoTextBox.AppendText("Checksum: " + tapFile.checksum.ToString("X2") + Environment.NewLine);

                    // Calculate actual checksum
                    byte actualChecksum = 0;
                    for (int i = 0; i < tapFile.program.Length; i++)
                        actualChecksum += tapFile.program[i];
                    infoTextBox.AppendText("Calculated Checksum: " + actualChecksum.ToString("X2"));
                    if (tapFile.checksum == actualChecksum)
                        infoTextBox.AppendText(" (agreed)" + Environment.NewLine);
                    else
                        infoTextBox.AppendText(Environment.NewLine + "Warning: checksums disagree (ignored)" + Environment.NewLine);
                    tapFile.checksum = actualChecksum;

                    if ((tapFile.fileType == 'M') || (tapFile.fileType == 'D'))
                    {
                        waveSeeker.readByte(out tapFile.mysteryByte);
                        infoTextBox.AppendText("Mystery byte: " + tapFile.mysteryByte.ToString("X2") + Environment.NewLine);
                    }
                }

                // Read excution point
                if (tapFile.fileType != '9' && tapFile.fileType != 'D')
                {
                    tapFile.executionPoint = waveSeeker.readUShort();
                    infoTextBox.AppendText("Execution point: 0x" + tapFile.executionPoint.ToString("X4") + Environment.NewLine);
                }

                // Scroll to top
                infoTextBox.SelectionStart = 0;
                infoTextBox.SelectionLength = 1;
                infoTextBox.ScrollToCaret();

                // Now enable Tap controls.
                saveButton.Enabled = isSaveEnabled();
            }
        }

        private void tapButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = chooseTapFileDialog();
        }

        private DialogResult chooseTapFileDialog()
        {
            SaveFileDialog tapFileDialog = new SaveFileDialog();
            if (wavFilenameTextBox.Text.Trim() != "")
            {
                tapFileDialog.InitialDirectory = Path.GetFullPath(tapFilenameTextBox.Text);
                tapFileDialog.FileName = Path.GetFileName(tapFilenameTextBox.Text);
            }
            tapFileDialog.CheckFileExists = false;
            tapFileDialog.OverwritePrompt = false;
            tapFileDialog.Filter = "Tap File (*.tap)|*.tap;";
            DialogResult dr = tapFileDialog.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                tapFilenameTextBox.Text = tapFileDialog.FileName;
                saveButton.Enabled = isSaveEnabled();
            }
            return dr;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool validateWavFilename()
        {
            wavFilenameTextBox.Text = wavFilenameTextBox.Text.Trim();
            if (wavFilenameTextBox.Text == "")
            {
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show("Please supply a valid wav filename.", "Warning", buttons, MessageBoxIcon.Warning);
                return false;
            }
            else return true;
        }

        private bool validateWavFormat(WaveSeeker waveSeeker)
        {
            infoTextBox.AppendText("Wave Format: " + waveSeeker.WaveFormat + Environment.NewLine);
            infoTextBox.AppendText("Length: " + waveSeeker.WaveFile.Length + " bytes" + Environment.NewLine);
            infoTextBox.AppendText("Total time: " + waveSeeker.WaveFile.TotalTime + Environment.NewLine);

            // Validate wave file format
            if (waveSeeker.WaveFormat.BitsPerSample != 16)
            {
                infoTextBox.AppendText("Failed: only works with 16 bit audio (-for now)." + Environment.NewLine);
                return false;
            }

            if (waveSeeker.WaveFormat.SampleRate != 22050)
            {
                infoTextBox.AppendText("Failed: only works with 22Khz sample rate (-for now)." + Environment.NewLine);
                return false;
            }

            return true;
        }

        private bool findSyncZero(WaveSeeker waveSeeker)
        {
            WavePositionsStruct wavePositions = waveSeeker.findSyncZero();

            if (waveSeeker.EndOfFile)
            {
                infoTextBox.AppendText("Failed: Sync zero not found. End of File." + Environment.NewLine);
                return false;
            }

            infoTextBox.AppendText(
                "Sync zero found. " +
                "(" + wavePositions.start +
                "," + wavePositions.min +
                "," + wavePositions.crossover +
                "," + wavePositions.max + ")" + Environment.NewLine
            );

            return true;
        }

        private bool findA5Byte(WaveSeeker waveSeeker)
        {
            if (!waveSeeker.seekA5Byte())
            {
                infoTextBox.AppendText("Failed: A5 Byte not found! End Of File." + Environment.NewLine);
                return false;
            }

            infoTextBox.AppendText("A5 byte found at " + waveSeeker.WavePositions.max + Environment.NewLine);
            return true;
        }

        private bool readLoadFilename(WaveSeeker waveSeeker)
        {
            tapFile.loadName = waveSeeker.readLoadName();

            if (tapFile.loadName == "")
            {
                infoTextBox.AppendText("Failed: Load Name not found." + Environment.NewLine);
                return false;
            }
            infoTextBox.AppendText("Load Name: " + (tapFile.loadName == "<Level9>" ? "N/A " : "") + tapFile.loadName + Environment.NewLine);
            return true;
        }

        private bool readFileType(WaveSeeker waveSeeker)
        {
            tapFile.fileType = waveSeeker.readFileType();

            if (tapFile.fileType == '\0')
                infoTextBox.AppendText("Warning: File Type not found." + Environment.NewLine);
            else
                infoTextBox.AppendText("File Type: " + tapFile.fileType + Environment.NewLine);

            return true;
        }

        private bool readProgramLength(WaveSeeker waveSeeker)
        {
            // Read program length
            tapFile.programLength = waveSeeker.readUShort();

            if (tapFile.programLength == 0)
                infoTextBox.AppendText("Warning: Program Length not found." + Environment.NewLine);
            else
                infoTextBox.AppendText("Program Length: " + tapFile.programLength + Environment.NewLine);

            return true;
        }

        private bool readLoadPoint(WaveSeeker waveSeeker)
        {
            // Read load point
            if ((tapFile.fileType == '9') || (tapFile.fileType == 'M'))
            {
                tapFile.loadPoint = waveSeeker.readUShort();
                infoTextBox.AppendText("Load Point: " + "0x" + tapFile.loadPoint.ToString("X4") + Environment.NewLine);
            }
            return true;
        }

        private bool readHeaderInfo(WaveSeeker waveSeeker)
        {
            try
            {
                infoTextBox.Clear();

                infoTextBox.AppendText("Header Information: " + Environment.NewLine + "-------------------" + Environment.NewLine);

                if (!validateWavFormat(waveSeeker)) return false;

                if (!findSyncZero(waveSeeker)) return false;

                if (!findA5Byte(waveSeeker)) return false;

                if (!readLoadFilename(waveSeeker)) return false;

                if (tapFile.loadName == "<Level9>")
                {
                    tapFile.fileType = '9';
                    if (!readLoadPoint(waveSeeker)) return false;
                    if (!readProgramLength(waveSeeker)) return false;
                }
                else if (tapFile.loadName != "")
                {
                    if (!findA5Byte(waveSeeker)) return false;
                    if (!readFileType(waveSeeker)) return false;
                    if (!readProgramLength(waveSeeker)) return false;
                    if (!readLoadPoint(waveSeeker)) return false;
                }

                return true;
            }
            catch (IOException err)
            {
                infoTextBox.AppendText(err.Message + Environment.NewLine + "Error reading file." + Environment.NewLine);
                return false;
            }
        }

        private bool readProgramBytes(WaveSeeker waveSeeker)
        {
            tapFile.program = new byte[tapFile.programLength];

            programLength = waveSeeker.readProgram(tapFile.program, tapFile.programLength);

            if (tapFile.programLength != programLength)
            {
                infoTextBox.AppendText("Failed: Actual length: " + programLength + Environment.NewLine);
                return false;
            }

            if (waveSeeker.EndOfFile)
            {
                infoTextBox.AppendText("Failed: End-of-file reached. " + Environment.NewLine);
                return false;
            }

            return true;
        }

        private void displayProgramBytes()
        {
            // Display program bytes
            infoTextBox.AppendText(Environment.NewLine + "Program Bytes: " + Environment.NewLine + "--------------" + Environment.NewLine);

            short counter = 0;
            string charVersion = "";

            for (ushort i = 0; i < tapFile.programLength; i++)
            {
                infoTextBox.AppendText(tapFile.program[i].ToString("X2") + " ");
                charVersion += printableChar((char)tapFile.program[i]);
                if (counter == 7)
                {
                    infoTextBox.AppendText(" " + charVersion + Environment.NewLine);
                    charVersion = "";
                    counter = 0;
                }
                else
                    counter++;
            }

            // Formatting...
            while (counter < 8)
            {
                infoTextBox.AppendText("   ");
                counter++;
            }

            if (charVersion != "")
                infoTextBox.AppendText(" " + charVersion + Environment.NewLine + Environment.NewLine);
        }

        private char printableChar(char theChar)
        {
            return ((Char.IsWhiteSpace(theChar) || Char.IsControl(theChar)) ? '.' : theChar);
        }


        private bool isSaveEnabled()
        {
            return (tapFile.programLength != 0) && (tapFilenameTextBox.Text != "");
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            bool isValidFilename = false;
            MessageBoxButtons buttons;

            while (!isValidFilename)
            {
                if (File.Exists(tapFilenameTextBox.Text))
                {
                    buttons = MessageBoxButtons.YesNoCancel;
                    dr = MessageBox.Show(Path.GetFileName(tapFilenameTextBox.Text) + " already exists. Do you wish to overwrite this file?", "Tap File", buttons, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                        isValidFilename = true;
                    else if (dr == DialogResult.Cancel)
                        return;
                    if (dr == DialogResult.No)
                    {
                        DialogResult chooseResult = chooseTapFileDialog();
                        if (chooseResult == DialogResult.Cancel)
                            return;
                    }
                }
                else
                    isValidFilename = true;
            }

            buttons = MessageBoxButtons.OK;
            if (tapFile.WriteTapFile(tapFilenameTextBox.Text))
                MessageBox.Show("Tap file saved successfully.", "Tap File", buttons, MessageBoxIcon.Information);
            else
                MessageBox.Show("Failed - Tap file failed to save.", "Tap File", buttons, MessageBoxIcon.Error);
        }

        private void tapFilenameTextBox_Leave(object sender, EventArgs e)
        {
            saveButton.Enabled = isSaveEnabled();
        }

        private void dumpButton_Click(object sender, EventArgs e)
        {
            if (!validateWavFilename()) return;

            Application.DoEvents();

            using (NAudio.Wave.WaveFileReader waveFile = new NAudio.Wave.WaveFileReader(wavFilenameTextBox.Text))
            {
                tapFile = new TapFile();

                WaveSeeker waveSeeker = new WaveSeeker(waveFile);

                infoTextBox.Clear();
                // Read Header Information
                if (!readHeaderInfo(waveSeeker)) return;

                byte theByte = 0;

                short counter = 0;
                string charVersion = "";

                // while not eof...
                while (waveSeeker.readByte(out theByte))
                {
                    infoTextBox.AppendText(theByte.ToString("X2") + " ");
                    charVersion += printableChar((char)theByte);
                    if (counter == 7)
                    {
                        infoTextBox.AppendText(" " + charVersion + Environment.NewLine);
                        charVersion = "";
                        counter = 0;
                    }
                    else
                        counter++;
                }

                // Formatting...
                while (counter < 8)
                {
                    infoTextBox.AppendText("   ");
                    counter++;
                }

                if (charVersion != "")
                    infoTextBox.AppendText(" " + charVersion + Environment.NewLine + Environment.NewLine);
            }
        }
    }
}
