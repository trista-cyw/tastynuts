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
    public class bannersController : ApiController
    {
        private nutDBContext db = new nutDBContext();

        // show出banner
        [HttpGet]
        // GET: api/banners
        public IHttpActionResult Getbanners()
        {
            int count = 3;
            var today = DateTime.Now.Date;
            var banner = db.banners.OrderByDescending(x=>x.bannerStartDate).Where(x=>(x.bannerStartDate <= today && today <= x.bannerEndDate)).Take(count);
            return Ok(banner);
        }
    }
}