using System;
using System.IO;

namespace MikesTapMaker
{
    public class TapFile
    {
        internal string tapFilename = "";
        internal string loadName = "";
        internal char fileType = 'B';
        internal ushort programLength = 0;
        internal byte[] program;
        internal ushort loadPoint = 0;
        internal byte checksum = 0;
        internal byte mysteryByte = 0;

        // -ve offset from the end of program (in 2s comp)
        internal UInt16 executionPoint = 0;

        internal string TapName = "";

        // Little endian. Probably not needed (already little edian)
        public ushort SwapBytes(ushort x)
        {
            return (ushort)((ushort)(x << 8) | (x >> 8));
        }

        internal bool WriteTapFile(string filename)
        {
            try
            {
                using (BinaryWriter binWriter = new BinaryWriter(new FileStream(filename, FileMode.Create)))
                {
                    if (this.fileType == '9')
                    {
                        // Level 9 Header
                        binWriter.Write((byte)0xA5);
                        binWriter.Write((byte)0x41);
                        binWriter.Write(BitConverter.GetBytes(this.loadPoint), 0, 2);
                        binWriter.Write(BitConverter.GetBytes(this.programLength), 0, 2);
                    }
                    else
                    {
                        // Basic/Machine Code header
                        binWriter.Write(this.loadName.ToCharArray(), 0, this.loadName.Length);
                        binWriter.Write(this.fileType);
                        binWriter.Write(BitConverter.GetBytes(this.programLength), 0, 2);
                        if (this.fileType == 'M')
                            binWriter.Write(BitConverter.GetBytes(this.loadPoint), 0, 2);
                    }
                    // Program (all file types)
                    binWriter.Write(this.program, 0, this.program.Length);

                    // Trailing info
                    if (this.fileType == 'M')
                    {
                        binWriter.Write(this.checksum);
                        binWriter.Write(this.mysteryByte);
                    }
                    if (this.fileType == '9')
                    {
                        binWriter.Write(this.checksum);
                        byte[] padding = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                        binWriter.Write(padding);
                        binWriter.Write((byte)0x80);
                    }
                    else
                    {
                        byte[] exPointBytes = BitConverter.GetBytes(this.executionPoint);
                        binWriter.Write(exPointBytes);
                        binWriter.Write(exPointBytes[1]); // High byte, for some reason.
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
