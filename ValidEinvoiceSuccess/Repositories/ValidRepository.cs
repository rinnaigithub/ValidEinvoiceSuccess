using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using ValidEinvoiceSuccess.Entities;
using ValidEinvoiceSuccess.Enums;
using ValidEinvoiceSuccess.Models;

#region Remark

/*****************************************************************
* Date: 2017-11-09
* Author: juncheng.liu
* Purpose: 驗證電子發票是否有成功寫入Einvoice資料庫
* Remark:
* Modify:
*****************************************************************/

#endregion Remark

namespace ValidEinvoiceSuccess.Repositories
{
    public class ValidRepository : IDisposable
    {
        private ERPDB m_db = new ERPDB();
        private ERPDB DB { get { return m_db; } set { m_db = value; } }

        /// <summary>
        /// 比對發票並將不符合的資料存進靜態Buffer
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public void ComparerEinvoiceNumber(DateTime startDate, DateTime endDate)
        {
            eInvoiceSLNDBEntities einvoiceDB = new eInvoiceSLNDBEntities();

            #region 發票開立

            List<ContactModel> erpDataB2B = GetAllERPEinvoiceDataB2B(startDate, endDate, FilterInvoiceType.ISSUE);
            foreach (var b2b in erpDataB2B)
            {
                var a0401h = einvoiceDB.A0401H.Where(o => o.MInvoiceNumber == b2b.InvoiceNumber).FirstOrDefault();
                if (a0401h == null)
                {
                    ValidMailBufferModel.B2BLostDataBuffer.Add(new ValidInfoBuffer()
                    {
                        InvoiceNo = b2b.InvoiceNumber,
                        OrderNo = b2b.OrderNumber,
                        StartDatetime = b2b.StartDate
                    });
                }
            }

            List<ContactModel> erpDataB2C = GetAllERPEinvoiceDataB2C(startDate, endDate, FilterInvoiceType.ISSUE);
            foreach (var b2c in erpDataB2C)
            {
                var c0401h = einvoiceDB.C0401H.Where(o => o.MInvoiceNumber == b2c.InvoiceNumber).FirstOrDefault();
                if (c0401h == null)
                {
                    ValidMailBufferModel.B2CLostDataBuffer.Add(new ValidInfoBuffer()
                    {
                        InvoiceNo = b2c.InvoiceNumber,
                        OrderNo = b2c.OrderNumber,
                        StartDatetime = b2c.StartDate
                    });
                }
            }

            #endregion 發票開立

            #region 發票作廢

            List<ContactModel> erpDropDataB2B = GetAllERPEinvoiceDataB2B(startDate, endDate, FilterInvoiceType.DROP);
            foreach (var b2c in erpDropDataB2B)
            {
                var a0501h = einvoiceDB.A0501.Where(o => o.CancelInvoiceNumber == b2c.InvoiceNumber).FirstOrDefault();
                if (a0501h == null)
                {
                    ValidMailBufferModel.B2BLostDropDataBuffer.Add(new ValidInfoBuffer()
                    {
                        InvoiceNo = b2c.InvoiceNumber,
                        OrderNo = b2c.OrderNumber,
                        StartDatetime = b2c.StartDate
                    });
                }
            }

            List<ContactModel> erpDropDataB2C = GetAllERPEinvoiceDataB2C(startDate, endDate, FilterInvoiceType.DROP);
            foreach (var b2c in erpDropDataB2C)
            {
                var c0501h = einvoiceDB.C0501.Where(o => o.CancelInvoiceNumber == b2c.InvoiceNumber).FirstOrDefault();
                if (c0501h == null)
                {
                    ValidMailBufferModel.B2CLostDropDataBuffer.Add(new ValidInfoBuffer()
                    {
                        InvoiceNo = b2c.InvoiceNumber,
                        OrderNo = b2c.OrderNumber,
                        StartDatetime = b2c.StartDate
                    });
                }
            }

            #endregion 發票作廢
        }

