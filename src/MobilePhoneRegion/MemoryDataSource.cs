using System;
using System.IO;
using System.Text;

namespace MobilePhoneRegion
{
    /// <summary>
    /// Memory-based data sources
    /// </summary>
    /// <seealso cref="IDataSource" />
    public class MemoryDataSource : IDataSource
    {
        private readonly byte[] DataSource;

        public MemoryDataSource(byte[] data)
        {
            DataSource = data;
        }

        public MemoryDataSource(string filename)
        {
            DataSource = File.ReadAllBytes(filename);
        }

        public MemoryDataSource(Stream stream)
        {
            if (stream.CanSeek)
            {
                DataSource = new byte[stream.Length];
                stream.Read(DataSource, 0, DataSource.Length);
            }
            else
            {
                using (var mem = new MemoryStream())
                {
                    stream.CopyTo(mem);
                    DataSource = mem.ToArray();
                }
            }
        }

        public long Length => DataSource.Length;

        public byte ReadByte(int position)
        {
            if (position < 0 || position > DataSource.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            return DataSource[position];
        }

        public int ReadData(int position, ref byte[] data)
        {
            if (position < 0 || position > DataSource.Length || data.Length > DataSource.Length - position)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }
            int i = 0;
            for (; i < data.Length; i++)
            {
                data[i] = DataSource[i + position];
            }
            return i;
        }

        public string ReadString(int position, int count)
        {
            if (position < 0 || position > DataSource.Length || count > DataSource.Length - position)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            return Encoding.UTF8.GetString(DataSource, position, count);
        }

        public ushort ReadUInt16(int position)
        {
            if (position < 0 || position > DataSource.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            return BitConverter.ToUInt16(DataSource, position);
        }

        public short ReadInt16(int position)
        {
            if (position < 0 || position > DataSource.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            return BitConverter.ToInt16(DataSource, position);
        }

        public uint ReadUInt32(int position)
        {
            if (position < 0 || position > DataSource.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            return BitConverter.ToUInt32(DataSource, position);
        }

        public int ReadInt32(int position)
        {
            if (position < 0 || position > DataSource.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            return BitConverter.ToInt32(DataSource, position);
        }

        public unsafe int ReadInt32In3Bit(int position)
        {
            fixed (byte* pbyte = &DataSource[position])
            {
                return (*pbyte) | (*(pbyte + 1) << 8) | (*(pbyte + 2) << 16);
            }
        }

        public ulong ReadUInt64(int position)
        {
            if (position < 0 || position > DataSource.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            return BitConverter.ToUInt64(DataSource, position);
        }

        public long ReadInt64(int position)
        {
            if (position < 0 || position > DataSource.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            return BitConverter.ToInt64(DataSource, position);
        }

        public void Dispose()
        {

        }
    }
}