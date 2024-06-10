using DataAccess.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CustomModels
{
    public class AdminDashboardModel
    {
        public List<Order>? orders { get; set; }
        public List<Orderdetail>? orderdetails { get; set; }
        public List<Book>? books { get; set; }
        public List<Author>? Authors { get; set; }
        public List<Publisher>? publishers { get; set; }
        public List<Category>? categories { get; set; }
        public List<Customer>? customers { get; set; }
        public List<Status>? statuses { get; set; }
        public string? status { get; set; }
    }
    public class AdminBookListmodel
    {
        public List<Category>? categories { get; set; }
        public List<Book>? bookList { get; set; }
        public List<Author>? Authors { get; set; }
        public List<Publisher>? publishers { get; set; }
        public int UserId { get; set; }

        public string? search1 { get; set; }
        public List<int>? search2 { get; set; }
        public List<int>? search3 { get; set; }
        public List<int> search4 { get; set; }

        // Pagination properties
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        
    }
    public class AdminProfileModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string? FirstName { get; set; }
        public string? Password { get; set; }
        public string? Password2 { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string? LastName { get; set; }
        public string? gender { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [EmailAddress(ErrorMessage = "enter valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.PhoneNumber)]
        public string? Contact { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public DateTime? birthdate { get; set; }

        public string? role { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string? city { get; set; }
    }

    public class AuthorListmodel
    {
        public List<Author>? Authors { get; set; }
        public int AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public string? Bio { get; set; }
        public DateTime? birthdate { get; set; }
        public bool? isdeleted { get; set; }
    }
    public class CategoryListModel
    {
        public List<Category>? categories { get; set; }
        public int categoryId { get; set; }
        public string? categoryName { get; set; }
      
        public bool? isdeleted { get; set; }
    }
}
