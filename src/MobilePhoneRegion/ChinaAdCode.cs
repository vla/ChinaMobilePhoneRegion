using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MobilePhoneRegion
{
    /// <summary>
    /// 中国行政区域信息
    /// </summary>
    public partial class ChinaAdCode
    {
        private static Dictionary<int, ChinaAdCode> Dict_AreaCode;
        private static Dictionary<string, IList<ChinaAdCode>> Dict_CityCode;
        private static Dictionary<string, IList<ChinaAdCode>> Dict_ZipCode;
        private static Dictionary<string, IList<ChinaAdCode>> Dict_Province;

        static ChinaAdCode()
        {
            RrepareResource();
        }

        /// <summary>
        /// 根据行政编码获取信息
        /// </summary>
        /// <param name="areacode">行政编码</param>
        /// <returns><see cref="ChinaAdCode"/></returns>
        public static ChinaAdCode Get(int areacode)
        {
            Dict_AreaCode.TryGetValue(areacode, out ChinaAdCode code);
            return code;
        }

        /// <summary>
        /// 根据邮政编码获取信息
        /// </summary>
        /// <param name="zipcode">邮政编码</param>
        /// <returns><see cref="ChinaAdCode"/></returns>
        public static IEnumerable<ChinaAdCode> GetByZipCode(string zipcode)
        {
            if (Dict_ZipCode.TryGetValue(zipcode, out var list))
            {
                return list;
            }

            return Enumerable.Empty<ChinaAdCode>();
        }

        /// <summary>
        /// 根据固话区号获取信息
        /// </summary>
        /// <param name="citycode">固话区号</param>
        /// <returns><see cref="ChinaAdCode"/></returns>
        public static IEnumerable<ChinaAdCode> GetByCityCode(string citycode)
        {
            if (Dict_CityCode.TryGetValue(citycode, out var list))
            {
                return list;
            }
            return Enumerable.Empty<ChinaAdCode>();
        }

        /// <summary>
        /// 查询中国行政区域信息
        /// </summary>
        /// <param name="province">完整的省份名称，如：广东省</param>
        /// <returns></returns>
        public static IEnumerable<ChinaAdCode> Search(string province)
        {
            if (Dict_Province.TryGetValue(province, out var list))
            {
                return list;
            }

            return Enumerable.Empty<ChinaAdCode>();
        }

        /// <summary>
        /// 查询中国行政区域信息
        /// </summary>
        /// <param name="province">省</param>
        /// <param name="city">城市</param>
        /// <returns></returns>
        public static IEnumerable<ChinaAdCode> Search(string province, string city)
        {
            var match = false;

            var list = new List<ChinaAdCode>();

            foreach (var entity in Search(province))
            {
                if (entity.City.StartsWith(city))
                {
                    match = true;
                    list.Add(entity);
                }
                else
                {
                    //因为数据是连续性的，当遇到成功的后续匹配失败则退出
                    if (match)
                        break;
                }
            }

            return list;
        }

        /// <summary>
        /// 查询中国行政区域信息
        /// </summary>
        /// <param name="province">省</param>
        /// <param name="city">城市</param>
        /// <param name="district">区县</param>
        /// <returns></returns>
        public static ChinaAdCode Search(string province, string city, string district)
        {
            foreach (var entity in Search(province, city))
            {
                if (entity.District.StartsWith(district))
                {
                    return entity;
                }
            }

            return null;
        }

        private static void RrepareResource()
        {
            var lstAdCode = new List<ChinaAdCode>();

            var t = typeof(ChinaAdCode);
            var name = $"{t.Namespace}.{t.Name}.txt";

            using (var stream = t.Assembly.GetManifestResourceStream(name))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    string line;
                    while (!string.IsNullOrEmpty((line = sr.ReadLine())))
                    {
                        var values = line.Split('|');

                        if (values.Length != 13)
                            continue;

                        ChinaAdCode info = new ChinaAdCode
                        {
                            Id = int.Parse(values[0]),
                            ParentId = int.Parse(values[1]),
                            Level = int.Parse(values[2]),
                            Name = values[3],
                            ShortName = values[4],
                            Province = values[5],
                            City = values[6],
                            District = values[7],
                            Lng = double.Parse(values[8]),
                            Lat = double.Parse(values[9]),
                            CityCode = values[10],
                            ZipCode = values[11],
                            PinYin = values[12]
                        };

                        lstAdCode.Add(info);
                    }
                }
            }

            //建立查询索引
            Dict_AreaCode = new Dictionary<int, ChinaAdCode>(lstAdCode.Count);
            Dict_CityCode = new Dictionary<string, IList<ChinaAdCode>>();
            Dict_ZipCode = new Dictionary<string, IList<ChinaAdCode>>();
            Dict_Province = new Dictionary<string, IList<ChinaAdCode>>();

            foreach (var entity in lstAdCode)
            {
                Dict_AreaCode[entity.Id] = entity;

                if (!string.IsNullOrWhiteSpace(entity.Province))
                {
                    if (Dict_Province.TryGetValue(entity.Province, out var list))
                    {
                        list.Add(entity);
                    }
                    else
                    {
                        Dict_Province.Add(entity.Province, new[] { entity }.ToList());
                    }
                }

                if (!string.IsNullOrWhiteSpace(entity.CityCode))
                {
                    if (Dict_CityCode.TryGetValue(entity.CityCode, out var list))
                    {
                        list.Add(entity);
                    }
                    else
                    {
                        Dict_CityCode.Add(entity.CityCode, new[] { entity }.ToList());
                    }
                }

                if (!string.IsNullOrWhiteSpace(entity.ZipCode))
                {
                    if (Dict_ZipCode.TryGetValue(entity.ZipCode, out var list))
                    {
                        list.Add(entity);
                    }
                    else
                    {
                        Dict_ZipCode.Add(entity.ZipCode, new[] { entity }.ToList());
                    }
                }
            }
        }
    }
}