using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ams.Models {
    public class PaymentAdjustment {

        [Key]
        public int AdjustmentID { get; set; }

        [Required]
        public int CusSupID { get; set; }

        [DisplayFormat( NullDisplayText = "0.00" )]
        public decimal? Amount { get; set; }

        public string Remarks { get; set; }

        [StringLength(1)]
        public string type { get; set; }

        [Display( Name = "Date" )]
        [DataType( DataType.Date ), DisplayFormat( DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true )]
        public DateTime? Date { get; set; }

    }
}