using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MobilePhoneRegion;

namespace PerformanceTest
{
    public class ChinaAdCodePerformanceTest
    {
        [Test("行政区域查询性能")]
        public void Search()
        {
            Helper.PrintMem();

            //预热
            ChinaAdCode.Search("广东省");

            Helper.Time("编码查询", () =>
            {
                var info = ChinaAdCode.Get(440402);
            }, 1000000);

            Helper.Time("省份查询", () =>
            {
                var c = ChinaAdCode.Search("广东省");
            }, 1000000);

            Helper.Time("省市查询", () =>
            {
                var c = ChinaAdCode.Search("广东省", "深圳");
            }, 1000000);

            Helper.Time("省市县查询", () =>
            {
                var c = ChinaAdCode.Search("广东省", "珠海", "香洲");
            }, 1000000);

        }
    }
}
