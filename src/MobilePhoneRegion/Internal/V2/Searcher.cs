using System;

namespace MobilePhoneRegion.Internal.V2
{
    /// <summary>
    /// 手机归属地查询
    /// </summary>
    internal class Searcher : ISearcher
    {
        private const int head = 5;             //头部字节
        private const int storeSize = 11;       //号码数据块区字节

        private int phone_start_pos; //号码起始流位置

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

                position = phone_start_pos + middle * storeSize;

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
            phone_start_pos = DataSource.ReadInt32(1);
            Count = (int)(DataSource.Length - phone_start_pos) / storeSize;
        }

        private void FillStore(ref Store store, int position)
        {
            if (DataSource is MemoryDataSource)
            {
                store.Before = DataSource.ReadUInt16(position);
                store.After = DataSource.ReadInt32In3Bit(position + 2);
                store.AdCodeIndex = DataSource.ReadUInt16(position + 5);
                store.IspOffset = DataSource.ReadUInt32(position + 7);
            }
            else
            {
                var data = new byte[storeSize];

                DataSource.ReadData(position, ref data);

                store.Before = BitConverter.ToUInt16(data, 0);
                store.After = data[2] | data[3] << 8 | data[4] << 16;
                store.AdCodeIndex = BitConverter.ToUInt16(data, 5);
                store.IspOffset = BitConverter.ToUInt32(data, 7);
            }
        }

        private int FindAdCode(ref Store store)
        {
            var position = head + store.AdCodeIndex * 4;
            return DataSource.ReadInt32(position);
        }

        private string FindIsp(ref Store store)
        {
            var offset = (int)store.IspOffset;
            var len = DataSource.ReadByte(offset);
            var isp = DataSource.ReadString(offset + 1, len);
            return isp;
        }
    }
}