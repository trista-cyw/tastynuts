using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NutShopping.Models
{
    public class Product_img
    {
        [Key]
        [Display(Name = "主key")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "商品照片檔名")]
        [MaxLength(200)]
        [Required(ErrorMessage = "必填")]
        public string productImgName { get; set; }

        [Display(Name = "商品照片上傳日期")]
        public DateTime productImgDate { get; set; }

        //ForeignKey
        [Display(Name = "商品id")]
        public int productId { get; set; }
        [ForeignKey("productId")]
        public virtual Product product { get; set; }
    }
}