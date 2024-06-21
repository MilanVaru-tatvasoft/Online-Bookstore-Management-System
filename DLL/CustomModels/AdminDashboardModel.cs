using DataAccess.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CustomModels
{
    public class AdminDashboardModel
    {
        public List<decimal> monthlySales { get; set; } 
        public List<decimal> dailySales { get; set; } 
        public List<string> categorties { get; set; } 
        public List<int> noofbooks { get; set; }
        
        public int? newOrders { get; set; }
        public int? ProcessingOrder { get; set; }
        public int? shippedOrder { get; set; }
        public int? deliveredOrders { get; set; }
        public int? numberOfBooks { get; set; }
        public int? totalSalelBooks { get; set; }
        public int? numberOfCustomer { get; set; }
        public decimal? salelofThisMonth { get; set; }
    }
    public class OrderListModel
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
        public List<Author>? Authors { get; set; }
        public List<Publisher>? publishers { get; set; }
        public List<Category>? categories { get; set; }
        public string? search1 { get; set; }
        public List<int>? search2 { get; set; }
        public List<int>? search3 { get; set; }
        public string? search4 { get; set; } 
        public List<AdminBookList> adminBookLists { get; set; }
    }
    public class AdminBookList
    {
            public int bookId { get; set; }
            public string Title { get; set; }
            public string AuthorName { get; set; }
            public string CategoryName { get; set; }
            public int CategoryId { get; set; }
            public int authorId { get; set; }
            public int stock { get; set; }
            public string BookPhoto { get; set; }
            public decimal Price { get; set; }
    }
    public class AdminProfileModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string? firstName { get; set; }
        public string? password { get; set; }
        public string? confirmPassword { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string? lastName { get; set; }
        public string? gender { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [EmailAddress(ErrorMessage = "enter valid email")]
        public string email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.PhoneNumber)]
        public string? contact { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public DateTime? birthDate { get; set; }

        public string? role { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string? address { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string? city { get; set; }
    }

    public class AuthorListmodel
    {
        public List<Author>? Authors { get; set; }
        public int AuthorId { get; set; }
        [Required(ErrorMessage = "This field is required")]

        public string? AuthorName { get; set; }
        [Required(ErrorMessage = "This field is required")]

        public string? Bio { get; set; }
        [Required(ErrorMessage = "This field is required")]

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

    [DataContract]
    public class DataPoint
    {
        public DataPoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "x")]
        public Nullable<double> X = null;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<double> Y = null;
    }
}
