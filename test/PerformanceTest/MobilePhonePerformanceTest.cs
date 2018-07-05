using System;
using System.IO;
using System.Linq;
using MobilePhoneRegion;

namespace PerformanceTest
{
    public class MobilePhonePerformanceTest
    {
        [Test("内置数据源性能测试")]
        public void InnerDataSourceSearch()
        {
            Helper.PrintMem();

            var searcher = MobilePhoneFactory.GetSearcher();

            //预热
            searcher.TryGet("13702331111", out var info);
            searcher.Search(1370233);

            Helper.Time("单条查询", () =>
            {
                searcher.Search(1370233);
            });

            int count = 20000000;

            Helper.Time("批量查询", () =>
            {
                searcher.Search(1370233);
            }, count);

            Helper.TimeWithParallel("并发查询", () =>
            {
                searcher.Search(1370233);
            }, count);

            Helper.TimeWithThread("多线程查询", () =>
            {
                searcher.Search(1370233);
            }, 4, count);
        }

        [Test("文件IO数据源性能测试")]
        public void StreamDataSourceSearch()
        {
            Helper.PrintMem();

            var dest = Path.Combine(AppContext.BaseDirectory, "MobilePhoneRegion.dat");

            if (!File.Exists(dest))
            {
                var filename = Path.Combine(AppContext.BaseDirectory, "phones.txt");

                using (var fs = File.Create(dest))
                {
                    MobilePhoneFactory.Generate(MobilePhoneRegion.Version.V2, Helper.GetPhoneList(filename).ToArray(), fs);
                }
            }

            var dataSource = new StreamDataSource(dest);

            var searcher = MobilePhoneFactory.GetSearcher(dataSource);

            //预热
            searcher.TryGet("13702331111", out var info);
            searcher.Search(1370233);

            Helper.Time("单条查询", () =>
            {
                searcher.Search(1370233);
            });

            int count = 10000;

            Helper.Time("批量查询", () =>
            {
                searcher.Search(1370233);
            }, count);

            Helper.TimeWithParallel("并发查询", () =>
            {
                searcher.Search(1370233);
            }, count);

            Helper.TimeWithThread("多线程查询", () =>
            {
                searcher.Search(1370233);
            }, 4, count);
        }
    }
}