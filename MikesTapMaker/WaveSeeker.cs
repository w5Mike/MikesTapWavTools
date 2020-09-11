using System;

namespace MikesTapMaker
{
    public struct WavePositionsStruct
    {
        public long start, max, crossover, min;

        public WavePositionsStruct(long start, long max, long crossover, long min)
        {
            this.start = start;
            this.max = max;
            this.crossover = crossover;
            this.min = min;
        }
    }

    public class WaveSeeker : SampleReader
    {
        private short currentSample = 0;
        private long zeroWaveStart = -1;

        private int whatIsBit()
        {
            //TODO: hardcoded constants - should be based on sample rate, etc.
            //TODO: Do we care about wave height/amplitude?

            long length = this.WavePositions.max - this.WavePositions.start;
            return ((length > 27) ? 1 : 0);
        }

        internal WavePositionsStruct WavePositions;

        internal NAudio.Wave.WaveFileReader WaveFile;

        public WaveSeeker(NAudio.Wave.WaveFileReader reader)
            : base(reader)
        {
            currentSample = base.readNextSample();
            this.WaveFile = reader;
        }

        internal WavePositionsStruct findSyncZero()
        {
            do
                this.WavePositions = this.seekWave();
            while
                ((this.whatIsBit() == 1) && !this.EndOfFile);

            // Statistics
            this.zeroWaveStart = this.WavePositions.start;
            // TODO: what if it's not a zero sync wave? Keep searching? iterate to end of file
            return this.WavePositions;
        }

        // Search for the A5 byte -> Binary(10100101)
        internal bool seekA5Byte()
        {
            int theByte;

            // Skip past the "zero waves", until reaching a "one wave"
            do
            {
                this.WavePositions = this.seekWave();
            }
            while
                ((this.whatIsBit() == 0) && !this.EndOfFile);

            if (this.EndOfFile)
                return false;

            theByte = 0;

            for (int i = 0; i < 8; i++)
            {
                // Find the next "wave"
                if (i != 0)
                    this.WavePositions = this.seekWave();
                // build the byte based on wave "type"
                theByte = theByte | (this.whatIsBit() << (7 - i));
            }
            //Binary(10100101)
            return (theByte == 0xA5);
        }

        // Read/assemble the next byte
        internal bool readByte(out byte result)
        {
            int theByte = 0;
            result = 0;

            if (this.EndOfFile)
                return false;

            for (int i = 0; i < 8; i++)
            {
                //seek next wave
                this.WavePositions = this.seekWave();
                if ((this.WavePositions.max == -1) || this.EndOfFile)
                    return false;
                else
                    // build the byte based on wave "type"
                    theByte = theByte | (this.whatIsBit() << (7 - i));
            }
            result = (byte)theByte;
            return true;
        }

        // Searches for one wavelength beginning, minimum, crossover, maximum
        // (ignore end position, because that's the next start position)
        internal WavePositionsStruct seekWave()
        {
            WavePositionsStruct falseResult = new WavePositionsStruct(-1, -1, -1, -1);
            WavePositionsStruct result = new WavePositionsStruct(0, 0, 0, 0);

            int position = -1;

            if ((position = this.seekCrossunder()) == -1)
                return falseResult;
            else
                result.start = position;

            if ((position = this.seekMin()) == -1)
                return falseResult;
            else
                result.min = position;

            if ((position = this.seekCrossover()) == -1)
                return falseResult;
            else
                result.crossover = position;

            if ((position = this.seekMax()) == -1)
                return falseResult;
            else
                result.max = position;

            return result;
        }

        // Read/parse "load filname"
        internal string readLoadName()
        {
            byte theByte = 0;
            string result = "";

            if ((!this.readByte(out theByte)) || (theByte != 0x22))
            {
                if (theByte == 0)
                    return "";
                else if (theByte == 0x41)
                {
                    return "<Level9>";
                }
            }

            result += (char)theByte;
            theByte = 0;

            while ((!this.EndOfFile) && (theByte != 0x22))
            {
                this.readByte(out theByte);
                result += (char)theByte;
            }

            return (this.EndOfFile ? "" : result);
        }

        internal char readFileType()
        {
            byte theByte = 0;

            if ((!this.readByte(out theByte)) || this.EndOfFile)
                return Convert.ToChar(0);

            // Check if 'B' or 'M'
            if ((theByte == 66) || (theByte == 77))
                return Convert.ToChar(theByte);
            else
                return Convert.ToChar(0);
        }

        internal bool readChecksum(out byte result)
        {
            byte checksum1;
            result = 0;

            if ((!this.readByte(out checksum1)) || this.EndOfFile)
                return false;

            result = checksum1;
            return true;
        }

        internal ushort readUShort()
        {
            byte[] value = new byte[2];

            if (!this.readByte(out value[0]) || !this.readByte(out value[1]))
                return (ushort)0;

            return BitConverter.ToUInt16(value, 0);
        }

        internal ushort read8Bytes(byte[] theBytes)
        {
            ushort byteCount = 0;

            while ((!this.EndOfFile) && (byteCount < 8))
                this.readByte(out theBytes[byteCount++]);
            return byteCount;
        }

        // Reads N bytes into "program", and returns byte count (should match length)
        internal ushort readProgram(byte[] program, ushort length)
        {
            ushort byteCount = 0;

            while ((!this.EndOfFile) && (byteCount < length))
            {
                this.readByte(out program[byteCount++]);
            }
            return byteCount;
        }

        // locates next upwards crossing of X axis
        private int seekCrossover()
        {
            short nextSample;

            while (!this.EndOfFile)
            {
                nextSample = this.readNextSample();
                if ((this.currentSample < 0) && (nextSample >= 0))
                {
                    this.currentSample = nextSample;
                    return this.currentPosition - 1;      // "current" position is now "next" sample
                }
                this.currentSample = nextSample;
            }
            // if not found
            return -1;
        }

        // locates next downwards crossing of X axis (start of wave)
        private int seekCrossunder()
        {
            short nextSample;

            while (!this.EndOfFile)
            {
                nextSample = this.readNextSample();
                if ((this.currentSample > 0) && (nextSample <= 0))
                {
                    this.currentSample = nextSample;
                    return this.currentPosition - 1;      // "current" position is now "next" sample
                }
                this.currentSample = nextSample;
            }
            // if not found
            return -1;
        }

        //// locates next minimum 
        private int seekMin()
        {
            short nextSample;

            while (!this.EndOfFile)
            {
                nextSample = this.readNextSample();
                if (this.currentSample < nextSample)
                {
                    this.currentSample = nextSample;
                    return this.currentPosition - 1;      // "current" position is now "next" sample
                }
                this.currentSample = nextSample;
            }
            // if not found
            return -1;
        }

        //// locates next maximum 
        private int seekMax()
        {
            short nextSample;

            while (!this.EndOfFile)
            {
                nextSample = this.readNextSample();
                if (this.currentSample > nextSample)
                {
                    this.currentSample = nextSample;
                    return this.currentPosition - 1;      // "current" position is now "next" sample
                }
                this.currentSample = nextSample;
            }
            // if not found
            return -1;
        }
    }
}
