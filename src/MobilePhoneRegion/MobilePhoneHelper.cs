using System.Text.RegularExpressions;

namespace MobilePhoneRegion
{
    /// <summary>
    /// MobilePhoneHelper
    /// </summary>
    public static class MobilePhoneHelper
    {
        //手机号码
        //130 131 132 133 134 135 136 137 138 139 
        //145 146 147 148 149 
        //150 151 152 153 155 156 157 158 159 
        //166 
        //171 172 173 174 175 176 177 178 
        //180 181 182 183 184 185 186 187 188 189 
        //199 198
        private static readonly Regex MOBILE_Expression = new Regex("^[1][3-9][0-9]{9}$", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        /// <summary>
        /// 是否手机号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMobilePhone(string input)
        {
            return MOBILE_Expression.IsMatch(input);
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