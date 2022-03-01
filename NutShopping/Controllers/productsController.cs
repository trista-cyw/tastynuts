using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    public class productsController : ApiController
    {
        private nutDBContext db = new nutDBContext();

        // 商品介紹
        [HttpGet]
        // GET: api/products/5
        [Route("api/products/{id}")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult Getproduct(int id)
        {
            var product = db.products.Where(x => x.Id == id).FirstOrDefault();
            if (product == null)
            {
                return BadRequest("此商品id不存在");
            }
            product.productImgCover = "/product_Cover/" + product.productImgCover;
            if (db.Product_img.Any(x=>x.productId == id))
            {
                var productImg = db.Product_img.Where(p => p.productId == id).Select(p => new
                {
                    ImgId = p.Id,
                    productImgName = "/product_Img/" + p.productImgName,
                    p.productImgDate
                });
                var productAll = new
                {
                    product,
                    productImg
                };
                return Ok(productAll);
            }
            else
            {
                var productAll = new
                {
                    product
                };
                return Ok(productAll);
            }
        }

        // 商品列表
        [HttpGet]
        [Route("api/productlist/{page:int?}")]
        public IHttpActionResult Getproductlist(int page = 1)
        {
            int count = 9;
            double maxpage = db.products.Count() / (double)count;
            maxpage = Math.Ceiling(maxpage);
            var productlist = db.products.Where(x => x.productIsLaunched == true).OrderBy(p => p.productClass).Skip((page - 1) * count).Take(count);
            var productListAll = new
            {
                maxpage = maxpage,
                productlist = productlist.Select(x => new
                {
                    Id = x.Id,
                    productName = x.productName,
                    productOriPrice = x.productOriPrice,
                    productSpePrice = x.productSpePrice,
                    productImgCover = "/product_Cover/" + x.productImgCover
                })
            };
            if (productlist == null)
            {
                return BadRequest("此頁無商品");
            }
            return Ok(productListAll);
        }


        // 商品分類列表
        [HttpGet]
        [Route("api/productlistclass/{productclass}")]
        public IHttpActionResult Getclass(productClass productclass)
        {
            var productlist = db.products.Where(x => x.productIsLaunched == true && x.productClass == productclass);
            var productListAll = productlist.Select(x => new
            {
                Id = x.Id,
                productName = x.productName,
                productOriPrice = x.productOriPrice,
                productSpePrice = x.productSpePrice,
                productImgCover = x.productImgCover
            });
            if (productlist == null)
            {
                return BadRequest("此類別無商品");
            }
            return Ok(productListAll);
        }


        // 新增商品
        [HttpPost]
        // POST: api/addproduct
        [Route("api/addproduct")]
        public IHttpActionResult Postproduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (db.products.Select(p => p.productNumber).Contains(product.productNumber)) return BadRequest("此商品已存在");
            product.productDate = DateTime.Now;
            string coverStr = db.products.OrderByDescending(x => x.productImgCover).Select(x => x.productImgCover).FirstOrDefault().ToString();
            product.productImgCover = GetCoverName(coverStr);
            ////處理檔案
            //string nowHost = Request.RequestUri.Host;
            //string path = "https://" + nowHost + "/image/";
            ////實體路徑
            //string physicalPath = System.Web.HttpContext.Current.Server.MapPath("~/image/");
            //HttpFileCollection files = HttpContext.Current.Request.Files;
            //for (int i = 0; i < files.Count; i++)
            //{
            //    HttpPostedFile file = files[i];
            //    string filepath = physicalPath + files[i].FileName;
            //    if (!File.Exists(@"filepath"))
            //    {
            //        file.SaveAs(@"filepath");
            //    }
            //}
            //product.productImgCover = files[0].FileName;
            //product.productImg02 = files[1].FileName;
            //product.productImg03 = files[2].FileName;
            //product.productImg04 = files[3].FileName;
            db.products.Add(product);
            db.SaveChanges();
            return Ok(product);
        }

        private string GetCoverName(string coverStr)
        {
            if (coverStr != "0")
            {
                string headDate = coverStr.Substring(0, 8);
                int lastNumber = int.Parse(coverStr.Substring(8, 6));
                if (headDate == DateTime.Now.ToString("yyyyMMdd"))
                {
                    lastNumber++;
                    return headDate + lastNumber.ToString("000000") + ".jpg";
                }
            }
            return DateTime.Now.ToString("yyyyMMdd") + "000001" + ".jpg";
        }

        // 編輯商品
        [HttpPut]
        // PUT: api/editproduct/id
        [Route("api/editproduct/{id}")]
        public IHttpActionResult Putproduct(int id, [FromBody] Product product)
        {
            product.Id = id;
            if (product == null) return BadRequest("此商品不存在");
            if (db.products.Where(x => x.Id != id).Select(x => x.productNumber).Contains(product.productNumber)) return BadRequest("商品編號重複");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(product);
        }
    }
}