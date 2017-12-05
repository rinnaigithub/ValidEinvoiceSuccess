using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidEinvoiceSuccess.Enums
{
    public enum WorkflowTypeEnum
    {
        /// <summary>
        /// 正式
        /// </summary>
        [Description("正式")]
        RELEASE = 1,
        /// <summary>
        ///開發
        /// </summary>
        [Description("開發")]
        DEBUG = 2,
    }
}