        /// <summary>
        /// 取得所有開立訂單與阿票號碼
        /// </summary>
        /// <param name="filterID">[31=>B2B]，[32=>B2C]</param>
        /// <returns></returns>
        private List<ContactModel> GetAllERPEinvoiceDataB2B(DateTime startDate, DateTime endDate, FilterInvoiceType filterType)
        {
            string type = filterType.GetDescription();
            List<ContactModel> phoneOrderNumbers = new List<ContactModel>();
            try
            {
                phoneOrderNumbers =
                    this.DB.Rinnai_GUI_Transaction_Header
                     .Where(o =>
                         o.GUI_Transaction_Type == "SAL" &&
                        (SqlFunctions.Replicate("0", 5 - o.Period_Of_Declaration.Length) +
                        o.Period_Of_Declaration).CompareTo("10611") > 0 &&
                         o.Order_Date > new DateTime(2011, 01, 01) &&
                         o.GUI_Foramt_Code == "31" &&
                         o.VAT_Tax_Type == type &&
                         o.Return_Seq__No_ == 0 &&
                         o.Create_Date >= startDate &&
                         o.Create_Date <= endDate)
                         .Select(s => new ContactModel()
                             {
                                 InvoiceNumber = s.VAT_Transaction_Number,
                                 StartDate = s.Order_Date,
                                 OrderNumber = s.Order_No_
                             }
                         )
                     .ToList();

                #region 無發票直接回傳

                if (phoneOrderNumbers.Count == 0)
                {
                    return phoneOrderNumbers;
                }

                #endregion 無發票直接回傳
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return phoneOrderNumbers;
        }

        /// <summary>
        /// 取得所有開立訂單與阿票號碼
        /// </summary>
        /// <param name="filterID">[31=>B2B]，[32=>B2C]</param>
        /// <returns></returns>
        private List<ContactModel> GetAllERPEinvoiceDataB2C(DateTime startDate, DateTime endDate, FilterInvoiceType filterType)
        {
            string type = filterType.GetDescription();
            List<ContactModel> phoneOrderNumbers = new List<ContactModel>();
            try
            {
                List<string> invoiceNumbers =
                    this.DB.Rinnai_GUI_Transaction_Header
                     .Where(o =>
                         o.GUI_Transaction_Type == "SAL" &&
                        (SqlFunctions.Replicate("0", 5 - o.Period_Of_Declaration.Length) +
                        o.Period_Of_Declaration).CompareTo("10611") > 0 &&
                         o.Order_Date > new DateTime(2011, 01, 01) &&
                         o.GUI_Foramt_Code == "32" &&
                         o.VAT_Tax_Type == type &&
                         o.Return_Seq__No_ == 0 &&
                         o.Create_Date >= startDate &&
                         o.Create_Date <= endDate)
                         .Select(s => s.Invoice_No_)
                     .ToList();

                #region 無發票直接回傳

                if (invoiceNumbers.Count == 0)
                {
                    return phoneOrderNumbers;
                }

                #endregion 無發票直接回傳

                List<string> serviceOrderNumbers =
                    this.DB.Rinnai_Service_Ledger_Entry
                    .Where(o => invoiceNumbers.Contains(o.Document_No_) && o.Document_Type == 2)
                    .Select(s => s.Service_Order_No_)
                    .Distinct()
                    .ToList();

                phoneOrderNumbers =
                           this.DB.Rinnai_Posted_Service_Header
                    .Where(o => serviceOrderNumbers.Contains(o.No_))
                    .Select(s => new ContactModel()
                    {
                        OrderNumber = s.No_,
                        StartDate = s.Starting_Date
                    })
                    .ToList();

                //取發票號碼
                foreach (var obj in phoneOrderNumbers)
                {
                    var inv = this.DB.Rinnai_Sales_Invoice_Line.Where(o => o.Shipment_No_ == obj.OrderNumber).FirstOrDefault();
                    if (inv != null)
                        obj.InvoiceNumber = inv.VAT_Transaction_Number;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return phoneOrderNumbers;
        }

        /// <summary>
        /// 傳送Email
        /// </summary>
        /// <param name="totalProcessTime"></param>
        public void EmailSend(string dateGroup, string date)
        {
            var body = new StringBuilder();
            body.AppendLine(
            @"<p><span style=""color: #808080;""><strong>【" + date + " 電子發票資料比對清單】 </strong></span></p>");
            body.AppendLine(@"
                    <p><strong><span style=""color: #99cc00;"">核對日期：</span></strong>" + dateGroup + "</p>");

            #region B2C 開立

            body.AppendLine(@"
                    <p><strong><span style=""color: #99cc00;"">Einvoice[B2C-發票開立]未有資料清單如下</span></strong>，共 " + ValidMailBufferModel.B2CLostDataBuffer.Count + " 筆</p>");

            if (ValidMailBufferModel.B2CLostDataBuffer.Count > 0)
            {
                body.AppendLine(@"
                    <table style=""border:1px #ccc solid;"" cellpadding=""5"" border='1'>
                        <thead>
                            <tr>
                                <th style=""width: 162px;"">訂單號碼</th>
                                <th style=""width: 162px;"">發票號碼</th>
                                <th style=""width: 162px;"">訂單StartDate</th>
                            </tr>
                        </thead>
                        <tbody>");
                foreach (var item in ValidMailBufferModel.B2CLostDataBuffer)
                {
                    body.AppendLine(@"
                            <tr>
                                <td style=""width: 162px;"">" + item.OrderNo + @"</td>
                                <td style=""width: 162px;"">" + item.InvoiceNo + @"</td>
                                <td style=""width: 162px;"">" + item.StartDatetime.Value.ToString("yyyy\\/MM\\/dd") + @"</td>
                            </tr>");
                }

                body.AppendLine(@"
                        </tbody>
                    </table>");
            }

            #endregion B2C 開立

            #region B2B 開立

            body.AppendLine(@"
                    <p><strong><span style=""color: #99cc00;"">Einvoice[B2B-發票開立]未有資料清單如下</span></strong>，共 " + ValidMailBufferModel.B2BLostDataBuffer.Count + " 筆</p>");

            if (ValidMailBufferModel.B2BLostDataBuffer.Count > 0)
            {
                body.AppendLine(@"
                    <table style=""border:1px #ccc solid;"" cellpadding=""5"" border='1'>
                        <thead>
                            <tr>
                                <th style=""width: 162px;"">訂單號碼</th>
                                <th style=""width: 162px;"">發票號碼</th>
                                <th style=""width: 162px;"">訂單StartDate</th>
                            </tr>
                        </thead>
                        <tbody>");
                foreach (var item in ValidMailBufferModel.B2BLostDataBuffer)
                {
                    body.AppendLine(@"
                            <tr>
                      <td style=""width: 162px;"">" + item.OrderNo + @"</td>
                                <td style=""width: 162px;"">" + item.InvoiceNo + @"</td>
                                <td style=""width: 162px;"">" + item.StartDatetime.Value.ToString("yyyy\\/MM\\/dd") + @"</td>
                            </tr>");
                }
                body.AppendLine(@"
                        </tbody>
                    </table>");
            }

            #endregion B2B 開立

            #region B2C 作廢

            body.AppendLine(@"
                    <p><strong><span style=""color: #99cc00;"">Einvoice[B2C-發票作廢]未有資料清單如下</span></strong>，共 " + ValidMailBufferModel.B2CLostDropDataBuffer.Count + " 筆</p>");

            if (ValidMailBufferModel.B2CLostDropDataBuffer.Count > 0)
            {
                body.AppendLine(@"
                    <table style=""border:1px #ccc solid;"" cellpadding=""5"" border='1'>
                        <thead>
                            <tr>
                                <th style=""width: 162px;"">訂單號碼</th>
                                <th style=""width: 162px;"">發票號碼</th>
                                <th style=""width: 162px;"">訂單StartDate</th>
                            </tr>
                        </thead>
                        <tbody>");
                foreach (var item in ValidMailBufferModel.B2CLostDropDataBuffer)
                {
                    body.AppendLine(@"
                            <tr>
                      <td style=""width: 162px;"">" + item.OrderNo + @"</td>
                                <td style=""width: 162px;"">" + item.InvoiceNo + @"</td>
                                <td style=""width: 162px;"">" + item.StartDatetime.Value.ToString("yyyy\\/MM\\/dd") + @"</td>
                            </tr>");
                }
                body.AppendLine(@"
                        </tbody>
                    </table>");
            }

            #endregion B2C 作廢

            #region B2B 作廢

            body.AppendLine(@"
                    <p><strong><span style=""color: #99cc00;"">Einvoice[B2B-發票作廢]未有資料清單如下</span></strong>，共 " + ValidMailBufferModel.B2CLostDropDataBuffer.Count + " 筆</p>");

            if (ValidMailBufferModel.B2BLostDropDataBuffer.Count > 0)
            {
                body.AppendLine(@"
                    <table style=""border:1px #ccc solid;"" cellpadding=""5"" border='1'>
                        <thead>
                            <tr>
                                <th style=""width: 162px;"">訂單號碼</th>
                                <th style=""width: 162px;"">發票號碼</th>
                                <th style=""width: 162px;"">訂單StartDate</th>
                            </tr>
                        </thead>
                        <tbody>");
                foreach (var item in ValidMailBufferModel.B2BLostDropDataBuffer)
                {
                    body.AppendLine(@"
                            <tr>
                      <td style=""width: 162px;"">" + item.OrderNo + @"</td>
                                <td style=""width: 162px;"">" + item.InvoiceNo + @"</td>
                                <td style=""width: 162px;"">" + item.StartDatetime.Value.ToString("yyyy\\/MM\\/dd") + @"</td>
                            </tr>");
                }
                body.AppendLine(@"
                        </tbody>
                    </table>");
            }

            #endregion B2B 作廢

            //email通知
            MailerAPI.MailInfo mailInfo = new MailerAPI.MailInfo()
            {
                Subject = string.Format("[電子發票資料驗證通知系統]{0}通知明細", date),
                CC = PublicStaticMethod.AdminEmail,
                Body = body,
                To = PublicStaticMethod.CurrentWorkflowMode == WorkflowTypeEnum.RELEASE ? PublicStaticMethod.AdminEmailGroup : PublicStaticMethod.AdminEmail
            };
            MailerAPI.Mailer mailer = new MailerAPI.Mailer(mailInfo);
            mailer.SendMail();
        }

        public void Dispose()
        {
            if (this.DB.Database.Connection.State == System.Data.ConnectionState.Open)
                this.DB.Database.Connection.Close();
            this.DB.Dispose();
            this.DB = null;
        }
    }
}