using System;
using System.Net.Mail;

namespace MailerAPI
{
    /// <summary>
    /// Mailer
    /// </summary>
    public class Mailer
    {
        private MailInfo info { get; set; }

        /// <summary>
        /// init MailInfo
        /// </summary>
        /// <param name="info"></param>
        public Mailer(MailInfo info)
        {
            this.info = info;
        }

        /// <summary>
        /// SendMail
        /// </summary>
        /// <returns></returns>
        public Boolean SendMail()
        {
            Boolean isSuccess = false;
            string address = String.Format("{0}<{1}>", "林內工業", "System@Rinnai.com.tw");
            MailMessage mail = new MailMessage();

            try
            {
                if (info.To.Count == 0)
                    return false;
                foreach (var to in info.To)
                    mail.To.Add(to);
                //寄件者
                mail.From = new MailAddress(address, "系統提醒");
                //以Html方式發送
                mail.IsBodyHtml = true;
                mail.Subject = info.Subject;
                mail.Body = info.Body.ToString();
                if (info.CC != null) { info.CC.ForEach(x => mail.CC.Add(x)); }

                using (SmtpClient smtp = new SmtpClient())
                {
                    //發送郵件
                    smtp.Send(mail);
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                //todo log mail content
                throw new Exception(ex.Message + ", " + ex.StackTrace);
            }
            finally
            {
                mail.Dispose();
                mail = null;
            }
            return isSuccess;
        }
    }
}