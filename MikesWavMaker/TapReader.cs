using System;
using System.IO;
using System.Text;

namespace MikesWavMaker
{
    public class TapFile
    {
        internal bool failed = false;
        internal string loadName = "";
        internal char fileType = '\0';
        internal ushort programLength = 0;
        internal byte[] program;
        internal ushort headerLength = 0;
        internal ushort loadPoint = 0;
        internal byte checksum = 0;
        internal ushort executionPoint = 0;
        internal byte mysteryByte = 0;
    }

    public class TapReader : TapFile
    {

        private string filename;
        private byte[] fileBytes = null;
        private int offset = 0;
        internal byte[] nextBytes = null;

        internal bool isLastTap = true;

        public TapReader(string filename)
        {
            this.filename = filename;
        }

        public TapReader(byte[] byteBuffer)
        {
            this.fileBytes = byteBuffer;
        }

        private string ParseHeader()
        {
            StringBuilder resultText = new StringBuilder();
            string filename = "";
            if (this.fileBytes[this.offset] == 0xA5)
            {
                this.offset++;
                if (this.fileBytes[this.offset++] == 0x41)
                {
                    this.fileType = '9';
                    this.loadName = "<Level9>";
                    this.loadPoint = BitConverter.ToUInt16(new byte[2] { this.fileBytes[this.offset++], this.fileBytes[this.offset++] }, 0);
                    this.programLength = BitConverter.ToUInt16(new byte[2] { this.fileBytes[this.offset++], this.fileBytes[this.offset++] }, 0);
                }
                else
                {
                    resultText.Append("Error reading tap file. Unknown file type." + Environment.NewLine);
                    this.failed = true;
                    return resultText.ToString();
                }
            }
            else
            {
                if (this.fileBytes[this.offset++] != 0x22)
                {
                    resultText.Append("Error reading tap file: load name not found." + Environment.NewLine);
                    this.failed = true;
                    return resultText.ToString();
                }
                while (this.fileBytes[this.offset] != 0x22 && this.offset <= 80)
                {
                    filename += (char)this.fileBytes[this.offset++];
                }
                if (this.offset > 80)
                {
                    resultText.Append("Error reading tap file: load name not found." + Environment.NewLine);
                    this.failed = true;
                    return resultText.ToString();
                }
                this.loadName = filename;

                this.offset++;
                if (this.fileBytes[this.offset] != 66 && this.fileBytes[this.offset] != 77  && this.fileBytes[this.offset] != 68)
                {
                    resultText.Append("Error reading tap file. File type (M/B/D) not found." + Environment.NewLine);
                    this.failed = true;
                    return resultText.ToString();
                }
                this.fileType = (char)this.fileBytes[this.offset++];
                this.programLength = BitConverter.ToUInt16(new byte[2] { this.fileBytes[this.offset++], this.fileBytes[this.offset++] }, 0);
                if (this.fileType == 'M')
                {
                    this.loadPoint = BitConverter.ToUInt16(new byte[2] { this.fileBytes[this.offset++], this.fileBytes[this.offset++] }, 0);
                }
                else
                    this.loadPoint = 0;
            }
            this.headerLength = (ushort)(this.offset);
            this.offset++;
            resultText.Append("Header Information: " + Environment.NewLine + "-------------------" + Environment.NewLine);
            resultText.Append("Load Name: " + this.loadName + Environment.NewLine);
            resultText.Append("File Type: " + this.fileType + Environment.NewLine);
            resultText.Append("Program/Data Length: " + this.programLength + " bytes" + Environment.NewLine);
            if (this.fileType == 'M')
            {
                resultText.Append("Load Point: 0x" + $"{this.loadPoint:X4}" + Environment.NewLine);
            }
            resultText.Append("Header Length: " + this.headerLength + " bytes" + Environment.NewLine);
            return resultText.ToString();
        }

