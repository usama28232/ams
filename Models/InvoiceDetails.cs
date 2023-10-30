using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ams.Models
{
    public class InvoiceDetails
    {

        [Key]
        public int Id { get; set; }
        public int Invid { get; set; }
        public string Item { get; set; }

        [DisplayFormat(NullDisplayText = "0.00")]
        public decimal? Quantity { get; set; }
        [DisplayFormat(NullDisplayText = "0.00")]
        public decimal? Price { get; set; }
        [DisplayFormat(NullDisplayText = "0.00")]
        public decimal? Total { get; set; }

        //Virtual
        public virtual Invoice Inv { get; set; }

    }
}