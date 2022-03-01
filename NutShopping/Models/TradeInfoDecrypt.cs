using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NutShopping.Models
{
    public class TradeInfoDecrypt
    {
        /// <summary>
        /// 回傳狀態
        /// <para>1.若交易付款成功，則回傳SUCCESS。</para>
        /// <para>2.若交易付款失敗，則回傳錯誤代碼。</para>
        /// <para>3.若使用新增自訂支付欄位之交易，則回傳CUSTOM。</para>
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 回傳訊息(敘述此次交易狀態。)
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 當 RespondType為JSON時，回傳參數會放至此陣列下。
        /// </summary>
        public DecrpyResult Result { get; set; }


       
    }
}