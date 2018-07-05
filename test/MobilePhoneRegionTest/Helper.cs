using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using MobilePhoneRegion;

namespace MobilePhoneRegionTest
{
    internal class Helper
    {
        internal static IList<MobilePhone> GetPhoneList(string filename)
        {
            var list = new List<MobilePhone>();

            foreach (var line in File.ReadLines(filename))
            {
                var seg = line.Split('|');

                MobilePhone info = new MobilePhone();
                info.Phone = int.Parse(seg[0]);
                var province = seg[1];
                var city = seg[2];

                if (province == city)
                {
                    info.Area = city;
                }
                else
                {
                    info.Area = province + city;
                }

                info.Isp = seg[3];
                info.CityCode = seg[4];
                info.ZipCode = seg[5];
                info.AdCode = int.Parse(seg[6]);

                list.Add(info);
            }

            return list;
        }
    }
}
