using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailerAPI
{
	/// <summary>
	/// Mail Info
	/// </summary>
	public class MailInfo
	{
		/// <summary>
		/// constructor
		/// </summary>
		public MailInfo()
		{
			this.CC = new List<string>();
			this.Body = new StringBuilder();
		}

		/// <summary>
		/// Addressee
		/// </summary>
        //public string To { get; set; }
        /// <summary>
        /// Mail ToMultiple
        /// </summary>
        public List<string> To { get; set; }

		/// <summary>
		/// Mail Title
		/// </summary>
		public string Subject { get; set; }
		/// <summary>
		/// Type of String, Mail Body
		/// </summary>
		public StringBuilder Body { get; set; }
		/// <summary>
		/// Type of DataTable, Mail Body
		/// </summary>
        //public DataTable TableBody { get; set; }
		/// <summary>
		/// Multiple CC
		/// </summary>
		public List<string> CC { get; set; }
		/// <summary>
		/// Addressee Domain Template
		/// </summary>
        //public string AddresseeTemp { get; set; }

		// @".*@rinnai.com.tw*"
		/// <summary>
		///  Addressee Domain Pattern with Regex
		/// </summary>
        //public string DomainPattern { get; set; }
	}
}
