using System.Text.RegularExpressions;

namespace MobilePhoneRegion
{
    /// <summary>
    /// MobilePhoneHelper
    /// </summary>
    public static class MobilePhoneHelper
    {
        //手机号码
        //移动：134[0-8],135,136,137,138,139,150,151,157,158,159,182,187,188
        //联通：130,131,132,152,155,156,185,186
        //电信：133,1349,153,180,189
        private static readonly Regex MOBILE_Expression = new Regex("^1(3[0-9]|5[0-35-9]|8[025-9])\\d{8}$", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        private static readonly Regex CM_Expression = new Regex("^1(34[0-8]|(3[5-9]|5[017-9]|8[278])\\d)\\d{7}$", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static readonly Regex CU_Expression = new Regex("^1(3[0-2]|5[256]|8[56])\\d{8}$", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static readonly Regex CT_Expression = new Regex("^1((33|53|8[09])[0-9]|349)\\d{7}$", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        /// <summary>
        /// 是否手机号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMobilePhone(string input)
        {
            return MOBILE_Expression.IsMatch(input)
                 || CM_Expression.IsMatch(input)
                 || CU_Expression.IsMatch(input)
                 || CT_Expression.IsMatch(input);
        }

        /// <summary>
        /// 手机归属地查询
        /// </summary>
        /// <param name="searcher"><see cref="ISearcher"/></param>
        /// <param name="mobilePhone">中国手机号码</param>
        /// <param name="info">手机归属地信息</param>
        /// <returns>是否匹配成功</returns>
        public static bool TryGet(this ISearcher searcher, string mobilePhone, out MobilePhone info)
        {
            info = null;

            if (string.IsNullOrWhiteSpace(mobilePhone))
                return false;

            if (mobilePhone.Length < 7)
                return false;

            if (IsMobilePhone(mobilePhone))
            {
                if (mobilePhone.Length > 7)
                {
                    mobilePhone = mobilePhone.Substring(0, 7);
                }

                if (int.TryParse(mobilePhone, out int number))
                {
                    var result = searcher.Search(number);

                    if (result.Success)
                    {
                        var adCode = ChinaAdCode.Get(result.AdCode);

                        info = new MobilePhone
                        {
                            Phone = number,
                            AdCode = result.AdCode,
                            Isp = result.Isp,
                            CityCode = adCode.CityCode,
                            ZipCode = adCode.ZipCode,
                            Area = adCode.Province + adCode.City
                        };

                        return true;
                    }
                }
            }
            return false;
        }
    }
}