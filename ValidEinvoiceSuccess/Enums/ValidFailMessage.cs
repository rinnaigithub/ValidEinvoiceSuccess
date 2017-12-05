using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Enums
{
    public enum ValidFailMessage
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("Success")]
        Success = 1,
        /// <summary>
        /// 手機號碼格式不正確
        /// </summary>
        [Description("手機號碼格式不正確")]
        PhomeForamtFail = 2,
        /// <summary>
        /// 手機或訂單號碼為空
        /// </summary>
        [Description("手機或訂單號碼為空")]
        DataIsEmpty = 3,
    }
}
