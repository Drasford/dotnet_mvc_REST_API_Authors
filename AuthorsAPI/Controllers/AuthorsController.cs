using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorsAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace AuthorsAPI.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        // GET: api/Authors
        [HttpGet]
        public List<Author> Get()
        {
            var authors = AuthorsHelper.GetAllAuthors();
            return authors;
        }

        // GET: api/Authors/5
        //[HttpGet("{id}", Name = "Get")]
        [Route("{id}")]
        public Author Get(int id)
        {
            var author = AuthorsHelper.GetAuthorById(id);
            return author;
        }

        //GET : api/authors/5/articles
        [Route("{id}/articles")]
        [HttpGet]
        //[HttpGet("{id}/articles", Name = "Author_articles")]
        public List<Article> GetArticlesByAuthor(int id, string title = "", string level="",string publishedDate="")
        {

            var article = AuthorsHelper.GetAllArticles(id, title,level,publishedDate);
            return article;

        }

        [Route("{id}/articles")]
        [HttpPost]
        public ActionResult PostArticle(int id, [FromBody] Article article)
        {
            StringValues admin = "admin";
            if (Request.Headers.ContainsKey("Authorization") && Request.Headers.TryGetValue("Authorization",out admin))
            {
                AuthorsHelper.AddNewArticle(id, article);
                return RedirectToAction("api/authors");
            }
            else
            {
                return Content("Not authorized for this action :)");
            }
          
        }

        // POST: api/Authors
        [HttpPost]
        public ActionResult Post([FromBody] Author author)
        {
            StringValues admin = "admin";
            if (Request.Headers.ContainsKey("Authorization") && Request.Headers.TryGetValue("Authorization", out admin))
            {
                var allAuthors = AuthorsHelper.GetAllAuthors();
                if (ModelState.IsValid && author != null)
                {
                    if (allAuthors.Any(a => a.UserID == author.UserID))
                    {
                       throw new Exception();
                    }
                    AuthorsHelper.AddNewAuthor(author);
                    return RedirectToAction("api/authors");
                }
                else
                {
                    return Content("Something bad happened :)");
                }
            }
            else
            {
                return Content("Not authorized for this action :)");
            }
           

        }

        // PUT: api/Authors/5
        [Route("{id}/articles/{aid}")]
        [HttpPut]
        public ActionResult Put(int id, int aid, [FromBody] Article article)
        {

            StringValues admin = "admin";
            if (Request.Headers.ContainsKey("Authorization") && Request.Headers.TryGetValue("Authorization", out admin))
            {
                AuthorsHelper.UpdateArticle(id, article, aid);
                return RedirectToAction("GetArticlesByAuthor");
            }
            else
            {
                return Content("Not authorized for this action :)");
            }
        }



        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
