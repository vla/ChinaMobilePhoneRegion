using System;
using System.IO;
using System.Linq;
using MobilePhoneRegion;
using Xunit;

namespace MobilePhoneRegionTest
{
    public class ChinaAdCodeTests
    {
        [Fact]
        public void Can_Match_AdCode()
        {
            var adCode = ChinaAdCode.Get(110107);

            Assert.Equal(110107, adCode.Id);

        }

        [Fact]
        public void Can_Match_ZipCode()
        {
            var info = ChinaAdCode.GetByZipCode("519000");
            Assert.True(info.Any());
        }

        [Fact]
        public void Can_Match_CityCode()
        {
            var info = ChinaAdCode.GetByCityCode("020");
            Assert.True(info.Any());
        }

        [Fact]
        public void Can_Match_SourceFile()
        {
            var t = typeof(ChinaAdCode);

            using (var stream = t.Assembly.GetManifestResourceStream($"{t.Namespace}.{t.Name}.txt"))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    string line;
                    while (!string.IsNullOrEmpty((line = sr.ReadLine())))
                    {
                        var values = line.Split('|');
                        var id = int.Parse(values[0]);
                        var parentid = int.Parse(values[1]);
                        var level = int.Parse(values[2]);
                        var province = values[3];
                        var city = values[4];
                        var district = values[5];
                        var lng = double.Parse(values[6]);
                        var lat = double.Parse(values[7]);
                        var cityCode = values[8];
                        var zipCode = values[9];

                        var info = ChinaAdCode.Get(id);

                        Assert.Equal(id, info.Id);
                        Assert.Equal(level, info.Level);
                        Assert.Equal(province, info.Province);
                        Assert.Equal(city, info.City);
                        Assert.Equal(district, info.District);
                        Assert.Equal(lng, info.Lng);
                        Assert.Equal(lat, info.Lat);
                        Assert.Equal(cityCode, info.CityCode);
                        Assert.Equal(zipCode, info.ZipCode);
                        
                    }
                }
            }
        }
    }
}