        internal string Read()
        {
            this.failed = false;
            //Read the file bytes (once) 
            if (this.fileBytes == null)
                this.fileBytes = File.ReadAllBytes(this.filename);

            string resultMessage = "";

            if (this.fileBytes.Length == 0)
            {
                resultMessage = "Error reading tap file: empty file." + Environment.NewLine;
                this.failed = true;
                return resultMessage;
            }

            resultMessage = ParseHeader();

            if (!this.failed)
            {
                this.program = new byte[this.programLength];
                resultMessage += GetProgramBytes(this.headerLength, this.programLength);
                resultMessage += ParseFooter(this.headerLength + this.programLength);
            }
            return resultMessage;
        }

        private string ParseFooter(int pos)
        {
            StringBuilder resultText = new StringBuilder();
            resultText.Append(Environment.NewLine + "Trailing Information: " + Environment.NewLine + "---------------------" + Environment.NewLine);

            if (this.fileType == 'M' || this.fileType == 'D')
            {
                this.checksum = this.fileBytes[pos++];
                this.mysteryByte = this.fileBytes[pos++];
                resultText.Append("Checksum: " + $"{this.checksum:X2}" + Environment.NewLine);
                resultText.Append("Mystery Byte: " + $"{this.mysteryByte:X2}" + Environment.NewLine);
            }
            else if (this.fileType == '9')
            {
                this.checksum = this.fileBytes[pos++];
                resultText.Append("Checksum: " + $"{this.checksum:X2}" + Environment.NewLine);
                // Ignore Padding and EOF (0x80)
                pos += 11;
            }

            if (this.fileType != '9' && this.fileType != 'D')
            {
                this.executionPoint = BitConverter.ToUInt16(new byte[2] { this.fileBytes[pos++], this.fileBytes[pos++] }, 0);
                resultText.Append("Execution Point: 0x" + $"{this.executionPoint:X4}" + Environment.NewLine + Environment.NewLine);

                //Skip the last byte
                pos++;
            }

            //Check for more taps...
            this.isLastTap = true;
            if ((this.fileBytes.Length > (pos + 2)))
            {
                if ((this.fileBytes[pos] == 0x22) || (this.fileBytes[pos] == 0xA5))
                {
                    resultText.Append(Environment.NewLine);
                    this.offset = pos;
                    this.isLastTap = false;
                    this.nextBytes = new byte[this.fileBytes.Length - pos];
                    Array.Copy(this.fileBytes, pos, this.nextBytes, 0, this.nextBytes.Length);
                }
            }
            //Result
            return resultText.ToString();
        }

        private string GetProgramBytes(int offset, int len)
        {
            StringBuilder resultText = new StringBuilder();
            try
            {
                int j = 0;
                string theAscii = "";
                resultText.Append(Environment.NewLine + "Program Bytes: " + Environment.NewLine + "--------------" + Environment.NewLine);

                // Display program bytes as hex
                for (int i = offset; i < offset + len; i++)
                {
                    this.program[i - offset] = this.fileBytes[i];
                    char theChar = (char)this.fileBytes[i];
                    resultText.Append($"{this.fileBytes[i]:X2} ");

                    // Display ascii representation
                    theAscii += ((char.IsWhiteSpace(theChar) || char.IsControl(theChar)) ? '.' : theChar);
                    j++;

                    if (j == 8)
                    {
                        resultText.Append(" " + theAscii + Environment.NewLine);
                        System.Windows.Forms.Application.DoEvents();
                        theAscii = "";
                        j = 0;
                    }
                }
                // End padding/spaces
                if (j > 0)
                {
                    for (int i = j; i < 8; i++)
                        resultText.Append("   ");
                    resultText.Append(" " + theAscii + Environment.NewLine);
                }
            }
            catch (System.IO.IOException err)
            {
                resultText.Append("Error reading tap file: I/O error reading program data: " + Environment.NewLine +
                    err.Message.ToString() + Environment.NewLine);
            }
            return resultText.ToString();
        }
    }
}
