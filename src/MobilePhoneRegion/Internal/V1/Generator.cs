using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MobilePhoneRegion.Internal.V1
{
    /**
             文件存储结构 v1

             头部区
             +------------+------------+-----------+
             | 1bytes     | 4bytes     | 1bytes    |
             +------------+------------+-----------+
                 版本        号码总数     运营商总数

             号码数据存储区
             +------------+-------------------+--------------+-----------+
             | 2bytes     | 3bytes            | 2bytes       | 2bytes    |
             +------------+-------------------+--------------+-----------+
               号码前五位    连续数及号码后两位   行政区域索引    运营商索引

             行政区域存储区
             +------------+
             | 4bytes     |
             +------------+
              行政区域编码

             运营商存储区
             +------------+
             | 12bytes    |
             +------------+
               运营商字符
*/

    /// <summary>
    /// 提供手机归属地数据压缩储存操作
    /// </summary>
    internal class Generator : IGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Generator"/> class.
        /// </summary>
        /// <param name="data">手机归属地列表</param>
        /// <exception cref="ArgumentNullException">data</exception>
        /// <exception cref="ArgumentException">data</exception>
        public Generator(MobilePhone[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data.Length == 0)
                throw new ArgumentException(nameof(data) + " is empty");

            Data = data;
        }

        /// <summary>
        /// 手机归属地列表
        /// </summary>
        public MobilePhone[] Data { get; }

        public byte Version => 1;

        public void Write(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8))
            {
                var lstAdCode = new List<int>();    //行政编码索引数据
                var lstIsp = new List<string>();    //运营商的索引数据

                PrepareHead(bw);

                var count = WritePhone(bw, lstAdCode, lstIsp);

                FillHead(bw, count, lstIsp.Count);

                WriteAdCode(bw, lstAdCode);
                WriteIsp(bw, lstIsp);
            }
        }

        private int WritePhone(BinaryWriter bw, List<int> lstAdCode, List<string> lstIsp)
        {
            var idxPhone = 0;

            Store prev = new Store();

            for (int i = 0; i < Data.Length; i++)
            {
                var info = Data[i];

                if (i == 0)
                {
                    //先获取第一条记录向前比较
                    var idxAdCode = GetOrAdd(lstAdCode, info.AdCode);
                    var idxIsp = GetOrAdd(lstIsp, info.Isp);
                    prev = new Store(info.Phone, idxAdCode, idxIsp);
                }
                else
                {
                    var idxAdCode = GetOrAdd(lstAdCode, info.AdCode);
                    var idxIsp = GetOrAdd(lstIsp, info.Isp);

                    if (info.Phone - prev.GetPhone() == 1 && idxAdCode == prev.AdCodeIndex)
                    {
                        prev.SetSkip((ushort)(info.Phone - prev.GetBegin()));
                    }
                    else
                    {
                        ++idxPhone;

                        Write(bw, prev);

                        //更新当前比较的号码段内容
                        prev = new Store(info.Phone, idxAdCode, idxIsp);
                    }
                }
            }

            //最后一条递增数大于0的需要写入
            Write(bw, prev);
            ++idxPhone;

            return idxPhone;
        }

        private void WriteAdCode(BinaryWriter bw, List<int> list)
        {
            bw.Seek(0, SeekOrigin.End);

            foreach (var code in list)
            {
                bw.Write(code);
            }
        }

        private void WriteIsp(BinaryWriter bw, List<string> list)
        {
            bw.Seek(0, SeekOrigin.End);
            foreach (var code in list)
            {
                var data = Encoding.UTF8.GetBytes(code);
                bw.Write(data);
            }
        }

        private void PrepareHead(BinaryWriter bw)
        {
            bw.Seek(0, SeekOrigin.Begin);
            bw.Write(Version);//版本
            bw.Write(0);//手机号码总数
            bw.Write((byte)0xFF);//运营商总数
        }

        private void FillHead(BinaryWriter sw, int phoneCount, int ispCount)
        {
            sw.Seek(1, SeekOrigin.Begin);
            sw.Write(phoneCount);//手机号码总数
            sw.Write((byte)ispCount);//运营商总数
        }

        private ushort GetOrAdd<T>(IList<T> list, T item)
        {
            int index = list.IndexOf(item);

            if (index > -1)
            {
                return (ushort)index;
            }

            list.Add(item);

            return (ushort)(list.Count - 1);
        }

        private void Write(BinaryWriter bw, Store info)
        {
            bw.Write(info.Before);
            var data = BitConverter.GetBytes(info.After);
            bw.Write(data, 0, 3);
            bw.Write(info.AdCodeIndex);
            bw.Write(info.IspIndex);
        }
    }
}