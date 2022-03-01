using Jose;
using NutShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace NutShopping.Security
{
    public class JwtAuthUtil
    {
        private static readonly string secretKey = "yiwen&33deNutsFinal"; //加解密的key,如果不一樣會無法成功解密
        private nutDBContext db = new nutDBContext();
        public string GenerateToken(int id)
        {
            var member = db.members.Where(m => m.Id == id).FirstOrDefault();
            Dictionary<string, Object> claim = new Dictionary<string, Object>();//payload 需透過token傳遞的資料
            claim.Add("Id", member.Id);
            claim.Add("Name", member.memberName);
            claim.Add("Exp", DateTime.Now.AddDays(90).ToString());//Token 時效設定100秒
            var payload = claim;
            var token = Jose.JWT.Encode(payload, Encoding.UTF8.GetBytes(secretKey), JwsAlgorithm.HS512);//產生token
            return token;
        }


        public string RefreshToken(Dictionary<string, object> tokendata)
        {
            Dictionary<string, Object> claim = new Dictionary<string, Object>();//payload 需透過token傳遞的資料
            claim.Add("Id", (int)tokendata["Id"]);
            claim.Add("Name", tokendata["Name"].ToString());
            claim.Add("Exp", DateTime.Now.AddDays(90).ToString());//Token 時效設定100秒
            var payload = claim;
            var token = Jose.JWT.Encode(payload, Encoding.UTF8.GetBytes(secretKey), JwsAlgorithm.HS512);//產生token
            return token;
        }
    }
}