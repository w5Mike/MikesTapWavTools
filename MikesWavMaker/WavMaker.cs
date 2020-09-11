using System;
using System.IO;
using System.Media;

namespace MikesWavMaker
{
    public class WavMaker : IDisposable
    {
        // WAV File Header Constants
        private const int ID_RIFF = 1179011410;
        private const int ID_WAVE = 1163280727;
        private const int ID_FMT = 544501094;
        private const int ID_DATA = 1635017060;

        // Private members
        private BinaryWriter binaryWriter;
        private float volume;
        private int sampleRateHz;
        private int gapSize; // Seconds
        private int bytesPerSecond;
        private short bitsPerSample;

        // Wave data buffers, for performance.
        private byte[] zeroWave;
        private byte[] oneWave;
        private int outByteCounter = 0;

        //internal byte[] WavFile;

        internal WavMaker(float volume, int sampleRate, short bits)
        {
            this.volume = volume;
            this.sampleRateHz = sampleRate;
            this.bytesPerSecond = sampleRateHz * (bitsPerSample / 8);
            this.gapSize = 6 * sampleRateHz;
            this.binaryWriter = new BinaryWriter(new MemoryStream());
            this.bitsPerSample = bits;
            InitialiseWaves();
        }

        private void InitialiseWaves()
        {
            // Theory:
            //   Tape 0 is 600 baud 
            //   no of samples == 22050khz/600baud 
            //   => 36.75 => (24.5 + 49)/2 (average "sample/wave" = bit-rate)
            //   => zeroWaveLength == 24.5 @ 22050Hz (approx ~25 samples)
            //   => zeroWaveLength == 49 @ 22050Hz (approx ~50 samples)
            //
            float zeroWaveLength = 25 * sampleRateHz / 22050F;
            float oneWaveLength = zeroWaveLength * 2F;
            this.zeroWave = new byte[(int)zeroWaveLength * 2];
            this.oneWave = new byte[(int)oneWaveLength * 2];
            float maxValue = (float)short.MaxValue * this.volume;
            short counter = 0;
            for (float i = 0; (i < (Math.PI * 2)) && (counter < zeroWaveLength); i += ((2 * (float)Math.PI) / zeroWaveLength))
            {
                BitConverter.GetBytes((short)(maxValue * (0 - Math.Sin(i)))).CopyTo(this.zeroWave, counter * 2);
                counter++;
            }

            counter = 0;
            for (float i = 0; (i < (Math.PI * 2)) && (counter < oneWaveLength); i += ((2 * (float)Math.PI) / oneWaveLength))
            {
                BitConverter.GetBytes((short)(maxValue * (0 - Math.Sin(i)))).CopyTo(this.oneWave, counter * 2);
                counter++;
            }
        }

        private void MakeWavHeader(int sizeInBytes)
        {
            this.binaryWriter.Write(ID_RIFF);
            this.binaryWriter.Write(36 + sizeInBytes); //TODO: Update retrospectively
            this.binaryWriter.Write(ID_WAVE);
            this.binaryWriter.Write(ID_FMT);
            this.binaryWriter.Write(16);        //chunk size (16 for PCM)
            this.binaryWriter.Write((short)1);
            this.binaryWriter.Write((short)1);
            this.binaryWriter.Write(sampleRateHz);
            this.binaryWriter.Write(sampleRateHz * 2);
            this.binaryWriter.Write((short)2);
            this.binaryWriter.Write((short)16);
            this.binaryWriter.Write(ID_DATA);
            this.binaryWriter.Write(sizeInBytes);
        }

        private void WriteBitWave(int bit)
        {
            if (bit == 0)
            {
                this.binaryWriter.Write(this.zeroWave);
                outByteCounter += this.zeroWave.Length;
            }
            else
            {
                this.binaryWriter.Write(this.oneWave);
                outByteCounter += this.oneWave.Length;
            }
        }

        internal void WriteByte(byte theByte)
        {
            for (int i = 7; i >= 0; i--)
                WriteBitWave((theByte >> i) & 1);
        }

