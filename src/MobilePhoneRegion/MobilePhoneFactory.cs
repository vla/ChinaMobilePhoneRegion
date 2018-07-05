using System;
using System.IO;

namespace MobilePhoneRegion
{
    /// <summary>
    /// MobilePhoneFactory
    /// </summary>
    public class MobilePhoneFactory
    {
        /// <summary>
        /// 生成手机归属地数据源
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="data">手机归属地信息</param>
        /// <param name="stream">要写入的流</param>
        public static void Generate(Version version, MobilePhone[] data, Stream stream)
        {
            switch (version)
            {
                case Version.V1:
                    new Internal.V1.Generator(data).Write(stream);
                    break;
            }
        }

        /// <summary>
        /// 获取号码归属地查询器
        /// </summary>
        /// <param name="dataSource"><see cref="IDataSource"/></param>
        /// <returns><see cref="ISearcher"/></returns>
        /// <exception cref="FileLoadException">Invalid file version</exception>
        public static ISearcher GetSearcher(IDataSource dataSource)
        {
            var ver = dataSource.ReadByte(0);

            if (Enum.TryParse<Version>(ver.ToString(), out var version))
            {
                switch (version)
                {
                    case Version.V1:
                        return new Internal.V1.Searcher(dataSource);
                }
            }

            throw new FileLoadException("Invalid file version");
        }
    }
}