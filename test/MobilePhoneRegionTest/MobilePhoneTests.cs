using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MobilePhoneRegion;
using Xunit;

namespace MobilePhoneRegionTest
{
    public class MobilePhoneTests
    {
        [Fact]
        public void Writer_And_Reader_V1()
        {
            var filename = Path.Combine(AppContext.BaseDirectory, "phones.txt");
            var dest = Path.Combine(AppContext.BaseDirectory, "v1.dat");

            using (var fs = File.Create(dest))
            {
                MobilePhoneFactory.Generate(MobilePhoneRegion.Version.V2, Helper.GetPhoneList(filename).ToArray(), fs);
            }

            var dataSource = new StreamDataSource(dest);

            var searcher = MobilePhoneFactory.GetSearcher(dataSource);

            var result = searcher.Search(1370233);

            Assert.True(result.Success);

            dataSource.Dispose();
        }

        [Fact]
        public void Check_ALL_Match_V2()
        {
            var filename = Path.Combine(AppContext.BaseDirectory, "phones.txt");
            var dest = Path.Combine(AppContext.BaseDirectory, "v2.dat");
            var data = Helper.GetPhoneList(filename).ToArray();

            using (var fs = File.Create(dest))
            {
                MobilePhoneFactory.Generate(MobilePhoneRegion.Version.V2, data, fs);
            }

            var dataSource = new MemoryDataSource(dest);

            var searcher = MobilePhoneFactory.GetSearcher(dataSource);
            SearchResult result;
            foreach (var info in data)
            {
                result = searcher.Search(info.Phone);
                Assert.True(result.Success);
            }

            dataSource.Dispose();
        }

        [Fact]
        public void Check_ALL_Match_Inner_DataSource()
        {
            var filename = Path.Combine(AppContext.BaseDirectory, "phones.txt");
            var data = Helper.GetPhoneList(filename).ToArray();

            var searcher = MobilePhoneFactory.GetSearcher();
            SearchResult result;
            foreach (var info in data)
            {
                result = searcher.Search(info.Phone);
                Assert.True(result.Success);
            }
        }
    }
}
