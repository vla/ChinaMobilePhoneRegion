namespace MobilePhoneRegion.Internal.V2
{
    /// <summary>
    /// 归属地存储
    /// </summary>
    internal struct Store
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Store"/> struct.
        /// </summary>
        /// <param name="phone">手机号码七位段</param>
        /// <param name="adcodeIdx">行政区域索引位置</param>
        /// <param name="ispOffset">运营商流位置</param>
        public Store(int phone, ushort adcodeIdx, uint ispOffset)
        {
            AdCodeIndex = adcodeIdx;
            IspOffset = ispOffset;
            Before = (ushort)(phone * 0.01);
            After = phone - Before * 100;
        }

        /// <summary>
        /// 号码前五位 如：1370233 -> 13702
        /// </summary>
        public ushort Before { get; set; }

        /// <summary>
        /// 最末存储手机号码七位段的最后两位，前面储存连续的递增数值
        /// 如：1370233 -> 33,1370234 -> 133,1370235 -> 233
        /// 表示1370233~1370235的最后两位是从33递增2位表示233
        /// </summary>
        public int After { get; set; }

        /// <summary>
        /// 行政区域索引位置
        /// </summary>
        public ushort AdCodeIndex { get; set; }

        /// <summary>
        /// 运营商流位置
        /// </summary>
        public uint IspOffset { get; set; }

        /// <summary>
        /// 获取手机七位段原始号码
        /// </summary>
        /// <returns></returns>
        public int GetPhone()
        {
            // 1370233 + 1 = 1370234
            return GetBegin() + GetSkip();
        }

        /// <summary>
        /// 获取手机七位段的起始号码
        /// </summary>
        /// <returns></returns>
        public int GetBegin()
        {
            // 137023 * 100 + 33 = 1370233
            return Before * 100 + (After - GetSkip() * 100);
        }

        /// <summary>
        /// 获取递增数
        /// </summary>
        /// <returns></returns>
        public int GetSkip()
        {
            return (int)(After * 0.01);
        }

        /// <summary>
        /// 设置递增数
        /// </summary>
        /// <param name="skip"></param>
        public void SetSkip(int skip)
        {
            //skip = 2
            //After = 133
            //2 * 100 + 33 = 233;
            After = skip * 100 + GetLastTwoNumber(After);
        }

        /// <summary>
        /// 获取整数最后2位
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private int GetLastTwoNumber(int number)
        {
            return (number - (int)(number * 0.01) * 100);
        }

        public override string ToString()
        {
            return $"手机号码前7位:{GetPhone()},前半段:{Before},后半段:{After},起始位置:{GetBegin()},递增数:{GetSkip()}";
        }
    }
}