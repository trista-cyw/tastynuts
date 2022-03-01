using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using NutShopping.Models;
using NutShopping.Security;

namespace NutShopping.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    /// <summary>
    /// 使用者端 - 會員資料
    /// </summary>
    public class membersController : ApiController
    {
        /// <summary>
        /// 建構式
        /// </summary>
        private nutDBContext db = new nutDBContext();

        /// <summary>
        /// 使用者端 - 購物車頁面顯示會員資料
        /// </summary>
        /// <returns></returns>
        [JwtFilter]
        [HttpGet]
        [Route("api/membership")]
        public IHttpActionResult Getmember4ship()
        {
            var oldtoken = JwtFilter.GetToken(Request.Headers.Authorization.Parameter);
            int id = (int)oldtoken["Id"];
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            var newtoken = jwtAuthUtil.RefreshToken(oldtoken);
            var member = db.members.Where(x => x.Id == id);
            var showmember = member.Select(x => new
            {
                x.Id,
                x.memberName,
                x.memberMobilePhone,
                x.memberHomePhone,
                x.memberMail,
                x.memberPostcode,
                x.memberAddress,
                x.memberIsVerified
            }).FirstOrDefault();
            if (showmember == null)
            {
                return BadRequest("這個會員id不存在");
            }
            return Ok(new { status = true, token = newtoken, showmember });
        }

        /// <summary>
        /// 使用者端 - 會員中心顯示會員資料
        /// </summary>
        /// <returns></returns>
        [JwtFilter]
        [HttpGet]
        [Route("api/members")]
        public IHttpActionResult Getmember()
        {
            var oldtoken = JwtFilter.GetToken(Request.Headers.Authorization.Parameter);
            int id = (int)oldtoken["Id"];
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            var newtoken = jwtAuthUtil.RefreshToken(oldtoken);
            var member = db.members.Where(x => x.Id == id);
            var memberInfo = member.Select(x => new
            {
                x.Id,
                x.memberName,
                x.memberMobilePhone,
                x.memberHomePhone,
                x.memberMail,
                x.memberPostcode,
                x.memberAddress,
                x.memberBirth
            }).FirstOrDefault();
            if (memberInfo == null)
            {
                return BadRequest("這個會員id不存在");
            }
            return Ok(new { status = true, token = newtoken, memberInfo });
        }


        /// <summary>
        /// 使用者端 - 註冊會員資料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        // POST: api/members
        [Route("api/addmembers")]
        //[ResponseType(typeof(member))]
        public IHttpActionResult Postmember([FromBody] Member member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!Regex.IsMatch(member.memberMail, @"[a-zA-Z0-9_]+@[a-zA-Z0-9\._]+")) return BadRequest("請確認E-mail是否正確");
            if (db.members.Select(m => m.memberMail).Contains(member.memberMail.ToLower())) return BadRequest("此信箱已註冊");
            member.memberGUID = Guid.NewGuid().ToString();
            member.memberGUIDValidTime = DateTime.Now.AddMinutes(10);
            member.memberPasswordSalt = member.memberGUID.Substring(0, 5);
            member.memberPassword = BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(member.memberPassword + member.memberPasswordSalt))).Replace("-", null);

            // 取得現在網域
            string nowHost = Request.RequestUri.Host;
            // 寄驗證信
            sendVerificationLetter(member.memberMail, member.memberGUID, nowHost, member.memberName);
            db.members.Add(member);
            db.SaveChanges();
            return Ok(new { id = member.Id, member });
        }

        // 寄驗證信
        private void sendVerificationLetter(string memberMail, string memberGUID, string nowHost, string memberName)
        {
            string verifiedLink = "https://" + nowHost + "/verify?guid=" + memberGUID;
            //寄給使用者
            MailMessage mail = new MailMessage();
            //前面是發信email後面是顯示的名稱
            mail.From = new MailAddress("tastynuts.service@gmail.com", "品辰歲月");
            //收信者email
            mail.To.Add(memberMail);
            //設定優先權
            mail.Priority = MailPriority.Normal;
            //標題
            mail.Subject = "品辰歲月 - 驗證您的電子信箱";
            //內容
            string mailContent = memberName + "您好:<br>請點選連結以驗證您的電子信箱。<br>" + verifiedLink + "<br>此郵件為自動產生及發送，請勿直接回覆，謝謝。<br>";
            mailContent += "若您最近沒有嘗試以此電子信箱建立新帳戶，請放心忽略此郵件。<br>";
            mail.Body = mailContent;
            //內容使用html
            mail.IsBodyHtml = true;
            //設定gmail的smtp
            SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);
            //您在gmail的帳號密碼
            MySmtp.Credentials = new System.Net.NetworkCredential("tastynuts.service@gmail.com", "nutnut1234");
            //開啟ssl
            MySmtp.EnableSsl = true;
            //發送郵件
            MySmtp.Send(mail);
            //放掉宣告出來的MySmtp
            MySmtp = null;
            //放掉宣告出來的mail
            mail.Dispose();
        }


        /// <summary>
        /// 使用者端 - 修改驗證欄位
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        // PUT: api/verify/5
        [Route("api/verify/{GUID}")]
        public IHttpActionResult PutVerify(string GUID)
        {
            var nowtime = DateTime.Now;
            var members = db.members.Where(m => m.memberGUID == GUID).FirstOrDefault();
            if (nowtime <= members.memberGUIDValidTime)
            {
                members.memberIsVerified = true;
                db.SaveChanges();
                return Ok("驗證成功");
            }
            else
            {
                members.memberGUID = Guid.NewGuid().ToString();
                members.memberGUIDValidTime = nowtime.AddMinutes(10);
                // 取得現在網域
                string nowHost = Request.RequestUri.Host;
                db.SaveChanges();
                // 寄驗證信
                sendVerificationLetter(members.memberMail, members.memberGUID, nowHost, members.memberName);
                return BadRequest("驗證已逾時，請重新驗證。");
            }
        }

        /// <summary>
        /// 使用者端 - 修改會員資料
        /// </summary>
        /// <returns></returns>
        [JwtFilter]
        [HttpPut]
        // PUT: api/editmember/5
        [Route("api/editmember")]
        public IHttpActionResult Putmember([FromBody] Member member)
        {
            var oldtoken = JwtFilter.GetToken(Request.Headers.Authorization.Parameter);
            int id = (int)oldtoken["Id"];
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            var newtoken = jwtAuthUtil.RefreshToken(oldtoken);
            var db_members = db.members.Where(x => x.Id == id).FirstOrDefault();
            if (db_members == null) return BadRequest("此會員不存在");
            //if (!ModelState.IsValid) return BadRequest(ModelState);
            db_members.memberName = member.memberName;
            db_members.memberBirth = member.memberBirth;
            db_members.memberMobilePhone = member.memberHomePhone;
            db_members.memberHomePhone = member.memberHomePhone;
            db_members.memberPostcode = member.memberPostcode;
            db_members.memberAddress = member.memberAddress;
            db.Entry(db_members).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(new { status = true, token = newtoken, db_members });
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        private bool memberExists(int id)
        {
            return db.members.Count(e => e.Id == id) > 0;
        }


        /// <summary>
        /// 使用者端 - 登入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/login")]
        public Object Post([FromBody] Member member)
        {
            var memberDB = db.members.Where(x => x.memberMail == member.memberMail).FirstOrDefault();
            var loginPassword = BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(member.memberPassword + memberDB.memberPasswordSalt))).Replace("-", null);
            var test = BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes("ckshrimp" + "69026"))).Replace("-", null);
            if (member.memberMail == memberDB.memberMail && loginPassword == memberDB.memberPassword.ToString())
            {
                JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
                string jwtToken = jwtAuthUtil.GenerateToken(memberDB.Id);
                return new
                {
                    status = true,
                    token = jwtToken
                };
            }
            else
            {
                return new
                {
                    status = false,
                    token = "Account Or Password Error"
                };
            }
        }

        //[HttpGet]
        //[Route("Verify/{guid}")]
        //public IHttpActionResult Getguid(string guid)
        //{
        //    var member = db.members.Where(x => x.memberGUID == guid);

        //}
    }
}