using BusinessLogic.Interface;
using DataAccess.CustomModels;
using DataAccess.DataContext;
using DataAccess.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BusinessLogic.Repository
{
    public class AdminDashboardRepo : IAdminDashboardRepo
    {
        private readonly ApplicationDbContext _context;
        public AdminDashboardRepo(ApplicationDbContext context)
        {
            _context = context;

        }
        public AdminDashboardModel getAdminDashboardData()
        {
            AdminDashboardModel adminDashboard = new AdminDashboardModel();

            // Get the IDs of all orders that are not deleted
            var orderIds = _context.Orders
                                    .Where(x => x.Isdeleted != true)
                                    .Select(x => x.Orderid)
                                    .ToList();

            // Filter OrderDetails based on the list of order IDs
            adminDashboard.orderdetails = _context.Orderdetails
                                                .Where(x => orderIds.Contains((int)x.Orderid))
                                                .ToList();
            adminDashboard.orders = _context.Orders
                                                .Where(x => orderIds.Contains((int)x.Orderid))
                                                .ToList();
            adminDashboard.books = _context.Books
                                                .ToList();
            adminDashboard.authors = _context.Authors
                                               .ToList();
            adminDashboard.categories = _context.Categories
                                               .ToList();
            adminDashboard.customers = _context.Customers
                                                .ToList();
            adminDashboard.statuses = _context.Statuses
                                               .ToList();

            return adminDashboard;
        }

        public AdminBookListmodel getBookList(AdminBookListmodel model)
        {

            model.categories = _context.Categories.ToList();
            List<Book> bookList = _context.Books.ToList();
            model.authors = _context.Authors.ToList();
            model.publishers = _context.Publishers.ToList();

            if (model.search1 != null)
            {
                bookList = bookList.Where(r => r.Title.Trim().ToLower().Contains(model.search1.Trim().ToLower())).ToList();
            }

            if (model.search2 != null && model.search2.Count != 0)
            {
                bookList = bookList.Where(r => model.search2.Contains((int)r.Authorid)).ToList();
            }

            if (model.search3 != null && model.search3.Count != 0)
            {
                bookList = bookList.Where(r => model.search3.Contains((int)r.Categoryid)).ToList();
            }

            if (model.search4 != null && model.search4.Count != 0)
            {
                bookList = bookList.Where(r => model.search4.Contains((int)r.Publisherid)).ToList();
            }


            //int page = 1;
            // int pageSize = 10;


            //int totalItems = bookList.Count();
            //var books = bookList.Skip((page - 1) * model.PageSize).Take(pageSize).ToList();

            //model.CurrentPage = page;
            //model.PageSize = pageSize;
            //model.TotalItems = totalItems;
            //model.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            model.bookList = bookList;
            return model;
        }

        public viewBookModel viewBookDetails(int bookId, int? userId)
        {
            Customer? customer = _context.Customers.FirstOrDefault(x => x.Userid == userId);
            Book? book = _context.Books.FirstOrDefault(x => x.Bookid == bookId);
            viewBookModel model = new viewBookModel()
            {
                bookId = bookId,
                Title = book.Title,
                description = book.Description,
                pageNumber = book.Noofpages,
                price = book.Price,
                authorName = _context.Authors?.FirstOrDefault(x => x.Authorid == book.Authorid).Name,
                publisherName = _context.Publishers?.FirstOrDefault(x => x.Publisherid == book.Publisherid).Name,
                bookPic = book.Bookphoto,


            };
            return model;
        }

        public int isautherExist(string? autherName)
        {
            int? autherId = _context.Authors.FirstOrDefault(x => x.Name == autherName)?.Authorid;

            if (autherId == null)
            {
                Author author = new Author() { Name = autherName };
                _context.Authors.Add(author);
                _context.SaveChanges();
                autherId = author.Authorid;
            }


            return (int)autherId;
        }
        public int ispublisherExist(string? publisherName)
        {
            int? publisherId = _context.Publishers.FirstOrDefault(x => x.Name == publisherName)?.Publisherid;

            if (publisherId == null)
            {
                DataAccess.DataModels.Publisher publisher = new DataAccess.DataModels.Publisher() { Name = publisherName };
                _context.Publishers.Add(publisher);
                _context.SaveChanges();
                publisherId = publisher.Publisherid;
            }


            return (int)publisherId;
        }
        public int isCategoryExist(string? categoryName)
        {

            int? catId = _context.Categories.FirstOrDefault(x => x.Categoryname == categoryName)?.Categoryid;
            if (catId == null)
            {
                Category c = new Category() { Categoryname = categoryName };
                _context.Categories.Add(c);
                _context.SaveChanges();
                catId = c.Categoryid;
            }
            return (int)catId;
        }
        public bool isbookExist(string? bookTitle)
        {
            List<string> bookList = _context.Books.Select(x => x.Title).ToList();
            if (bookList.Contains(bookTitle)) { return true; }
            return false;
        }

        public void addBook(viewBookModel model,int? userId)
        {
            int? autherId = isautherExist(model.authorName);
            int? publisherId = ispublisherExist(model.publisherName);
            int? categoryId = isCategoryExist(model.categoryName);

            Book book = new Book();

            book.Title = model.Title;
            book.Authorid = autherId;
            book.Bookphoto = model.bookPhoto.FileName;
            book.Publisherid = publisherId;
            book.Categoryid = categoryId;
            book.Noofpages = model.pageNumber;
            book.Price = (decimal)model.price;
            book.Stockquantity = (int)model.Stockquantity;
            book.Createdby = userId;
            book.Createddate = DateTime.Now;
            book.Description = model.description;
            if (model.bookPhoto != null) { storefile(model.bookPhoto); } 
            _context.Books.Add(book);
            _context.SaveChanges();

        }
        public void storefile(IFormFile fileName)
        {
            string FileName = fileName.FileName;
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BookListCollections", FileName);

            if (!File.Exists(FilePath))
            {

                IFormFile DocFile = fileName;
                FileStream stream = new FileStream(FilePath, FileMode.Create);

                DocFile.CopyTo(stream);
                stream.Close();
            }

        }
    }
}
