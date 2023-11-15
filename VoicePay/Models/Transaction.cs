using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace VoicePay.Models
{
	public class Transaction
	{
		public string TransactionID { get; set; }
		public string ReceiverUEN { get; set; }
		public string SenderAccountNumber { get; set; }
		public string Amount { get; set; }


	}
}
