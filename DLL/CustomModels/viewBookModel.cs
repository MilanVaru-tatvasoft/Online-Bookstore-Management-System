using DataAccess.DataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CustomModels
{
    public class viewBookModel
    {
        public List<Addtocart>? Addtocarts {  get; set; }  
        public int? cartId {  get; set; }
        public string? categoryName {  get; set; }
        public int? bookId {  get; set; }
        public string? Title { get; set; }
        public string? description { get; set; }
        public string? AuthorName { get; set; }
        public string? bookPic { get; set; } = null;
        public IFormFile? bookPhoto { get; set; }
        public string? publisherName { get; set; }
        public decimal? price { get; set; }
        public int? pageNumber { get; set; }
        public int? quantity { get; set; }
        public int? Stockquantity { get; set; }

        public List<Author>? Author { get; set; }
        public List<Category>? categories { get; set; }

        
    }
}
