using Jose;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace NutShopping.Security
{
    public class JwtFilter : ActionFilterAttribute
    {
        private static readonly string secretKey = "yiwen&33deNutsFinal";
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            if (!WithoutVerifyToken(request.RequestUri.ToString()))
            {
                if (request.Headers.Authorization == null || request.Headers.Authorization.Scheme != "Bearer")
                {
                    string messageJson = JsonConvert.SerializeObject(new { Status = false, Message = "JwtToken 遺失，需導引重新登入" });
                    var errorMessage = new HttpResponseMessage()
                    {
                        ReasonPhrase = "JwtToken Lost",
                        Content = new StringContent(messageJson,
                                    Encoding.UTF8,
                                    "application/json"),
                    };
                    throw new HttpResponseException(errorMessage);
                }
                else
                {
                    try
                    {
                        // 解密後會回傳 Json 格式的物件 (即加密前的資料)
                        var jwtObject = JWT.Decode<Dictionary<string, Object>>(
                        request.Headers.Authorization.Parameter,
                        Encoding.UTF8.GetBytes(secretKey),
                        JwsAlgorithm.HS512);

                        if (IsTokenExpired(jwtObject["Exp"].ToString()))
                        {
                            string messageJson = JsonConvert.SerializeObject(new { Status = false, Message = "JwtToken 過期，需導引重新登入" });
                            var errorMessage = new HttpResponseMessage()
                            {
                                ReasonPhrase = "JwtToken Expired",
                                Content = new StringContent(messageJson,
                                    Encoding.UTF8,
                                    "application/json"),
                            };
                            throw new HttpResponseException(errorMessage);
                        }
                    }
                    catch (Exception)
                    {
                        string messageJson = JsonConvert.SerializeObject(new { Status = false, Message = "JwtToken 不符，需導引重新登入" });
                        var errorMessage = new HttpResponseMessage()
                        {
                            ReasonPhrase = "JwtToken Not Match",
                            Content = new StringContent(messageJson,
                                    Encoding.UTF8,
                                    "application/json"),
                        };
                        throw new HttpResponseException(errorMessage);
                    }

                }
            }

            base.OnActionExecuting(actionContext);
        }

        //Login不需要驗證因為還沒有token
        public bool WithoutVerifyToken(string requestUri)
        {
            if (requestUri.EndsWith("/Login"))
                return true;
            return false;
        }

        //驗證token時效
        public bool IsTokenExpired(string dateTime)
        {
            return Convert.ToDateTime(dateTime) < DateTime.Now;
        }

        public static Dictionary<string, object> GetToken(string token)
        {
            return JWT.Decode<Dictionary<string, object>>(token, Encoding.UTF8.GetBytes(secretKey), JwsAlgorithm.HS512);
        }

        public static int GetTokenId(string token)
        {
            var tokenData = JWT.Decode<Dictionary<string, object>>(token, Encoding.UTF8.GetBytes(secretKey), JwsAlgorithm.HS512);
            return (int)tokenData["Id"];
        }
    }
}