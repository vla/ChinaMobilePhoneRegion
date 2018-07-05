using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MobilePhoneRegion;

namespace Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "phones.txt");
            var dest = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MobilePhoneRegion.dat");

            using (var fs = File.Create(dest))
            {
                MobilePhoneFactory.Generate(MobilePhoneRegion.Version.V2, GetPhoneList(filename).ToArray(), fs);
            }

            Console.WriteLine("done.");
        }

        static IList<MobilePhone> GetPhoneList(string filename)
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
