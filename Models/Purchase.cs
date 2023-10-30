using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ams.Models {
    public class Purchase {

        [Key]
        public int PurchaseID { get; set; }
        public string BillNo { get; set; }

        [Display( Name = "Date" )]
        [DataType( DataType.Date ), DisplayFormat( DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true )]
        public DateTime? Date { get; set; }

        [Required]
        public int SupId { get; set; }

        [DisplayFormat( NullDisplayText = "0.00" )]
        public decimal? GAmount { get; set; }

        [DisplayFormat( NullDisplayText = "0.00" )]
        public decimal? Received { get; set; }

        [DisplayFormat( NullDisplayText = "0.00" )]
        public decimal? Balance { get; set; }

        //Virtual
        public virtual Supplier Sup { get; set; }

    }
}