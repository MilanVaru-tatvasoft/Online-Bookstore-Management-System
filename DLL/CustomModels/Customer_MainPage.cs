using DataAccess.DataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.CustomModels
{
    public class CustomerMainPage
    {
        public List<Category> Categories { get; set; }
        public List<Book> BookList { get; set; }
        public List<Author> Authors { get; set; }
        public List<Publisher> Publishers { get; set; }
        public List<Addtocart>? AddToCarts { get; set; }
        public List<Ratingreview>? Reviews { get; set; }
        public List<DashboardList>? DashboardLists { get; set; }
        public User? user { get; set; }
        public int? itemCount { get; set; }
        public int? UserId { get; set; }
        public int? BookCount { get; set; }
        public int PageNumber { get; set; }
        public string? searchByBookName { get; set; }
        public List<int>? filterAuthors { get; set; }
        public List<int>? filterCategory { get; set; }
        public string? searchByPublisher { get; set; }
    }

    public class DashboardList
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public string BookPhoto { get; set; }
        public decimal Price { get; set; }
        public decimal discount { get; set; }
        public decimal AvgRating { get; set; }
        public bool IsFavorite { get; set; }


    }

    public class UserProfile
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; }

        public string? Password { get; set; }
        public string? Password2 { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "this field is required")]

        public string? Gender { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [RegularExpression(@"^[a-z0-9._%+-]+@[a-z]{3,}\.[a-z]{2,}$", ErrorMessage = "Invalid email address format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Phone number must be exactly 10 digits")]
        public string? Contact { get; set; }

        [Required(ErrorMessage = "Birthdate is required")]
        public DateTime? Birthdate { get; set; }

        public string? Role { get; set; }
        public string? UserPhotoName { get; set; }
        [Required(ErrorMessage = "This is required")]

        public IFormFile? UserProfilePhoto { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string? City { get; set; }
    }

    public class OrderData
    {
        public List<Order> Orders { get; set; }
        public List<Orderdetail> OrderDetails { get; set; }
        public List<Payment> Payments { get; set; }
        public List<Addtocart>? AddToCarts { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Book>? BookList { get; set; }
        public List<Author>? Authors { get; set; }
        public List<Publisher>? Publishers { get; set; }
        public List<Status>? Statuses { get; set; }

        public int? ItemCount { get; set; }
        public decimal? TotalBooks { get; set; }
        public decimal? TotalAmountAfterDiscounts { get; set; }
        public decimal? ShippingAmount { get; set; }
        public decimal? GrossTotal { get; set; }
        public decimal? Tax { get; set; }
        public int? BookId { get; set; }
        public int? CustomerId { get; set; }
        public int? OrderId { get; set; }

        [Required(ErrorMessage = "Order address is required")]
        public string? OrderAddress { get; set; }

        [Required(ErrorMessage = "Update city is required")]
        public string? UpdateCity { get; set; }

        [Required(ErrorMessage = "Customer name is required")]
        public string? CustomerName { get; set; }

        public string? BookName { get; set; }
        public string? AuthorName { get; set; }
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [RegularExpression(@"^[a-z0-9._%+-]+@[a-z]{3,}\.[a-z]{2,}$", ErrorMessage = "Invalid email address format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Total amount is required")]
        public decimal? TotalAmount { get; set; }

        [Required(ErrorMessage = "Update order address is required")]
        public string? UpdateOrderAddress { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string? City { get; set; }

        [Required(ErrorMessage = "Zip code is required")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Zip code must be exactly 6 digits")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Zip code must only contain digits")]
        public string? ZipCode { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(10, ErrorMessage = "Phone number must be exactly 10 digits", MinimumLength = 10)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number must only contain digits")]
        public string UpdatePhoneNumber { get; set; }
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Update email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? UpdateEmail { get; set; }

        [Required(ErrorMessage = "Update customer name is required")]
        public string? UpdateCustomerName { get; set; }

        [Required(ErrorMessage = "Payment type is required")]
        public string? PaymentType { get; set; }

        public int? Quantity { get; set; }

        [Required(ErrorMessage = "Update quantity is required")]
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

    public class PaymentBillDetails
    {
        public string? CustomerName { get; set; }
        public int? OrderId { get; set; }
        public Order? Orders { get; set; }
        public List<Orderdetail> OrderDetails { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Book>? BookList { get; set; }
        public List<Author>? Authors { get; set; }
        public List<Publisher>? Publishers { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalBooks { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal TotalAfterDiscounts { get; set; }
        public decimal? ShippingAmount { get; set; }
        public decimal? GrossTotal { get; set; }
        public decimal? Tax { get; set; }
    }


    public class FavoriteModel
    {
        public int FavoriteId { get; set; }

        public int CustomerId { get; set; }
        public int BookCount { get; set; }

        public int BookId { get; set; }

        public List<DashboardList> FavBookList { get; set; }
    }


}