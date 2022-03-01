using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using NutShopping.Models;
using NutShopping.Security;



namespace NutShopping.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ordersController : ApiController
    {
        private nutDBContext db = new nutDBContext();

        // 我的訂單與訂閱顯示訂單摘要
        [JwtFilter]
        [HttpGet]
        // GET: api/orders/{id in members}
        [Route("api/orders")]
        public IHttpActionResult Getorder()
        {
            var oldtoken = JwtFilter.GetToken(Request.Headers.Authorization.Parameter);
            int id = (int)oldtoken["Id"];
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            var newtoken = jwtAuthUtil.RefreshToken(oldtoken);
            var order = db.orders.Where(x => x.memberId == id).OrderByDescending(x=>x.orderDate);
            var orderall = order.Select(x => new
            {
                x.Id,
                x.orderNumber,
                x.orderDate,
                orderPayment = x.orderPayment.ToString(),
                x.orderTotal,
                x.orderAmount,
                orderStatus = x.orderStatus.ToString(),
                orderIsSubscription = x.orderIsSubscription.ToString()
                /*,
                orderInfo = db.order_infos.Where(i => i.orderId == x.Id).Select(p => new
                {
                    productName = db.products.Where(n => n.Id == p.productId).Select(n => n.productName),
                    p.productAmount
                })*/
            });
            if (order == null)
            {
                return BadRequest("這個會員還沒有訂單");
            }
            return Ok(new { status = true, token = newtoken, orderall });
        }


        // 我的訂單與訂閱顯示訂單明細
        //[JwtFilter]
        [HttpGet]
        // GET: api/orderinfo/{id in orders}
        [Route("api/orderinfo/{id}")]
        public IHttpActionResult GetorderInfo(int id)
        {
            var order = db.orders.Where(x => x.Id == id);
            var orderinfoall = order.Select(x => new
            {
                x.Id,
                x.orderNumber,
                //orderPayment = x.orderPayment.ToString(),
                x.orderRcName,
                x.orderRcMPhone,
                x.orderRcHPhone,
                x.orderRcMail,
                x.orderRcPostCode,
                x.orderRcAddress,
                order_info = db.order_infos.Where(i => i.orderId == x.Id).Select(p => new
                {
                    productName = db.products.Where(n => n.Id == p.productId).Select(n => n.productName),
                    p.productUnitPrice,
                    p.productAmount,
                    p.orderSubtotal
                })
            });
            if (orderinfoall == null)
            {
                return BadRequest("訂單不存在");
            }
            return Ok(orderinfoall);
        }


        // 成立訂單
        [JwtFilter]
        [HttpPost]
        // POST: api/orderConfirmation
        [Route("api/orderConfirmation")]
        public IHttpActionResult Postorder([FromBody] Order_all order_All)
        {
            //產生新token
            var oldtoken = JwtFilter.GetToken(Request.Headers.Authorization.Parameter);
            int id = (int)oldtoken["Id"];
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            var newtoken = jwtAuthUtil.RefreshToken(oldtoken);

            Order order = new Order();
            order = order_All.order;
            Order_info[] order_info = order_All.order_info;
            Order_subinfo[] order_subinfo = order_All.order_subinfo;
            string dateStr = "";
            if (!db.orders.Any())
            {
                dateStr = "0";
            }
            else
            {
                dateStr = db.orders.OrderByDescending(x => x.orderNumber).Select(x => x.orderNumber).FirstOrDefault().ToString();
            }
            order.orderNumber = GetSerialNumber(dateStr);
            if (((int)order.orderPayment) > 1) BadRequest("付款方式錯誤");
            order.orderStatus = orderStatus.待付款;
            //判斷類別、總量、總金額
            if (order_subinfo == null)
            {
                order.orderIsSubscription = orderIsSubscription.訂購;
                order.orderTotal = order_info.Sum(x => x.productAmount * x.productUnitPrice);
                order.orderAmount = order_info.Sum(x => x.productAmount);
            }
            else if (order_info == null)
            {
                order.orderIsSubscription = orderIsSubscription.訂閱;
                order.orderTotal = order_subinfo.Sum(x => x.subscriptionPrice);
                order.orderAmount = order_subinfo.Count();
            }
            else
            {
                order.orderIsSubscription = orderIsSubscription.訂購且訂閱;
                order.orderTotal = order_info.Sum(x => x.productAmount * x.productUnitPrice) + order_subinfo.Sum(x => x.subscriptionPrice);
                order.orderAmount = order_info.Sum(x => x.productAmount) + order_subinfo.Count();
            }
            //判斷運費
            if (order.orderTotal>=2000)
            {
                order.orderShipping = 0;
            }
            else
            {
                order.orderShipping = 160;
            }
            order.orderTotal += order.orderShipping;
            order.orderDate = DateTime.Now;
            order.memberId = id;
            order.orderStatus = orderStatus.待付款;
            db.orders.Add(order);
            db.SaveChanges();
            if (order_subinfo == null) //訂購
            {
                for (int i = 0; i < order_All.order_info.Length; i++)
                {
                    order_info[i].orderId = order.Id;
                    order_info[i].orderSubtotal = order_info[i].productAmount * order_info[i].productUnitPrice;
                    db.order_infos.Add(order_info[i]);
                }
                //var returnOrder = new
                //{
                //    status = true,
                //    newtoken,
                //    order,
                //    order_info = order_info.Select(x => new
                //    {
                //        x.productId,
                //        x.productUnitPrice,
                //        x.productAmount,
                //        x.orderSubtotal
                //    })
                //};
                //db.SaveChanges();
                //return Ok(returnOrder);
            }
            else if (order_info == null) //訂閱
            {
                for (int i = 0; i < order_All.order_subinfo.Length; i++)
                {
                    order_subinfo[i].orderId = order.Id;
                    db.Order_subinfo.Add(order_subinfo[i]);
                }
                //var returnOrder = new
                //{
                //    status = true,
                //    newtoken,
                //    order,
                //    order_subinfo = order_subinfo.Select(x => new
                //    {
                //        x.subscriptiontId,
                //        x.subscriptioncCycle,
                //        x.subscriptionPrice
                //    })
                //};
                //db.SaveChanges();
                //return Ok(returnOrder);
            }
            else //兩者
            {
                for (int i = 0; i < order_All.order_info.Length; i++)
                {
                    order_info[i].orderId = order.Id;
                    order_info[i].orderSubtotal = order_info[i].productAmount * order_info[i].productUnitPrice;
                    db.order_infos.Add(order_info[i]);
                }
                for (int i = 0; i < order_All.order_subinfo.Length; i++)
                {
                    order_subinfo[i].orderId = order.Id;
                    db.Order_subinfo.Add(order_subinfo[i]);
                }
                //var returnOrder = new
                //{
                //    status = true,
                //    newtoken,
                //    order,
                //    order_info = order_info.Select(x => new
                //    {
                //        x.productId,
                //        x.productUnitPrice,
                //        x.productAmount,
                //        x.orderSubtotal
                //    }),
                //    order_subinfo = order_subinfo.Select(x => new
                //    {
                //        x.subscriptiontId,
                //        x.subscriptioncCycle,
                //        x.subscriptionPrice
                //    })
                //};
                //db.SaveChanges();
                //return Ok(returnOrder);
            }
            db.SaveChanges();
            var return2Newebpay = getResult(order.orderTotal, order.orderNumber, order.orderIsSubscription.ToString(), order.orderRcMail, order.orderDate);
            return Ok(new { status = true, newtoken, return2Newebpay });
        }


        private object getResult(int orderTotal, string orderNumber, string orderIsSubscription, string orderRcMail, DateTime orderDate)
        {
            // 必填欄位
            string MerchantID = "MS125897385"; //TradeInfo 必填
            string TradeInfo = "";
            string TradeSha = "";
            string Version = "1.6"; //TradeInfo 必填
            string HashKey = "elBLDifvVZCy2KYawypsyx3g6iZlsOG0";
            string HashIV = "CGQxJqMYcVbtqrUP";
            // TradeInfo內容
            string RespondType = "JSON";
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); //時間戳為1970-01-01至目前秒數
            string TimeStamp = (orderDate - startTime).TotalSeconds.ToString();
            string MerchantOrderNo = orderNumber;
            int Amt = orderTotal;
            string ItemDesc = orderIsSubscription;
            string Email = orderRcMail;
            string NotifyURL = "https://tastynuts.rocket-coding.com/NewebPay/returnNotify";//NotifyURL={NotifyURL}
            string ReturnURL = "https://tastynuts.rocket-coding.com"; 
            int LoginType = 0; //是否須登入藍新會員
            int CREDIT = 1; // 是否啟用信用卡一次付清支付方式
            int WEBATM = 1; // 是否啟用WEBATM轉帳支付方式
            int VACC = 1; //是否啟用ATM轉帳支付方式
            //string[] postDataArr = { "MerchantID", "Version", "RespondType", "TimeStamp", "MerchantOrderNo", "Amt", "ItemDesc", "Email", "LoginType", "CREDIT", "WEBATM", "VACC", "NotifyURL", "ReturnURL" };
            //string postDataStr = "";
            //for (int i = 0; i < postDataArr.Length; i++)
            //{
            //    postDataStr += "&\"" + postDataArr[i] + "\"=" + postDataArr[i];
            //}
            //postDataStr = postDataStr.Trim('&');
            string postDataStr = $"MerchantID={MerchantID}&Version={Version}&RespondType={RespondType}&TimeStamp={TimeStamp}&MerchantOrderNo={MerchantOrderNo}&Amt={Amt}&ItemDesc={ItemDesc}&Email={Email}&LoginType={LoginType}&CREDIT={CREDIT}&WEBATM={WEBATM}&VACC={VACC}&NotifyURL={NotifyURL}&ReturnURL={ReturnURL}";

            TradeInfo = EncryptAES256(postDataStr, HashKey, HashIV);
            TradeSha = getHashSha256($"HashKey={HashKey}&{TradeInfo}&HashIV={HashIV}");
            var postData = new
            {
                MerchantID,
                TradeInfo,
                TradeSha,
                Version
            };
            return postData;
        }

        // AES256加密
        public string EncryptAES256(string source, string HashKey, string HashIV)//加密
        {
            byte[] sourceBytes = AddPKCS7Padding(Encoding.UTF8.GetBytes(source), 32);
            var aes = new RijndaelManaged();
            aes.Key = Encoding.UTF8.GetBytes(HashKey);
            aes.IV = Encoding.UTF8.GetBytes(HashIV);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.None;
            ICryptoTransform transform = aes.CreateEncryptor();
            return ByteArrayToHex(transform.TransformFinalBlock(sourceBytes, 0, sourceBytes.Length)).ToLower();
        }
        private static byte[] AddPKCS7Padding(byte[] data, int iBlockSize)
        {
            int iLength = data.Length;
            byte cPadding = (byte)(iBlockSize - (iLength % iBlockSize)); var output = new byte[iLength + cPadding];
            Buffer.BlockCopy(data, 0, output, 0, iLength);
            for (var i = iLength; i < output.Length; i++)
                output[i] = (byte)cPadding; return output;
        }

        private static string ByteArrayToHex(byte[] barray)
        {
            char[] c = new char[barray.Length * 2]; byte b;
            for (int i = 0; i < barray.Length; ++i)
            {
                b = ((byte)(barray[i] >> 4));
                c[i * 2] = (char)(b > 9 ? b + 0x37 : b + 0x30);
                b = ((byte)(barray[i] & 0xF));
                c[i * 2 + 1] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            }
            return new string(c);
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
            return Encoding.UTF8.GetString(RemovePKCS7Padding(transform.TransformFinalBlock(encryptBytes, 0, encryptBytes.Length)));
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


        // SHA256加密
        public static string getHashSha256(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString.ToUpper();
        }


        private string GetSerialNumber(string dateStr)
        {
            if (dateStr != "0")
            {
                string headDate = dateStr.Substring(0, 8);
                int lastNumber = int.Parse(dateStr.Substring(8));
                if (headDate == DateTime.Now.ToString("yyyyMMdd"))
                {
                    lastNumber++;
                    return headDate + lastNumber.ToString("000000");
                }
            }
            return DateTime.Now.ToString("yyyyMMdd") + "000001";
        }
    }
}