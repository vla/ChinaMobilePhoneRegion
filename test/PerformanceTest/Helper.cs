using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using MobilePhoneRegion;

namespace PerformanceTest
{
    class Helper
    {
        public static void Time(string name, Action action, int iteration = 1)
        {
            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(name);

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            int[] gcCounts = new int[GC.MaxGeneration + 1];
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                gcCounts[i] = GC.CollectionCount(i);
            }

            Stopwatch watch = new Stopwatch();
            watch.Start();
            ulong cycleCount = GetCycleCount();
            for (int i = 0; i < iteration; i++) action();
            ulong cpuCycles = GetCycleCount() - cycleCount;
            watch.Stop();

            Console.ForegroundColor = currentForeColor;
            Console.WriteLine("\tIterations:\t" + iteration);
            Console.WriteLine("\tTime Elapsed:\t" + watch.Elapsed.TotalMilliseconds.ToString("N0") + "ms");
            Console.WriteLine("\tPer Second:\t" + (iteration / watch.Elapsed.TotalSeconds).ToString("N0"));
            Console.WriteLine("\tCPU Cycles:\t" + cpuCycles.ToString("N0"));
            Console.WriteLine("\tMemory:\t" + FormatBytesToString(Process.GetCurrentProcess().WorkingSet64));

            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                int count = GC.CollectionCount(i) - gcCounts[i];
                Console.WriteLine("\tGen " + i + ": \t\t" + count);
            }

            Console.WriteLine();
        }

        private static ulong GetCycleCount()
        {
            ulong cycleCount = 0;
            QueryThreadCycleTime(GetCurrentThread(), ref cycleCount);
            return cycleCount;
        }

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool QueryThreadCycleTime(IntPtr threadHandle, ref ulong cycleTime);

        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetCurrentThread();

        public static void PrintMem()
        {
            Console.WriteLine("Memory:" + Helper.FormatBytesToString(Process.GetCurrentProcess().WorkingSet64));
        }

        public static string FormatBytesToString(double bytes)
        {
            const ulong
              KB = 1UL << 10,
              MB = 1UL << 20,
              GB = 1UL << 30,
              TB = 1UL << 40,
              PB = 1UL << 50,
              EB = 1UL << 60;

            if (bytes > EB)
                return string.Format("{0}EB", (bytes / EB).ToString("F2"));
            if (bytes > PB)
                return string.Format("{0}PB", (bytes / PB).ToString("F2"));
            if (bytes > TB)
                return string.Format("{0}TB", (bytes / TB).ToString("F2"));
            if (bytes > GB)
                return string.Format("{0}GB", (bytes / GB).ToString("F2"));
            if (bytes > MB)
                return string.Format("{0}MB", (bytes / MB).ToString("F2"));
            if (bytes > KB)
                return string.Format("{0}KB", (bytes / KB).ToString("F2"));
            return bytes + "Byte";
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
