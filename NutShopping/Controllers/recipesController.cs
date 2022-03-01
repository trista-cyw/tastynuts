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
    public class recipesController : ApiController
    {
        private nutDBContext db = new nutDBContext();


        // 菜單列表
        [HttpGet]
        // GET: api/recipeslist
        [Route("api/recipeslist")]
        public IHttpActionResult GetrecipeList()
        {
            var recipelist = db.recipes.OrderByDescending(p => p.recipeDate).Where(p => p.recipeIsDraft == false);
            var recipeListAll = new
            {
                recipelist = recipelist.Select(x => new
                {
                    Id = x.Id,
                    recipeTitle = x.recipeTitle,
                    recipeSummary = x.recipeSummary,
                    recipeCover = "/recipe/" + x.recipeCover
                }),
            };
            if (recipeListAll == null)
            {
                return BadRequest("還沒有菜單喔");
            }

            return Ok(recipeListAll);
        }


        // 菜單內容
        [HttpGet]
        // GET: api/recipes/5
        [ResponseType(typeof(Recipe))]
        public IHttpActionResult Getrecipe(int id)
        {
            var recipe = db.recipes.Where(x => x.Id == id).FirstOrDefault();
            recipe.recipeCover = "/recipe/" + recipe.recipeCover;
            if (recipe == null)
            {
                return BadRequest("此菜單id不存在");
            }
            return Ok(recipe);
        }


        // 新增菜單
        [HttpPost]
        // POST: api/addsubscription
        [Route("api/addrecipe")]
        public IHttpActionResult Postrecipe([FromBody] Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (db.recipes.Select(p => p.recipeTitle).Contains(recipe.recipeTitle)) return BadRequest("此菜單已存在");
            recipe.recipeDate = DateTime.Now;
            db.recipes.Add(recipe);
            db.SaveChanges();
            return Ok(recipe);
        }


        // 編輯菜單
        [HttpPut]
        // PUT: api/editrecipe/id
        [Route("api/editrecipe/{id}")]
        public IHttpActionResult Putrecipe(int id, [FromBody] Recipe recipe)
        {
            recipe.Id = id;
            if (recipe == null) return BadRequest("此商品不存在");
            if (db.recipes.Where(x => x.Id != id).Select(x => x.recipeTitle).Contains(recipe.recipeTitle)) return BadRequest("菜單名稱重複");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Entry(recipe).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(recipe);
        }
    }
}