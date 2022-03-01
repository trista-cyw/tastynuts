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
    public class Order
    {
        [Key]
        [Display(Name = "主key")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "訂單編號")]
        [MaxLength(50)]
        [Required(ErrorMessage = "必填")]
        public string orderNumber { get; set; }

        [Display(Name = "訂單成立時間")]
        public DateTime orderDate { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [Display(Name = "訂單付費方式")]
        public orderPayment orderPayment { get; set; }

        [Display(Name = "訂單總金額")]
        public int orderTotal { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [Display(Name = "訂單狀態")]
        public orderStatus orderStatus { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [Display(Name = "是否為訂閱")]
        public orderIsSubscription orderIsSubscription { get; set; }

        [Display(Name = "收件人姓名")]
        [MaxLength(200)]
        [Required(ErrorMessage = "必填")]
        public string orderRcName { get; set; }

        [Display(Name = "收件人手機號碼")]
        [MaxLength(50)]
        [Required(ErrorMessage = "必填")]
        public string orderRcMPhone { get; set; }

        [Display(Name = "收件人家用電話")]
        [MaxLength(50)]
        public string orderRcHPhone { get; set; }

        [Display(Name = "收件人電子信箱")]
        [MaxLength(50)]
        [Required(ErrorMessage = "必填")]
        public string orderRcMail { get; set; }

        [Display(Name = "收件人郵遞區號")]
        [MaxLength(10)]
        [Required(ErrorMessage = "必填")]
        public string orderRcPostCode { get; set; }

        [Display(Name = "收件人地址")]
        [MaxLength(200)]
        [Required(ErrorMessage = "必填")]
        public string orderRcAddress { get; set; }

        [Display(Name = "訂單運費")]
        public int orderShipping { get; set; }

        [Display(Name = "訂單商品數量")]
        public int orderAmount { get; set; }

        [Display(Name = "訂單備註")]
        [MaxLength(int.MaxValue)]
        public string orderRemark { get; set; }

        //ForeignKey
        [Display(Name = "會員id")]
        public int memberId { get; set; }
        [ForeignKey("memberId")]
        public virtual Member member { get; set; }

        [JsonIgnore]
        public virtual ICollection<Order_info> order_infos { get; set; }

        [JsonIgnore]
        public virtual ICollection<Order_subinfo> order_subinfos { get; set; }

    }
}