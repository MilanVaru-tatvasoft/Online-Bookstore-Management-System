using DataAccess.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public interface IAdminDashboardRepo
    {
        public OrderListModel getOrderListData();
        public AdminDashboardModel GetAdminDashboardData();

        public AdminBookListModel GetBookList(AdminBookListModel model);
        public viewBookModel ViewBookDetails(int bookId, int? userId);
        public bool IsBookExist(string? bookTitle);
        public viewBookModel GetAddBook();
        public void AddBook(viewBookModel model, int? userId);
        public viewBookModel GetEditBook(int bookId);
        public void UpdateBook(viewBookModel model, int? userId);
        public bool EditAdminProfile(AdminProfileModel profile);
        public AdminProfileModel GetAdminProfile(int? uId);
        public bool GetDeleteBook(int bookId);
        public CategoryListModel getCategoriesList();
        public AuthorListModel GetAuthorList();
        public AuthorListModel GetEditAuthor(int AuthorId);
        public CategoryListModel GetAddCategory(int categtoryId);
        public bool GetDeleteCategory(int categoryId);
        public bool GetDeleteAuthor(int AuthorId);
        public bool AddOrUpdateAuthor(AuthorListModel model);
        public bool AddOrUpdateCategory(CategoryListModel model);

        public AdminDashboardModel GetChartData(DateTime date);
         public bool GetAcceptorder(int orderId,  int customerId);
         public bool GetShippedorder(int orderId,  int customerId);
         public bool GetDeliveredOrder(int orderId,  int customerId);
        public bool GetDeletedOrder(int orderId, int customerId);
        public bool HandleOrderAction(int orderId, int customerId, int tempId);


    }
}
