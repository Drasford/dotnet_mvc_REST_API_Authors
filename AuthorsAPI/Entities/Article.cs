using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorsAPI.Entities
{
    public class Article
    {
        public string Id { get; set; }
        [BindProperty(Name = "title", SupportsGet = true)]
        public string Title { get; set; }
        public string DatePublished { get; set; }
        public string Level { get; set; }

    }
}
