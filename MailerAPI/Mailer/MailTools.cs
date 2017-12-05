using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MailerAPI
{
	/// <summary>
	/// Mail Tools
	/// </summary>
	public static class MailTools
	{
		/// <summary>
		/// According Type of Body, Format to HTML Table.
		/// </summary>
		/// <param name="body">Type of object, Mail Body Content</param>
		/// <returns>Type of String, HTML Table</returns>
		public static string BodyToTable(object body)
		{
			var result = String.Empty;
			var type = body.GetType();
			switch (Type.GetTypeCode(type))
			{
				case TypeCode.String:
					result = (string)body;
					break;
				default:
					// Fallback to using if-else statements...
					if (type == typeof(DataTable))
					{
						//parse datatable to html table
						result = @"{0}<table border=1 style=''>{1}</table>";
						var th = new StringBuilder();
						var tr = new StringBuilder();
						foreach (DataColumn column in ((DataTable)body).Columns)
						{
							th.AppendFormat("<th>{0}</th>", column.Caption);
						}
						foreach (DataRow row in ((DataTable)body).Rows)
						{
							var td = String.Empty;
							foreach (var item in row.ItemArray)
							{
								td += String.Format("<td>{0}</td>", item);
							}
							tr.AppendFormat("<tr>{0}</tr>", td);
						}
						tr.Insert(0, th);
						result = String.Format(result, ((DataTable)body).TableName, tr);
					}
					break;
			}

			return result;
		}


	}
}
