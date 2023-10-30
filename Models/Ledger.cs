using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ams.Models
{
    public class Ledger
    {
        [Key]
        public int LedgerID { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        public int CustomerId { get; set; }
        public string BillNo { get; set; }
        public string Particular { get; set; }
        public int Debit { get; set; }
        public int Credit { get; set; }
        public int Balance { get; set; }
        public int isCheque { get; set; }
        public int isSeen { get; set; }
    }
}