        internal bool MakeWav(TapFile[] tapList, string filename)
        {
            try
            {

                this.binaryWriter = new BinaryWriter(new MemoryStream());
                this.MakeWavHeader(0);

                foreach (TapFile tap in tapList)
                {
                    // Output sync zeros for 2 seconds (882 samples == 1 second @ 22050 Hz) 
                    for (int i = 0; i < 222; i++)
                        this.WriteByte(0);

                    //Output A5 byte
                    this.WriteByte(0xA5);

                    byte[] bytes = BitConverter.GetBytes(tap.programLength);

                    if (tap.fileType == '9')
                    {
                        //Output 'A' character
                        this.WriteByte(0x41);

                        //Output load point
                        bytes = BitConverter.GetBytes(tap.loadPoint);
                        this.WriteByte(bytes[0]);
                        this.WriteByte(bytes[1]);
                    }
                    else
                    {
                        //Output load name in quotes 
                        this.WriteByte(0x22);
                        for (int i = 0; i < tap.loadName.Length; i++)
                            this.WriteByte((byte)tap.loadName[i]);
                        this.WriteByte(0x22);

                        // More sync zeros
                        for (int i = 0; i < 222; i++)
                            this.WriteByte(0);

                        //Output A5 byte
                        this.WriteByte(0xA5);

                        //Output M/B filetype byte
                        this.WriteByte((byte)tap.fileType);
                    }

                    //Output program length (little endian)
                    bytes = BitConverter.GetBytes(tap.programLength);
                    this.WriteByte(bytes[0]);
                    this.WriteByte(bytes[1]);

                    //Output load point
                    if (tap.fileType == 'M')
                    {
                        bytes = BitConverter.GetBytes(tap.loadPoint);
                        this.WriteByte(bytes[0]);
                        this.WriteByte(bytes[1]);
                    }

                    //Output program bytes
                    for (int i = 0; i < tap.programLength; i++)
                        this.WriteByte(tap.program[i]);

                    //Output checksum
                    if ((tap.fileType == 'M') || (tap.fileType == '9'))
                        this.WriteByte(tap.checksum);

                    if (tap.fileType == 'M')
                        this.WriteByte(tap.mysteryByte);

                    if ((tap.fileType == 'M') || (tap.fileType == 'B'))
                    {
                        //output excution point
                        bytes = BitConverter.GetBytes(tap.executionPoint);
                        this.WriteByte(bytes[0]);
                        this.WriteByte(bytes[1]);
                        this.WriteByte(bytes[1]);
                    }

                    if (tap.fileType == '9')
                    {
                        // Handle Level9 end zeros
                        byte[] padding = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                        for (int i = 0; i < 10; i++)
                            this.WriteByte(0);
                        //EOF marker?
                        this.WriteByte(0x80);
                    }

                    // output zero samples, just for neatness.
                    for (int i = 0; i < gapSize; i++)
                        this.binaryWriter.Write((ushort)0);
                }

                // Handle filesizes in Wav header retrospectively
                this.binaryWriter.Seek(4, SeekOrigin.Begin);
                this.binaryWriter.Write(36 + this.outByteCounter + (2 * gapSize * tapList.Length));
                this.binaryWriter.Seek(40, SeekOrigin.Begin);
                this.binaryWriter.Write(this.outByteCounter + (2 * gapSize * tapList.Length));
                this.binaryWriter.Seek(0, SeekOrigin.Begin);

                // Save wav file
                using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    MemoryStream mem = (MemoryStream)this.binaryWriter.BaseStream;
                    mem.WriteTo(file);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal void PlayWav()
        {
            this.binaryWriter.Seek(0, SeekOrigin.Begin);
            SoundPlayer soundPlayer = new SoundPlayer(this.binaryWriter.BaseStream);
            soundPlayer.PlaySync();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (this.binaryWriter != null)
                {
                    this.binaryWriter.Dispose();
                    this.binaryWriter = null;
                }
            }
        }
    }
}
