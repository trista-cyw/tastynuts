using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NutShopping.Models
{
    public class Member
    {
        [Key]
        [Display(Name = "主key")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "會員姓名")]
        [MaxLength(200)]
        [Required(ErrorMessage = "必填")]
        public string memberName { get; set; }

        [Display(Name = "會員生日")]
        public DateTime? memberBirth { get; set; }

        [Display(Name = "會員手機電話")]
        [MaxLength(50)]
        public string memberMobilePhone { get; set; }

        [Display(Name = "會員家用電話")]
        [MaxLength(50)]
        public string memberHomePhone { get; set; }

        [Display(Name = "會員電子信箱")]
        [MaxLength(50)]
        [Required(ErrorMessage = "必填")]
        public string memberMail { get; set; }

        [Display(Name = "會員郵遞區號")]
        [MaxLength(10)]
        public string memberPostcode { get; set; }

        [Display(Name = "會員地址")]
        [MaxLength(200)]
        public string memberAddress { get; set; }

        [Display(Name = "會員密碼")]
        [MaxLength(200)]
        public string memberPassword { get; set; }

        [Display(Name = "會員密碼鹽")]
        [MaxLength(5)]
        public string memberPasswordSalt { get; set; }

        [Display(Name = "會員是否驗證信箱")]
        public bool memberIsVerified { get; set; }

        [Display(Name = "是否社群登入")]
        public bool memberIsSNS { get; set; }

        [Display(Name = "會員GUID")]
        [MaxLength(50)]
        public string memberGUID { get; set; }

        [Display(Name = "會員驗證到期時間")]
        public DateTime? memberGUIDValidTime { get; set; }

        [JsonIgnore]
        public virtual ICollection<Order> orders { get; set; }
    }
}