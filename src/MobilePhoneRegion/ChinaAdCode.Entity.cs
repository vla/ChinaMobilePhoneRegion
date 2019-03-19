namespace MobilePhoneRegion
{
    /// <summary>
    /// 行政区域信息
    /// </summary>
    public partial class ChinaAdCode
    {
        /// <summary>
        /// 行政区域编码
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 父级编码
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 地区级别，如 1省份、2城市、3区县
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 完整名称
        /// </summary>
        public string Name {
            get {

                return $"{Province} {City} {District}".Trim();
            }
        }

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; } = string.Empty;

        /// <summary>
        ///  城市
        /// </summary>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// 区县
        /// </summary>
        public string District { get; set; } = string.Empty;

        /// <summary>
        /// 地球坐标系经度
        /// </summary>
        public double Lng { get; set; }

        /// <summary>
        /// 地球坐标系纬度
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// 电话区号
        /// </summary>
        public string CityCode { get; set; } = string.Empty;

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string ZipCode { get; set; } = string.Empty;


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"省份:{Province},城市:{City},区县:{District},固话区号:{CityCode},邮政编码:{ZipCode}";
        }
    }
}