namespace MobilePhoneRegion
{
    /// <summary>
    /// 查询结果
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// 查询成功或者失败
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 行政区域编码
        /// </summary>
        public int AdCode { get; set; }
        
        /// <summary>
        /// 运营商信息
        /// </summary>
        public string Isp { get; set; }
    }
}