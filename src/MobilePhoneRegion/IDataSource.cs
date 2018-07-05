using System;

namespace MobilePhoneRegion
{
    /// <summary>
    /// phone data source
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IDataSource : IDisposable
    {
        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        long Length { get; }

        /// <summary>
        /// Reads the byte.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        byte ReadByte(int position);

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="data">The data.</param>
        int ReadData(int position, ref byte[] data);

        /// <summary>
        /// Reads the int16.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        short ReadInt16(int position);

        /// <summary>
        /// Reads the int32.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        int ReadInt32(int position);

        /// <summary>
        /// Reads the int32 in 3bit.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        int ReadInt32In3Bit(int position);

        /// <summary>
        /// Reads the int64.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        long ReadInt64(int position);

        /// <summary>
        /// Reads the string.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        string ReadString(int position, int count);

        /// <summary>
        /// Reads the u int16.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        ushort ReadUInt16(int position);

        /// <summary>
        /// Reads the u int32.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        uint ReadUInt32(int position);

        /// <summary>
        /// Reads the u int64.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        ulong ReadUInt64(int position);
    }
}