using System;
using ValidEinvoiceSuccess.Repositories;

namespace ValidEinvoiceSuccess
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ValidRepository repo = new ValidRepository();
            var yestoday = DateTime.UtcNow.AddHours(8).AddDays(-1);
            //var yestoday = DateTime.UtcNow.AddHours(8);
            var filterBeginDate = new DateTime(
                yestoday.Year, yestoday.Month, yestoday.Day);
            //var filterBeginDate = new DateTime(2017, 11, 01, 00, 00, 00);
            var filterEndDate = new DateTime(
                 yestoday.Year, yestoday.Month, yestoday.Day, 23, 59, 59);
            repo.ComparerEinvoiceNumber(filterBeginDate, filterEndDate);
            string comparerDate = string.Format("核對期間：{0}~{1}", filterBeginDate, filterEndDate);
            repo.EmailSend(comparerDate, yestoday.ToString("yyyy\\/MM\\/dd"));
            repo.Dispose();
        }
    }
}