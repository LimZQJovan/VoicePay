using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace VoicePay.Models
{
	public class BankDetails
	{
		public string BankName { get; set; }
		public string BankID { get; set; }
		public string AccountNumber { get; set; }
		public string PayeeName { get; set; }
		public string MobileNo { get; set;}

	}
}
