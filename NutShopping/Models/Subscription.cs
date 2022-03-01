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
    public class Subscription
    {
        [Key]
        [Display(Name = "主key")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "訂閱商品名稱")]
        [MaxLength(200)]
        [Required(ErrorMessage = "必填")]
        public string subscriptionName { get; set; }

        [Display(Name = "訂閱商品編號")]
        [MaxLength(200)]
        [Required(ErrorMessage = "必填")]
        public string subscriptionNumber { get; set; }

        [Display(Name = "訂閱商品描述")]
        [MaxLength(450)]
        [Required(ErrorMessage = "必填")]
        public string subscriptionDescription { get; set; }

        [Display(Name = "訂閱商品摘要")]
        [MaxLength(450)]
        [Required(ErrorMessage = "必填")]
        public string subscriptionSummary { get; set; }

        [Display(Name = "訂閱商品價格")]
        public int subscriptionPrice { get; set; }

        [Display(Name = "是否上架")]
        public bool subscriptionIsLaunched { get; set; }

        [Display(Name = "上架日期")]
        public DateTime subscriptionDate { get; set; }

        [Display(Name = "每份含量")]
        public int subscriptionServing { get; set; }

        [Display(Name = "本包裝含")]
        public int subscriptionIncluding { get; set; }

        [Display(Name = "熱量(份)")]
        public int subscriptionSCalories { get; set; }

        [Display(Name = "熱量(100克)")]
        public int subscriptionGCalories { get; set; }

        [Display(Name = "蛋白質(份)")]
        public int subscriptionSProtein { get; set; }

        [Display(Name = "蛋白質(100克)")]
        public int subscriptionGProtein { get; set; }

        [Display(Name = "脂肪(份)")]
        public int subscriptionSFat { get; set; }

        [Display(Name = "脂肪(100克)")]
        public int subscriptionGFat { get; set; }

        [Display(Name = "飽和脂肪(份)")]
        public int subscriptionSSaturatedFat { get; set; }

        [Display(Name = "飽和脂肪(100克)")]
        public int subscriptionGSaturatedFat { get; set; }

        [Display(Name = "反式脂肪(份)")]
        public int subscriptionSTransFat { get; set; }

        [Display(Name = "反式脂肪(100克)")]
        public int subscriptionGTransFat { get; set; }

        [Display(Name = "碳水化合物(份)")]
        public int subscriptionSCarbohydrate { get; set; }

        [Display(Name = "碳水化合物(100克)")]
        public int subscriptionGCarbohydrate { get; set; }

        [Display(Name = "糖(份)")]
        public int subscriptionSSugar { get; set; }

        [Display(Name = "糖(100克)")]
        public int subscriptionGSugar { get; set; }

        [Display(Name = "鈉(份)")]
        public int subscriptionSNa { get; set; }

        [Display(Name = "鈉(100克)")]
        public int subscriptionGNa { get; set; }

        [Display(Name = "商品封面檔名")]
        [MaxLength(200)]
        [Required(ErrorMessage = "必填")]
        public string subscriptionImgCover { get; set; }

        //[Display(Name = "第二張照片檔名")]
        //[MaxLength(200)]
        //public string subscriptionImg02 { get; set; }

        //[Display(Name = "第三張照片檔名")]
        //[MaxLength(200)]
        //public string subscriptionImg03 { get; set; }

        //[Display(Name = "第四張照片檔名")]
        //[MaxLength(200)]
        //public string subscriptionImg04 { get; set; }

        [JsonIgnore]
        public virtual ICollection<Order_subinfo> order_subinfos { get; set; }
    }
}