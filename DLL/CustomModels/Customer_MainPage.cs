using DataAccess.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CustomModels
{
    public class Customer_MainPage
    {
        public List<Category> categories { get; set; }
        public List<Book> bookList { get; set; }
        public List<Author> Authors { get; set; }
        public List<Publisher> publishers { get; set; }
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

    public class UserProfile
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string? FirstName { get; set; }

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

    public class OrderData
    {
        public int? bookId { get; set; }
        public int? customerId { get; set; }
        public string? OrderAddress { get; set; }
        public string? updateCity { get; set; }
        public string? CustomerName { get; set; }
        public string? BookName { get; set; }
        public string? AuthorName { get; set; }
        public decimal? Price { get; set; }
        public string? Email { get; set; }
        public decimal? TotalAmount { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string? UpdateOrderAddress { get; set; }

        public string? city { get; set; }

        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? UpdateEmail { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string? UpdateCustomerName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be 10 digits")]
        public string? UpdatePhoneNumber { get; set; }

        public int? Quantity { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int? UpdateQuantity { get; set; }
    }

    public class ResetPasswordModel
    {
        public int? UserId { get; set; }
      
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirmation password is required")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        public string Password2 { get; set; }
    }

    public class CartListModel
    {
        public List<Addtocart>? addtocarts { get; set; }
    }
}
