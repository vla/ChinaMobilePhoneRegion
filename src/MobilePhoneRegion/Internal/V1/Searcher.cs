using System;

namespace MobilePhoneRegion.Internal.V1
{
    /// <summary>
    /// 手机归属地查询
    /// </summary>
    internal class Searcher : ISearcher
    {
        private const int head = 6;             //头部占用6个字节
        private const int storeSize = 9;        //内容占用字节
        private const int isp_char_length = 12; //运营商固定长度
        private int isp_count;       //运营商总数

        public Searcher(IDataSource dataSource)
        {
            DataSource = dataSource;

            LoadData();
        }

        public byte Version => DataSource.ReadByte(0);

        public int Count { get; private set; }

        public IDataSource DataSource { get; private set; }

        public SearchResult Search(int number)
        {
            int position = 0,
                left = 0,
                right = Count - 1;

            SearchResult result = new SearchResult();

            var store = new Store();

            while (left <= right)
            {
                int middle = (left + right) / 2;

                position = head + middle * storeSize;

                FillStore(ref store, position);

                if (number < store.GetBegin())
                {
                    right = middle - 1;
                }
                else if (number > store.GetPhone())
                {
                    left = middle + 1;
                }
                else
                {
                    result.Success = true;
                    result.AdCode = FindAdCode(ref store);
                    result.Isp = FindIsp(ref store);
                    break;
                }
            }

            return result;
        }

        private void LoadData()
        {
            Count = DataSource.ReadInt32(1);
            isp_count = DataSource.ReadByte(5);
        }

        private void FillStore(ref Store store, int position)
        {
            if (DataSource is MemoryDataSource)
            {
                store.Before = DataSource.ReadUInt16(position);
                store.After = DataSource.ReadInt32In3Bit(position + 2);
                store.AdCodeIndex = DataSource.ReadUInt16(position + 5);
                store.IspIndex = DataSource.ReadUInt16(position + 7);
            }
            else
            {
                var data = new byte[storeSize];

                DataSource.ReadData(position, ref data);

                store.Before = BitConverter.ToUInt16(data, 0);
                store.After = data[2] | data[3] << 8 | data[4] << 16;
                store.AdCodeIndex = BitConverter.ToUInt16(data, 5);
                store.IspIndex = BitConverter.ToUInt16(data, 7);
            }
        }

        private int FindAdCode(ref Store store)
        {
            //偏移量 = 头部长度 + (号码总数 * 号码占用长度) + (行政区域位置 * 行政区域占用长度)
            var position = head + (Count * storeSize) + (store.AdCodeIndex * 4);
            return DataSource.ReadInt32(position);
        }

        private string FindIsp(ref Store store)
        {
            //运营商存储在尾端
            //偏移量 = （流的总长度 - 运营商总数 * isp字符长度）+ (所在位置 * isp字符长度)
            var position = (DataSource.Length - isp_count * isp_char_length) + (store.IspIndex * isp_char_length);
            var isp = DataSource.ReadString((int)position, isp_char_length);
            return isp;
        }
    }
}