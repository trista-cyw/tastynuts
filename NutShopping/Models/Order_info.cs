using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NutShopping.Models
{
    public class Order_info
    {
        [Key]
        [Display(Name = "主key")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //ForeignKey
        [Display(Name = "商品id")]
        public int productId { get; set; }
        [ForeignKey("productId")]
        public virtual Product product { get; set; }

        [Display(Name = "商品數量")]
        public int productAmount { get; set; }

        [Display(Name = "商品小計")]
        public int productUnitPrice { get; set; }

        [Display(Name = "訂單小計")]
        public int orderSubtotal { get; set; }

        //ForeignKey
        [Display(Name = "訂單id")]
        public int orderId { get; set; }
        [ForeignKey("orderId")]
        public virtual Order order { get; set; }
    }
}