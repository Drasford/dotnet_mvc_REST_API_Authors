using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorsAPI.Entities
{
    public class AuthorsHelper
    {
        public static List<Author> GetAllAuthors()
        {
            List<Author> authors = new List<Author>();

            using (StreamReader r = new StreamReader("./Data/Authors.json"))
            {
                string json = r.ReadToEnd();
                authors = JsonConvert.DeserializeObject<List<Author>>(json);
            }

            return authors;
        }

        public static Author GetAuthorById(int id)
        {
            return GetAllAuthors().FirstOrDefault(a => a.UserID == id);
        }

        public static List<Article> GetAllArticles(int id, string title="",string level = "",string publishedDate="")
        {
            var articles = GetAuthorById(id).articles;

            if (!String.IsNullOrEmpty(title))
            {
                articles = articles.Where(a => a.Title == title).ToList();
            }
            if (!String.IsNullOrEmpty(level))
            {
                articles = articles.Where(a => a.Level == level).ToList();
            }
            if (!String.IsNullOrEmpty(publishedDate))
            {
                articles = articles.Where(a => a.DatePublished == publishedDate).ToList();
            }


            return articles;
        }
        public static List<Article> GetArticleByTitle(int id, string title)
        {
            return GetAuthorById(id).articles.Where(a => a.Title == title).ToList();
        }
        public static List<Article> GetArticleByLevel(int id, string level)
        {
            return GetAuthorById(id).articles.Where(a => a.Level == level).ToList();
        }

        public static void AddNewAuthor(Author author)
        {
           
            var allAuthors = GetAllAuthors();
            if (allAuthors.Any(e => e.UserID == author.UserID))
            {
                throw new Exception();
            }

            allAuthors.Add(author);

            JsonSerializer serializer = new JsonSerializer();


            using (StreamWriter sw = new StreamWriter("./Data/Authors.json"))
            {
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    serializer.Serialize(jw, allAuthors);
                }
            }
        }

        public static void AddNewArticle(int id, Article article)
        {
            var allAuthors = GetAllAuthors();
            var selectedAuthor =allAuthors.Where(x => x.UserID.Equals(id)).FirstOrDefault();

            if (selectedAuthor.articles.Any(x => x.Id.Equals(article.Id)))
            {
                throw new Exception();
            }

            selectedAuthor.articles.Add(article);
            var index = allAuthors.IndexOf(selectedAuthor);
            allAuthors[index] = selectedAuthor;

            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter("./Data/Authors.json"))
            {
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    serializer.Serialize(jw, allAuthors);
                }
            }

        }
        public static void UpdateArticle(int userId,Article article, int articleId)
        {
            var allAuthors = GetAllAuthors();
            var selectedAuthor = allAuthors.Where(x => x.UserID.Equals(userId)).FirstOrDefault();
            var selectedArticle = selectedAuthor.articles.Where(x => x.Id.Equals(articleId.ToString())).FirstOrDefault();

            var indexOfSelectedArticle = selectedAuthor.articles.IndexOf(selectedArticle);
            selectedArticle = article;
            selectedAuthor.articles[indexOfSelectedArticle] = selectedArticle;
            var index = allAuthors.IndexOf(selectedAuthor);
            allAuthors[index] = selectedAuthor;

            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter("./Data/Authors.json"))
            {
                using(JsonWriter jw = new JsonTextWriter(sw))
                {
                    serializer.Serialize(jw, allAuthors);
                }
            }
        }

    }
    
}
