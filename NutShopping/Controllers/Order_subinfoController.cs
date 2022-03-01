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
    public class Order_subinfoController : ApiController
    {
        private nutDBContext db = new nutDBContext();

        
    }
}