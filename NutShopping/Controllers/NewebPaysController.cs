using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Newtonsoft.Json;
using NutShopping.Models;

namespace NutShopping.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NewebPaysController : ApiController
    {
        private nutDBContext db = new nutDBContext();

        [HttpPost]
        // POST: NewebPay/returnNotify
        [Route("NewebPay/returnNotify")]
        public HttpResponseMessage returnNotify([FromBody] NewebPayReturn newebpayreturn)
        {
            //string requestStr = "Status=SUCCESS&MerchantID=MS125897385&Version=1.6&TradeInfo=27b5993f88e87b3a298cbe14c5d88dd8c6b5492f10ca076c3e414c2a2d00e3d3db8f6324429895f84fffdee0e5e50caaf6f31a9cacbde1d668a9975856310989a3bcaffd2d5098e3c8ce4de8fc5e3671590212a93b9fbd1ac89734397abafb4497441b90c24d5ec8ffdda031ed197f75c972af417a6b628d474edc1d10f07b632b3486e2d800ec30acbefc0b662dec334c7b2497c52b86900a76aec24de150502b89584f6439f18cb06f000b160adbfbe814f5df0b1f261ff319b928072e467f470b7bae955b5c6054a94491e0357cfbb3353457d4414118987f1e48877f7a48beab8ce117725cbaada2097ca1e7b78c9f62a68ff4f055812d2d5722ef521c6b6d112eb3370a053f67198913c2420b06b1767787417ac1b06ec30b83d909a349e4861bc6b1f86b4f98f45617988d5d2673c5de58e8afe31fd32772cd3a8b43985e4e1957225ab9d2397f5d92cd09e2f15b630688e965a3ed5dd70e2f32c347fe&TradeSha=05AFB9C2028762AE720CD10261A6B166F298E59BF3E3CD2110F62C1B0FCDE962";
            string HashKey = "elBLDifvVZCy2KYawypsyx3g6iZlsOG0";
            string HashIV = "CGQxJqMYcVbtqrUP";
            string TradeinfoDecrypt = newebpayreturn.TradeInfo;
            var decryptTradeInfo = DecryptAES256(TradeinfoDecrypt, HashKey, HashIV);
            //"{\"Status\":\"SUCCESS\",\"Message\":\"\\u6a21\\u64ec\\u4ed8\\u6b3e\\u6210\\u529f\",\"Result\":{\"MerchantID\":\"MS125897385\",\"Amt\":3260,\"TradeNo\":\"21111301360176028\",\"MerchantOrderNo\":\"20211113000020\",\"RespondType\":\"JSON\",\"IP\":\"114.39.78.240\",\"EscrowBank\":\"HNCB\",\"PaymentType\":\"WEBATM\",\"PayTime\":\"2021-11-13 01:36:03\",\"PayerAccount5Code\":\"12345\",\"PayBankCode\":\"008\"}}"
            TradeInfoDecrypt tradeinfodecrypt = JsonConvert.DeserializeObject<TradeInfoDecrypt>(decryptTradeInfo);
            DecrpyResult decrpyResult = tradeinfodecrypt.Result;
            var order = db.orders.Where(x => x.orderNumber == decrpyResult.MerchantOrderNo).FirstOrDefault();
            if (order == null) BadRequest("訂單編號不符");
            if (newebpayreturn.Status == "SUCCESS")
            {
                if (decrpyResult.PaymentType == "CREDIT")
                {
                    order.orderPayment = orderPayment.信用卡一次付清;
                }
                else if (decrpyResult.PaymentType == "WEBATM")
                {
                    order.orderPayment = orderPayment.WebATM;
                }
                else
                {
                    order.orderPayment = orderPayment.ATM轉帳;
                }
                order.orderStatus = orderStatus.已付款;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }
            HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }


        public string DecryptAES256(string encryptData, string HashKey, string HashIV)//解密
        {
            var encryptBytes = HexStringToByteArray(encryptData.ToUpper());
            var aes = new RijndaelManaged();
            aes.Key = Encoding.UTF8.GetBytes(HashKey);
            aes.IV = Encoding.UTF8.GetBytes(HashIV);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.None;
            ICryptoTransform transform = aes.CreateDecryptor();
            return Encoding.UTF8.GetString(RemovePKCS7Padding(transform.TransformFinalBlock(encryptBytes, 0, encryptBytes.Length))).Replace("\0","");
        }

        private static byte[] RemovePKCS7Padding(byte[] data)
        {
            int iLength = data[data.Length - 1];
            var output = new byte[data.Length - iLength]; Buffer.BlockCopy(data, 0, output, 0, output.Length);
            return output;
        }

        private static byte[] HexStringToByteArray(string hexString)
        {
            int hexStringLength = hexString.Length;
            byte[] b = new byte[hexStringLength / 2];
            for (int i = 0; i < hexStringLength; i += 2)
            {
                int topChar = (hexString[i] > 0x40 ? hexString[i] - 0x37 : hexString[i] - 0x30) << 4;
                int bottomChar = hexString[i + 1] > 0x40 ? hexString[i + 1] - 0x37 : hexString[i + 1] - 0x30;
                b[i / 2] = Convert.ToByte(topChar + bottomChar);
            }
            return b;
        }
    }
}