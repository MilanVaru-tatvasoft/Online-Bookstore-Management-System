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
        public AdminDashboardModel getAdminDashboardData();
        public AdminBookListmodel getBookList(AdminBookListmodel model);
        public viewBookModel viewBookDetails(int bookId, int? userId);
        public bool isbookExist(string? bookTitle);
        public void addBook(viewBookModel model, int? userId);
        public viewBookModel getEditBook(int bookId);
        public void updateBook(viewBookModel model, int? userId);


    }
}
