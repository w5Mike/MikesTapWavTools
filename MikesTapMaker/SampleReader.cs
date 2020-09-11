using System;
using NAudio;
using NAudio.Wave;

namespace MikesTapMaker
{
    // SampleReader data conversion, reading in "blocks", and end-of-file flag - but not much else.
    //
    public class SampleReader
    {
        public SampleReader(NAudio.Wave.WaveFileReader reader)
        {
            this.waveFileReader = reader;
            this.waveFormat = waveFileReader.WaveFormat;
            this.blockOffset = 0;
            this.sampleSize = Convert.ToInt16(this.waveFormat.BitsPerSample / 8);
            // Buffer length must be a "block align" multiple
            this.bufferLength = (this.bufferLength / waveFileReader.BlockAlign) * waveFileReader.BlockAlign;
            this.buffer = new byte[this.bufferLength];
            this.sampleBlock = new short[this.bufferLength / this.sampleSize];
        }

        private NAudio.Wave.WaveFileReader waveFileReader;
        private WaveFormat waveFormat;
        protected short[] sampleBlock;

        protected int blockOffset = 0;      // current position in sample block
        private int blockCounter = 0;       // how many sample blocks have been read

        private int bufferLength = 1024;    // buffer length in bytes
        private byte[] buffer;

        private int lastByteRead = 0;
        private long totalBytesRead = 0;        // count of total bytes read (for EOF detection)

        private short sampleSize = 0;

        private bool endOfFile = false;     // boolean flag indicating EOF or not

        private int previousSample = 0;
        private int currentSample = 0;
        private int nextSample = 0;

        private void readSampleBlock()
        {
            this.lastByteRead = this.waveFileReader.Read(this.buffer, 0, this.bufferLength);
            this.totalBytesRead += this.lastByteRead;

            // Detect end-of-file
            if (this.totalBytesRead == this.waveFileReader.Length)
                this.endOfFile = true;

            //convert bytes to 16 bit shorts
            for (int i = 0; i < this.bufferLength / this.sampleSize; i++)
                this.sampleBlock[i] = BitConverter.ToInt16(this.buffer, i * this.sampleSize);

            if (this.blockCounter == 0)
            {
                //initialise rolling value
                previousSample = this.sampleBlock[0];
                currentSample = this.sampleBlock[0];
                nextSample = this.sampleBlock[0];
            }
            this.blockOffset = 0;
            this.blockCounter++;
        }

        // Return the "next" sample (the first sample block is loaded on init)
        protected short readNextSample()
        {
            if (this.EndOfFile)
                return 0;

            if ((this.sampleBlock == null) || (this.blockOffset == this.sampleBlock.Length))
                this.readSampleBlock();

            // Simple averaging to remove minor "blips"
            previousSample = currentSample;
            currentSample = nextSample;
            nextSample = this.sampleBlock[this.blockOffset++];
            return (Convert.ToInt16((previousSample + currentSample + nextSample) / 3));
        }

        // WaveFormat for convenience
        public WaveFormat WaveFormat
        {
            get { return this.waveFormat; }
        }

        public int BitsPerSample
        {
            get { return this.waveFormat.BitsPerSample; }
        }

        public int SampleRate
        {
            get { return this.waveFormat.SampleRate; }
        }

        protected int currentPosition
        {
            get { return (((this.blockCounter - 1) * sampleBlock.Length) + this.blockOffset); }
        }

        public bool EndOfFile
        {
            get { return this.endOfFile; }
        }

    }
}