using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MobilePhoneRegion.Internal.V2
{
    /**
        文件存储结构

        头部区
        +------------+------------------+
        | 1bytes     | 4bytes           |
        +------------+------------------+
            版本        号码起始流位置

        行政编码存储区
        +------------+
        | 4bytes     |
        +------------+
            编码

        运营商存储区
        +------------+------------+
        | 1bytes     | data bytes |
        +------------+------------+
            字符长度      运营商字符

        号码存储区
        +------------+-------------------+--------------+-----------+
        | 2bytes     | 3bytes            | 2bytes       | 4bytes    |
        +------------+-------------------+--------------+-----------+
          号码前五位    连续数及号码后两位   行政区域索引    运营商流位置

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
        public MobilePhone[] Data { get; private set; }

        public byte Version => 2;

        public void Write(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8))
            {
                var lstIsp = new List<string>();    //运营商的索引数据

                PrepareHead(bw);

                var lstAdCode = PrepareAdCode(bw);
                var dictIsp = PrepareISP(bw);

                WritePhone(bw, lstAdCode, dictIsp);
            }
        }

        private void WritePhone(BinaryWriter bw, IList<int> lstAdCode, IDictionary<string, uint> dictIsp)
        {
            //设置号码起始流位置
            Write(bw, 1, (int)bw.BaseStream.Position);
            bw.Seek(0, SeekOrigin.End);
            Store prev = new Store();

            for (int i = 0; i < Data.Length; i++)
            {
                var info = Data[i];

                if (i == 0)
                {
                    //先获取第一条记录向前比较
                    var idxAdCode = lstAdCode.IndexOf(info.AdCode);
                    dictIsp.TryGetValue(info.Isp, out uint isp_offset);
                    prev = new Store(info.Phone, (ushort)idxAdCode, isp_offset);
                }
                else
                {
                    var idxAdCode = lstAdCode.IndexOf(info.AdCode);
                    dictIsp.TryGetValue(info.Isp, out uint isp_offset);

                    if (info.Phone - prev.GetPhone() == 1 && idxAdCode == prev.AdCodeIndex)
                    {
                        prev.SetSkip((ushort)(info.Phone - prev.GetBegin()));
                    }
                    else
                    {
                        Write(bw, prev);

                        //更新当前比较的号码段内容
                        prev = new Store(info.Phone, (ushort)idxAdCode, isp_offset);
                    }
                }
            }

            //最后一条递增数大于0的需要写入
            if (prev.GetSkip() > 0)
            {
                Write(bw, prev);
            }
        }

        private void PrepareHead(BinaryWriter bw)
        {
            bw.Seek(0, SeekOrigin.Begin);
            bw.Write(Version);//版本
            bw.Write(0);//号码起始流位置
        }

        private IList<int> PrepareAdCode(BinaryWriter bw)
        {
            var list = Data.Select(w => w.AdCode).Distinct().ToList();
            bw.Seek(0, SeekOrigin.End);
            foreach (var code in list)
            {
                bw.Write(code);
            }
            return list;
        }

        private IDictionary<string, uint> PrepareISP(BinaryWriter bw)
        {
            var dict_isp = new Dictionary<string, uint>();
            var list = Data.Select(w => w.Isp).Distinct();

            bw.Seek(0, SeekOrigin.End);
            foreach (var code in list)
            {
                var data = Encoding.UTF8.GetBytes(code);
                dict_isp[code] = (uint)bw.BaseStream.Position;
                bw.Write((byte)data.Length);
                bw.Write(data);
            }

            return dict_isp;
        }

        private void Write(BinaryWriter bw, int offset, int val)
        {
            bw.Seek(offset, SeekOrigin.Begin);
            bw.Write(val);
        }

        private void Write(BinaryWriter bw, Store info)
        {
            bw.Write(info.Before);
            var data = BitConverter.GetBytes(info.After);
            bw.Write(data, 0, 3);
            bw.Write(info.AdCodeIndex);
            bw.Write(info.IspOffset);
        }
    }
}