using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NutShopping.Models
{
    public enum productClass
    {
        堅果系列,
        乾果系列,
        經典禮盒,
        辦公室團購首選
    }

    public enum orderPayment
    {
        線上付款,
        貨到付款,
        信用卡一次付清,
        WebATM,
        ATM轉帳
    }

    public enum orderStatus
    {
        待付款,
        已付款,
        已出貨,
        訂單取消
    }

    public enum subscriptioncCycle
    {
        雙週,
        月,
        季
    }

    public enum orderIsSubscription
    {
        訂購,
        訂閱,
        訂購且訂閱
    }
}