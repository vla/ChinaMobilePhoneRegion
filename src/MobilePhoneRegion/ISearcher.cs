namespace MobilePhoneRegion
{
    /// <summary>
    /// 手机号码归属查询器
    /// </summary>
    public interface ISearcher
    {
        /// <summary>
        /// 版本
        /// </summary>
        byte Version { get; }

        /// <summary>
        /// 压缩后的总数
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 根据手机前7位进行搜索
        /// </summary>
        /// <param name="number">手机前7位</param>
        /// <returns></returns>
        SearchResult Search(int number);
    }
}