using System.ComponentModel;

namespace ValidEinvoiceSuccess.Enums
{
    public enum FilterInvoiceType
    {
        /// <summary>
        /// 開立
        /// </summary>
        [Description("1")]
        ISSUE,

        /// <summary>
        /// 作廢
        /// </summary>
        [Description("D")]
        DROP
    }
}