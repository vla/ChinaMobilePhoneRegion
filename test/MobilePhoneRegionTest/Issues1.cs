using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MobilePhoneRegion;
using Xunit;

namespace MobilePhoneRegionTest
{
    public class Issues1
    {
        [Fact]
        public void Check_Mobile17X()
        {
            var searcher = MobilePhoneFactory.GetSearcher();

            var result = searcher.TryGet("17093318888", out MobilePhone info);
            Assert.True(result);

            result = searcher.TryGet("16677408888", out info);
            Assert.True(result);

            result = searcher.TryGet("19893318888", out info);
            Assert.True(result);

            result = searcher.TryGet("19999798888", out info);
            Assert.True(result);
        }
    }
}
