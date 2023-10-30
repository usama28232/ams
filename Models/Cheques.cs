using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ams.Models
{
    public class Cheques
    {
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }
        public int Id { get; set; }
        public string BillNo { get; set; }
        public string Particular { get; set; }
        public string CustomerName { get; set; }
        public int isCheque { get; set; }
        public int amount { get; set; }
    }
}