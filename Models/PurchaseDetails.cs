using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ams.Models {
    public class PurchaseDetails {

        [Key]
        public int Id { get; set; }
        public int PurID { get; set; }
        public string Item { get; set; }

        [DisplayFormat( NullDisplayText = "0.00" )]
        public decimal? Quantity { get; set; }
        [DisplayFormat( NullDisplayText = "0.00" )]
        public decimal? Price { get; set; }
        [DisplayFormat( NullDisplayText = "0.00" )]
        public decimal? Total { get; set; }

        //Virtual
        public virtual Purchase Pur { get; set; }

    }
}