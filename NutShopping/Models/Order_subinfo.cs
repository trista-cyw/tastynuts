using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NutShopping.Models
{
    public class Order_subinfo
    {
        [Key]
        [Display(Name = "主key")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //ForeignKey
        [Display(Name = "訂閱商品id")]
        public int subscriptiontId { get; set; }
        [ForeignKey("subscriptiontId")]
        public virtual Subscription Subscription { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [Display(Name = "訂閱週期")]
        public subscriptioncCycle subscriptioncCycle { get; set; }

        [Display(Name = "訂閱小計")]
        public int subscriptionPrice { get; set; }

        //ForeignKey
        [Display(Name = "訂單id")]
        public int orderId { get; set; }
        [ForeignKey("orderId")]
        public virtual Order order { get; set; }
    }
}