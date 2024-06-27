using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using DataAccess.DataModels;
using Microsoft.AspNetCore.Http;

namespace DataAccess.CustomModels
{
    public class AdminDashboardModel
    {
        public List<decimal> MonthlySales { get; set; }
        public List<decimal> DailySales { get; set; }
        public List<string> Categories { get; set; }
        public List<int> NoOfBooks { get; set; }
        public User? user { get; set; }  
        public int? NewOrders { get; set; }
        public int? ProcessingOrders { get; set; }
        public int? ShippedOrders { get; set; }
        public int? DeliveredOrders { get; set; }
        public int? NumberOfBooks { get; set; }
        public int? TotalSaleBooks { get; set; }
        public int? NumberOfCustomers { get; set; }
        public decimal? SaleOfThisMonth { get; set; }
    }

    public class OrderListModel
    {
        public List<Order>? Orders { get; set; }
        public List<Orderdetail>? OrderDetails { get; set; }
        public List<Book>? Books { get; set; }
        public List<Author>? Authors { get; set; }
        public List<Publisher>? Publishers { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Customer>? Customers { get; set; }
        public List<Status>? Statuses { get; set; }
        public string? Status { get; set; }
    }

    public class AdminBookListModel
    {
        public List<Author>? Authors { get; set; }
        public List<Publisher>? Publishers { get; set; }
        public List<Category>? Categories { get; set; }
        public string? searchByBookName { get; set; }
        public List<int>? filterAuthors { get; set; }
        public List<int>? filterCategory { get; set; }
        public string? searchByPublisher { get; set; }
        public List<AdminBookList> AdminBookLists { get; set; }
    }

    public class AdminBookList
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public int Stock { get; set; }
        public string BookPhoto { get; set; }
        public decimal Price { get; set; }
    }

    public class AdminProfileModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string? FirstName { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string? LastName { get; set; }
        public string? Gender { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^[a-z0-9._%+-]+@[a-z]{3,}\.[a-z]{2,}$", ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.PhoneNumber)]
        public string? Contact { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public DateTime? BirthDate { get; set; }

        public string? Role { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string? City { get; set; }
        public string? UserPhotoName { get; set; }
        [Required(ErrorMessage ="This is required")]
        public IFormFile? UserProfilePhoto { get; set; }
    }

    public class AuthorListModel
    {
        public List<Author>? Authors { get; set; }
        public int? AuthorId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string? AuthorName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string? Bio { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public DateTime? BirthDate { get; set; }
        public bool? IsDeleted { get; set; }
    }

    public class CategoryListModel
    {
        public List<Category>? Categories { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public bool? IsDeleted { get; set; }
    }

    public class UserListModel
    {
        public List<UserList>? UsersList { get; set; }

    }

    public class UserList
    {
        public int? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public bool? IsDeleted { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Role { get; set; }
        public string? Gender { get; set; }
    }
}
