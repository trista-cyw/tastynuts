using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NutShopping.Models
{
    public class Banner
    {
        [Key]
        [Display(Name = "主key")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "banner檔案名稱")]
        [MaxLength(200)]
        [Required(ErrorMessage = "必填")] 
        public string bannerName { get; set; }

        [Display(Name = "banner開始日期")]
        public DateTime bannerStartDate { get; set; }

        [Display(Name = "banner結束日期")]
        public DateTime bannerEndDate { get; set; }
    }
}