using DataAccess.DataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CustomModels
{
    public class viewBookModel
    {
        public List<Addtocart>? Addtocarts {  get; set; }  
        public int? cartId {  get; set; }
        [Required(ErrorMessage = "This is required")]

        public string? categoryName {  get; set; }
        public int? bookId {  get; set; }
        [Required (ErrorMessage ="This is required")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "This is required")]

        public string? description { get; set; }
        [Required(ErrorMessage = "This is required")]

        public string? AuthorName { get; set; }
        [Required(ErrorMessage = "This is required")]

        public string? bookPic { get; set; } = null;
        [Required(ErrorMessage = "This is required")]

        public IFormFile? bookPhoto { get; set; }
        [Required(ErrorMessage = "This is required")]

        public string? publisherName { get; set; }
        [Required(ErrorMessage = "This is required")]

        public decimal? price { get; set; }
        [Required(ErrorMessage = "This is required")]

        public int? pageNumber { get; set; }
        [Required(ErrorMessage = "This is required")]

        public int? quantity { get; set; }
        [Required(ErrorMessage = "This is required")]

        public int? Stockquantity { get; set; }
        public int? itemCount { get; set; }

        public decimal? ratingValue { get; set; }
        public string? reviewNote { get; set; }

        public List<Author>? Author { get; set; }
        public List<RatingReview>? reviews { get; set; }
        public List<Category>? categories { get; set; }
        public List<Customer>? customers { get; set; }

        
    }
}
