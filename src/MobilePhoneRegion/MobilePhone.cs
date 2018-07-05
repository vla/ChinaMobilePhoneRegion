namespace MobilePhoneRegion
{
    /// <summary>
    /// 手机号码归属地信息
    /// </summary>
    public class MobilePhone
    {
        /// <summary>
        /// 手机号码前7位
        /// </summary>
        public int Phone { get; set; }

        /// <summary>
        /// 地区
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 运营商
        /// </summary>
        public string Isp { get; set; }

        /// <summary>
        /// 固话区号
        /// </summary>
        public string CityCode { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// 行政区域编码
        /// </summary>
        public int AdCode { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"手机号码前7位:{Phone},地区:{Area},运营商:{Isp},行政区域编码:{AdCode},固话区号:{CityCode},邮政编码:{ZipCode}";
        }
    }
}