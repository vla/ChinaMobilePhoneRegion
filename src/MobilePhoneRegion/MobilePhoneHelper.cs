using System;
using System.Collections.Generic;
using System.Text;
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
    }
}