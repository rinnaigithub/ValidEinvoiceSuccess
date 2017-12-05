using System;
namespace ValidEinvoiceSuccess.Models
{
    public class ContactModel
    {

        /// <summary>
        /// 訂單號碼
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 發票號碼
        /// </summary>
        public string InvoiceNumber { get; set; }
        /// <summary>
        /// 訂單日期
        /// </summary>
        public DateTime? StartDate { get; set; }

    }
}