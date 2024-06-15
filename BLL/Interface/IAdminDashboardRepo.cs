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
        public AdminDashboardModel getAdminData();

        public AdminBookListmodel getBookList(AdminBookListmodel model);
        public viewBookModel viewBookDetails(int bookId, int? userId);
        public bool isbookExist(string? bookTitle);
        public viewBookModel getAddBook();
        public void addBook(viewBookModel model, int? userId);
        public viewBookModel getEditBook(int bookId);
        public void updateBook(viewBookModel model, int? userId);
        public bool editAdminProfile(AdminProfileModel profile);
        public AdminProfileModel getAdminProfile(int? uId);
        public bool getDeleteBook(int bookId);
        public CategoryListModel getCategoriesList();
        public AuthorListmodel GetAuthorList();
        public AuthorListmodel getEditAuthor(int AuthorId);
        public CategoryListModel getAddCategory(int categtoryId);
        public bool getDeleteCategory(int categoryId);
        public bool getDeleteAuthor(int AuthorId);
        public bool AddOrUpdateAuthor(AuthorListmodel model);
        public bool AddOrUpdateCategory(CategoryListModel model);

        public AdminDashboardModel getmonthSales(int year);
         public bool getAcceptorder(int orderId,  int customerId);
         public bool getShippedorder(int orderId,  int customerId);
         public bool getDeliveredOrder(int orderId,  int customerId);
        public bool getDeletedOrder(int orderId, int customerId);

    }
}
