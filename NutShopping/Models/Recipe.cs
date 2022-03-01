using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NutShopping.Models
{
    public class Recipe
    {
        [Key]
        [Display(Name = "主key")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "菜單名稱")]
        [MaxLength(200)]
        [Required(ErrorMessage = "必填")]
        public string recipeTitle { get; set; }

        [Display(Name = "菜單描述")]
        [MaxLength(int.MaxValue)]
        [Required(ErrorMessage = "必填")]
        public string recipeSummary { get; set; }

        [Display(Name = "菜單食材")]
        [MaxLength(450)]
        [Required(ErrorMessage = "必填")]
        public string recipeIngredient { get; set; }

        [Display(Name = "菜單步驟一")]
        [MaxLength(450)]
        [Required(ErrorMessage = "必填")]
        public string recipeStep01 { get; set; }

        [Display(Name = "菜單步驟二")]
        [MaxLength(450)]
        public string recipeStep02 { get; set; }

        [Display(Name = "菜單步驟三")]
        [MaxLength(450)]
        public string recipeStep03 { get; set; }

        [Display(Name = "菜單封面檔名")]
        [MaxLength(200)]
        [Required(ErrorMessage = "必填")]
        public string recipeCover { get; set; }

        [Display(Name = "菜單上傳日期")]
        public DateTime recipeDate { get; set; }

        [Display(Name = "菜單是否為草稿")]
        public bool recipeIsDraft { get; set; }
    }
}