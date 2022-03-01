using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using NutShopping.Models;

namespace NutShopping.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class subscriptionsController : ApiController
    {
        private nutDBContext db = new nutDBContext();

        // 訂閱列表
        [HttpGet]
        [Route("api/subscriptionlist")]
        public IHttpActionResult Getsubscriptionlist()
        {
            var subscriptionlist = db.subscriptions.Where(s => s.subscriptionIsLaunched == true).OrderByDescending(s => s.subscriptionDate);
            var subscriptionlistAll = subscriptionlist.Select(s => new
            {
                Id = s.Id,
                subscriptionName=s.subscriptionName,
                subscriptionSummary = s.subscriptionSummary,
                subscriptionImgCover = "/product_Cover/" + s.subscriptionImgCover,
            });
            if (subscriptionlist == null)
            {
                return BadRequest("尚無訂閱商品");
            }
            return Ok(subscriptionlistAll);
        }


        // 訂閱商品介紹
        [HttpGet]
        [Route("api/subscription/{id}")]
        // GET: api/subscription/5
        public IHttpActionResult Getsubscription(int id)
        {
            var subscription = db.subscriptions.Where(x => x.Id == id).FirstOrDefault();
            subscription.subscriptionImgCover = "/product_Cover/" + subscription.subscriptionImgCover;
            var subscriptionAll = new
            {
                subscription,
                subscriptioncCycle = Enum.GetNames(typeof(subscriptioncCycle))
            };
            if (subscription == null)
            {
                return BadRequest("此訂閱商品id不存在");
            }
            return Ok(subscriptionAll);
        }


        // 新增訂閱商品
        [HttpPost]
        // POST: api/addsubscription
        [Route("api/addsubscription")]
        public IHttpActionResult Postsubscription([FromBody] Subscription subscription)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (db.subscriptions.Select(p => p.subscriptionNumber).Contains(subscription.subscriptionNumber)) return BadRequest("此訂閱商品已存在");
            subscription.subscriptionDate = DateTime.Now;
            db.subscriptions.Add(subscription);
            db.SaveChanges();
            return Ok(subscription);
        }


        // 編輯訂閱商品
        [HttpPut]
        // PUT: api/editsubscription/id
        [Route("api/editsubscription/{id}")]
        public IHttpActionResult Putsubscription(int id, [FromBody] Subscription subscription)
        {
            subscription.Id = id;
            if (subscription == null) return BadRequest("此訂閱商品不存在");
            if (db.subscriptions.Where(x => x.Id != id).Select(x => x.subscriptionNumber).Contains(subscription.subscriptionNumber)) return BadRequest("訂閱商品編號重複");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Entry(subscription).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(subscription);
        }
    }
}