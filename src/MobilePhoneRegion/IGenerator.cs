using System.IO;

namespace MobilePhoneRegion
{
    /// <summary>
    /// 手机归属地生成
    /// </summary>
    public interface IGenerator
    {
        /// <summary>
        /// 将数据写入流
        /// </summary>
        /// <param name="stream"></param>
        void Write(Stream stream);

        /// <summary>
        /// 版本
        /// </summary>
        byte Version { get; }
    }
}