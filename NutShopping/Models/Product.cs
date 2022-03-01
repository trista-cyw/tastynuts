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
    public class Product
    {
        [Key]
        [Display(Name = "主key")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "商品名稱")]
        [MaxLength(200)]
        [Required(ErrorMessage = "必填")]
        public string productName { get; set; }

        [Display(Name = "商品編號")]
        [MaxLength(200)]
        [Required(ErrorMessage = "必填")]
        public string productNumber { get; set; }

        [Display(Name = "商品描述")]
        [MaxLength(450)]
        [Required(ErrorMessage = "必填")]
        public string productDescription { get; set; }

        [Display(Name = "商品摘要")]
        [MaxLength(450)]
        [Required(ErrorMessage = "必填")]
        public string productSummary { get; set; }

        [Display(Name = "原價")]
        public int productOriPrice { get; set; }

        [Display(Name = "特價")]
        public int productSpePrice { get; set; }

        [Display(Name = "是否上架")]
        public bool productIsLaunched { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [Display(Name = "商品類別")]
        public productClass productClass { get; set; }

        [Display(Name ="上架日期")]
        public DateTime productDate { get; set; }

        [Display(Name ="每份含量")]
        public int? productServing { get; set; }

        [Display(Name = "本包裝含")]
        public int? productIncluding { get; set; }

        [Display(Name = "熱量(份)")]
        public double? productSCalories { get; set; }

        [Display(Name = "熱量(100克)")]
        public double? productGCalories { get; set; }

        [Display(Name = "蛋白質(份)")]
        public double? productSProtein { get; set; }

        [Display(Name = "蛋白質(100克)")]
        public double? productGProtein { get; set; }

        [Display(Name = "脂肪(份)")]
        public double? productSFat { get; set; }

        [Display(Name = "脂肪(100克)")]
        public double? productGFat { get; set; }

        [Display(Name = "飽和脂肪(份)")]
        public double? productSSaturatedFat { get; set; }

        [Display(Name = "飽和脂肪(100克)")]
        public double? productGSaturatedFat { get; set; }

        [Display(Name = "反式脂肪(份)")]
        public double? productSTransFat { get; set; }

        [Display(Name = "反式脂肪(100克)")]
        public double? productGTransFat { get; set; }

        [Display(Name = "碳水化合物(份)")]
        public double? productSCarbohydrate { get; set; }

        [Display(Name = "碳水化合物(100克)")]
        public double? productGCarbohydrate { get; set; }

        [Display(Name = "糖(份)")]
        public double? productSSugar { get; set; }

        [Display(Name = "糖(100克)")]
        public double? productGSugar { get; set; }

        [Display(Name = "鈉(份)")]
        public double? productSNa { get; set; }

        [Display(Name = "鈉(100克)")]
        public double? productGNa { get; set; }

        [Display(Name = "商品封面檔名")]
        [MaxLength(200)]
        public string productImgCover { get; set; }

        //[Display(Name = "第二張照片檔名")]
        //[MaxLength(200)]
        //public string productImg02 { get; set; }

        //[Display(Name = "第三張照片檔名")]
        //[MaxLength(200)]
        //public string productImg03 { get; set; }

        //[Display(Name = "第四張照片檔名")]
        //[MaxLength(200)]
        //public string productImg04 { get; set; }

        [JsonIgnore]
        public virtual ICollection<Order_info> order_infos { get; set; }

        [JsonIgnore]
        public virtual ICollection<Product_img> product_imgs { get; set; }
    }
}