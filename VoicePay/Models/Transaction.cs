using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace VoicePay.Models
{
    public class Transaction
    {
        public string? PayeeName { get; set; }
        public string? MobileNo { get; set; }
        public string? SenderAccountNumber { get; set; }
        public string? TransactionID { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.00}")]
        public decimal Amount { get; set; }


        public DateTime TransactionDateTime { get; set; }
        public string? ReceiverUEN { get; set; }
       
      
        public string? ReferenceNo { get; set; }

      
    }
}
