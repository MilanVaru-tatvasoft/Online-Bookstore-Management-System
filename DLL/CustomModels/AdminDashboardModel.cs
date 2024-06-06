using DataAccess.DataModels;
using System;
using System.Collections.Generic;
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
        public List<Author>? authors { get; set; }
        public List<Publisher>? publishers { get; set; }
        public List<Category>? categories { get; set; }
        public List<Customer>? customers { get; set; }
        public List<Status>? statuses { get; set; }
        public string? status { get; set; }
    }
    public class AdminBookListmodel
    {
        public List<Category> categories { get; set; }
        public List<Book> bookList { get; set; }
        public List<Author> authors { get; set; }
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
}
