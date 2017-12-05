using System;
using System.Collections.Generic;

namespace ValidEinvoiceSuccess.Models
{
    public static class ValidMailBufferModel
    {
        /// <summary>
        /// B2B未傳到Einvoice資料庫之訂單
        /// </summary>
        public static List<ValidInfoBuffer> B2BLostDataBuffer { get { return m_b2bLostDataBuffer; } set { m_b2bLostDataBuffer = value; } }

        private static List<ValidInfoBuffer> m_b2bLostDataBuffer = new List<ValidInfoBuffer>();

        /// <summary>
        /// B2B未傳到Einvoice資料庫之作廢單
        /// </summary>
        public static List<ValidInfoBuffer> B2CLostDataBuffer { get { return m_b2cLostDataBuffer; } set { m_b2cLostDataBuffer = value; } }

        private static List<ValidInfoBuffer> m_b2cLostDataBuffer = new List<ValidInfoBuffer>();

        /// <summary>
        /// B2C未傳到Einvoice資料庫之訂單
        /// </summary>
        public static List<ValidInfoBuffer> B2BLostDropDataBuffer { get { return m_b2bLostDropDataBuffer; } set { m_b2bLostDropDataBuffer = value; } }

        private static List<ValidInfoBuffer> m_b2bLostDropDataBuffer = new List<ValidInfoBuffer>();

        /// <summary>
        /// B2C未傳到Einvoice資料庫之作廢單
        /// </summary>
        public static List<ValidInfoBuffer> B2CLostDropDataBuffer { get { return m_b2cLostDropDataBuffer; } set { m_b2cLostDropDataBuffer = value; } }

        private static List<ValidInfoBuffer> m_b2cLostDropDataBuffer = new List<ValidInfoBuffer>();
    }


    public class ValidInfoBuffer
    {
        private string m_orderNo = string.Empty;
        private string m_invoiceNo = string.Empty;
        private DateTime? m_invoiceDate;

        public string OrderNo { get { return m_orderNo; } set { m_orderNo = value; } }
        public string InvoiceNo { get { return m_invoiceNo; } set { m_invoiceNo = value; } }
        public DateTime? StartDatetime { get { return m_invoiceDate; } set { m_invoiceDate = value; } }
    }
}