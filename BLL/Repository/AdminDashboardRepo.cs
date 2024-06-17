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
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BusinessLogic.Repository
{
    public class AdminDashboardRepo : IAdminDashboardRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthentication _authentication;
        private readonly IHttpContextAccessor _httpContext;

        public AdminDashboardRepo(ApplicationDbContext context, IAuthentication authentication, IHttpContextAccessor httpContext)
        {
            _context = context;
            _authentication = authentication;
            _httpContext = httpContext;
        }
        public AdminDashboardModel getAdminData()
        {
            AdminDashboardModel model = new AdminDashboardModel();
            var orders = _context.Orders.ToList();
            var statuses = _context.Statuses.ToList();

            var ordersByStatus = statuses.Select(status => new
            {
                Status = status,
                OrderCount = orders.Count(order => order.Orderstatusid == status.StatusId)
            }).ToList();

            model.newOrders = ordersByStatus.FirstOrDefault(x => x.Status.StatusId == 1)?.OrderCount ?? 0;
            model.ProcessingOrder = ordersByStatus.FirstOrDefault(x => x.Status.StatusId == 2)?.OrderCount ?? 0;
            model.shippedOrder = ordersByStatus.FirstOrDefault(x => x.Status.StatusId == 3)?.OrderCount ?? 0;
            model.deliveredOrders = ordersByStatus.FirstOrDefault(x => x.Status.StatusId == 4)?.OrderCount ?? 0;
            model.numberOfBooks = _context.Books.Where(x => x.Isdeleted != true).Count();
            model.numberOfCustomer = _context.Customers.Count();

            decimal sellofThisMonth = 0;

            var date = DateTime.Today;
            var monthlyOrders = _context.Orders
               .Where(o => o.Orderdate.Year == date.Year && o.Orderdate.Month == date.Month)
               .OrderBy(o => o.Orderid)
               .ToList();

            if (monthlyOrders != null && monthlyOrders.Count > 0)
            {
                sellofThisMonth = monthlyOrders.Sum(o => o.Totalamount);
            }



            model.sellofThisMonth = sellofThisMonth;
            return model;

        }
        public OrderListModel getOrderListData()
        {
            OrderListModel orderList = new OrderListModel();

            // Get the IDs of all orders that are not deleted
            var orderIds = _context.Orders
                                    .Where(x => x.Isdeleted != true)
                                    .Select(x => x.Orderid)
                                    .ToList();

            // Filter OrderDetails based on the list of order IDs
            orderList.orderdetails = _context.Orderdetails
                                                .Where(x => orderIds.Contains((int)x.Orderid))
                                                .ToList();
            orderList.orders = _context.Orders
                                                .Where(x => orderIds.Contains((int)x.Orderid))
                                                .ToList();
            orderList.books = _context.Books
                                                .ToList();
            orderList.Authors = _context.Authors
                                               .ToList();
            orderList.categories = _context.Categories
                                               .ToList();
            orderList.customers = _context.Customers
                                                .ToList();
            orderList.statuses = _context.Statuses
                                               .ToList();

            return orderList;
        }

        public AdminBookListmodel getBookList(AdminBookListmodel model)
        {

            model.categories = _context.Categories.ToList();
            List<Book> bookList = _context.Books.Where(x => x.Isdeleted != true).ToList();
            model.Authors = _context.Authors.ToList();
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
                AuthorName = _context.Authors?.FirstOrDefault(x => x.Authorid == book.Authorid).Name,
                publisherName = _context.Publishers?.FirstOrDefault(x => x.Publisherid == book.Publisherid).Name,
                bookPic = book.Bookphoto,


            };
            return model;
        }

        public int isAuthorExist(string? AuthorName)
        {
            int? AuthorId = _context.Authors.FirstOrDefault(x => x.Name == AuthorName && x.Isdeleted != true)?.Authorid;

            if (AuthorId == null)
            {
                Author Author = new Author() { Name = AuthorName };
                _context.Authors.Add(Author);
                _context.SaveChanges();
                AuthorId = Author.Authorid;
            }


            return (int)AuthorId;
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
            List<string> bookList = _context.Books.Where(i => i.Isdeleted != true).Select(x => x.Title).ToList();
            if (bookList.Contains(bookTitle)) { return true; }
            return false;
        }
        public viewBookModel getAddBook()
        {
            viewBookModel model = new viewBookModel()
            {
                Author = _context.Authors.ToList(),
                categories = _context.Categories.ToList(),
            };
            return model;
        }
        public void addBook(viewBookModel model, int? userId)
        {
            int? AuthorId = isAuthorExist(model.AuthorName);
            int? publisherId = ispublisherExist(model.publisherName);
            int? categoryId = isCategoryExist(model.categoryName);

            Book book = new Book();

            book.Title = model.Title;
            book.Authorid = AuthorId;
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

        public viewBookModel getEditBook(int bookId)
        {
            Book? book = _context.Books.FirstOrDefault(x => x.Bookid == bookId);
            viewBookModel model = new viewBookModel()
            {
                bookId = bookId,
                Title = book.Title,
                description = book.Description,
                pageNumber = book.Noofpages,
                price = book.Price,
                AuthorName = _context.Authors?.FirstOrDefault(x => x.Authorid == book.Authorid).Name,
                publisherName = _context.Publishers?.FirstOrDefault(x => x.Publisherid == book.Publisherid).Name,
                categoryName = _context.Categories?.FirstOrDefault(x => x.Categoryid == book.Categoryid).Categoryname,
                bookPic = book.Bookphoto,
                Stockquantity = book.Stockquantity,
                Author = _context.Authors.ToList(),
                categories = _context.Categories.ToList(),
            };
            return model;
        }
        public void updateBook(viewBookModel model, int? userId)
        {
            Book? book = _context.Books.FirstOrDefault(x => x.Bookid == model.bookId);

            int? AuthorId = isAuthorExist(model.AuthorName);
            int? publisherId = ispublisherExist(model.publisherName);
            int? categoryId = isCategoryExist(model.categoryName);


            book.Title = model.Title;
            book.Authorid = AuthorId;
            book.Bookphoto = model.bookPhoto?.FileName ?? model.bookPic;
            book.Publisherid = publisherId;
            book.Categoryid = categoryId;
            book.Noofpages = model.pageNumber;
            book.Price = (decimal)model.price;
            book.Stockquantity = (int)model.Stockquantity;
            book.Createdby = userId;
            book.Createddate = DateTime.Now;
            book.Description = model.description;
            if (model.bookPhoto != null) { storefile(model.bookPhoto); }
            _context.Books.Update(book);
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

        public bool getDeleteBook(int bookId)
        {
            if (bookId != 0)
            {
                Book? book = _context.Books.FirstOrDefault(x => x.Bookid == bookId);
                book.Isdeleted = true;
                _context.Books.Update(book);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public AdminProfileModel getAdminProfile(int? uId)
        {
            User user = _context.Users.FirstOrDefault(x => x.Userid == uId);
            AdminProfileModel Profile = new AdminProfileModel()
            {
                UserId = user.Userid,
                firstName = user.Firstname,
                lastName = user.Lastname,
                email = user.Email,
                contact = user.Phonenumber,
                address = user.Address,
                birthDate = user.Birthdate,
                city = user.City,
                gender = user.Gender,

            };
            return Profile;
        }
        public bool editAdminProfile(AdminProfileModel profile)
        {
            User? user = _context.Users.FirstOrDefault(x => x.Userid == profile.UserId);
            if (user != null)
            {
                user.Firstname = profile.firstName;
                user.Address = profile.address;
                user.Lastname = profile.lastName;
                user.Email = profile.email;
                user.Birthdate = profile.birthDate;
                user.City = profile.city;
                user.Gender = profile.gender;
                user.Phonenumber = profile.contact;
                _context.Users.Update(user);
                _context.SaveChanges();

                Admin? admin = _context.Admins.FirstOrDefault(x => x.Email == user.Email);
                if (admin != null)
                {
                    admin.Firstname = profile.firstName;
                    admin.Lastname = profile.lastName;
                    admin.Address = profile.address;
                    admin.Email = profile.email;
                    admin.City = profile.city;
                    admin.Gender = profile.gender;
                    admin.Phonenumber = profile.contact;
                    _context.Admins.Update(admin);
                    _context.SaveChanges();
                }

                return true;
            }
            return false;

        }

        public AuthorListmodel GetAuthorList()
        {
            AuthorListmodel model = new AuthorListmodel()
            {
                Authors = _context.Authors.Where(x => x.Isdeleted != true).ToList(),
            };
            return model;
        }
        public CategoryListModel getCategoriesList()
        {
            CategoryListModel model = new CategoryListModel()
            {
                categories = _context.Categories.Where(x => x.Isdeleted != true).ToList(),
            };
            return model;
        }

        public AuthorListmodel getEditAuthor(int AuthorId)
        {
            Author? Author = _context.Authors.FirstOrDefault(x => x.Authorid == AuthorId);
            AuthorListmodel model = new AuthorListmodel();
            if (Author != null && AuthorId != 0)
            {
                model.AuthorId = Author.Authorid;
                model.AuthorName = Author.Name;
                model.birthdate = Author.Birthdate;
                model.Bio = Author.Bio;
            }
            else
            {
                model.AuthorId = 0;
            }

            return model;
        }

        public CategoryListModel getAddCategory(int categoryId)
        {
            Category? category = _context.Categories.FirstOrDefault(x => x.Categoryid == categoryId);
            CategoryListModel model = new CategoryListModel();
            if (category != null && categoryId != 0)
            {
                model.categoryId = category.Categoryid;
                model.categoryName = category.Categoryname;
            }
            else
            {
                model.categoryId = 0;
            }

            return model;
        }

        public bool getDeleteAuthor(int AuthorId)
        {
            if (AuthorId != 0)
            {
                Author? Author = _context.Authors.FirstOrDefault(x => x.Authorid == AuthorId);
                Author.Isdeleted = true;
                _context.Authors.Update(Author);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool getDeleteCategory(int categoryId)
        {
            if (categoryId != 0)
            {
                Category? category = _context.Categories.FirstOrDefault(x => x.Categoryid == categoryId);
                category.Isdeleted = true;
                _context.Categories.Update(category);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool AddOrUpdateAuthor(AuthorListmodel model)
        {
            if (model.AuthorId != 0)
            {
                Author? Author = _context.Authors.FirstOrDefault(x => x.Authorid == model.AuthorId);
                Author.Name = model.AuthorName;
                Author.Bio = model.Bio;
                Author.Birthdate = model.birthdate;
                Author.Isdeleted = false;
                _context.Authors.Update(Author);
                _context.SaveChanges();
                return true;
            }
            else
            {
                if (_context.Authors.Any(x => x.Name.Trim() != model.AuthorName.Trim()))
                {
                    Author Author = new Author()
                    {
                        Name = model.AuthorName,
                        Bio = model.Bio,
                        Birthdate = model.birthdate,
                        Isdeleted = false
                    };
                    _context.Authors.Add(Author);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }


        }
        public bool AddOrUpdateCategory(CategoryListModel model)
        {
            if (model.categoryId != 0)
            {
                Category? ca = _context.Categories.FirstOrDefault(x => x.Categoryid == model.categoryId);
                ca.Categoryname = model.categoryName;
                ca.Isdeleted = false;
                _context.Categories.Update(ca);
                _context.SaveChanges();
                return true;
            }
            else
            {
                if (_context.Categories.Any(x => x.Categoryname.Trim() != model.categoryName.Trim()))
                {
                    Category category = new Category()
                    {
                        Categoryname = model.categoryName,
                        Isdeleted = false
                    };
                    _context.Categories.Add(category);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public AdminDashboardModel getmonthSales(int year)
        {
            AdminDashboardModel model = new AdminDashboardModel();
            List<decimal> saleslist = new List<decimal>();
            for (int month = 1; month <= 12; month++)
            {
                var monthlyOrders = _context.Orders
                    .Where(o => o.Orderdate.Year == year && o.Orderdate.Month == month)
                    .OrderBy(o => o.Orderid)
                    .ToList();

                decimal monthlySales = 0;

                if (monthlyOrders != null && monthlyOrders.Count > 0)
                {
                    monthlySales = monthlyOrders.Sum(o => o.Totalamount);
                }

                saleslist.Add(monthlySales);
            }

            model.monthlySales = saleslist;
            List<string> categories = _context.Categories
                .Where(x => x.Isdeleted != true)
                .OrderBy(x => x.Categoryid)
                .Select(i => i.Categoryname)
                .ToList();

            List<int> booksSoldPerCategory = new List<int>();

            foreach (var category in categories)
            {
                int booksSold = _context.Orders
                    .Where(o => o.Orderdate.Year == year && o.Orderdetails.Any(od => od.Book.Category.Categoryname == category))
                    .SelectMany(o => o.Orderdetails.Where(od => od.Book.Category.Categoryname == category))
                    .Sum(od => od.Quantity);

                booksSoldPerCategory.Add(booksSold);
            }


            model.categorties = categories;
            model.noofbooks = booksSoldPerCategory;

            return model;
        }

     

        public bool getAcceptorder(int orderId, int customerId)
        {
            if (orderId != 0 && customerId != 0)
            {
                var ord = _context.Orders.FirstOrDefault(x => x.Orderid == orderId);

                if (ord != null)
                {
                    ord.Orderstatusid = 2;
                    ord.Modifiedby = _httpContext.HttpContext.Session.GetInt32("UserId");
                    ord.Modifieddate = DateTime.Now;
                    _context.SaveChanges();

                    var customer = _context.Customers.FirstOrDefault(x => x.Customerid == customerId);
                    if (customer != null)
                    {
                        string recipientEmail = customer.Email;
                        string status = "Your order has been accepted by the admin. It will be delivered to you very soon.";
                        string body = _authentication.ordermessage(status);
                        string subject = "Order Acceptance";

                        _authentication.emailSender(recipientEmail, subject, body);
                    }

                }
                return true;

            }
            else
            {
                return false;
            }
        }
        public bool getShippedorder(int orderId, int customerId)
        {
            if (orderId != 0 && customerId != 0)
            {
                var ord = _context.Orders.FirstOrDefault(x => x.Orderid == orderId);

                if (ord != null)
                {
                    ord.Orderstatusid = 3;
                    ord.Modifiedby = _httpContext.HttpContext.Session.GetInt32("UserId");
                    ord.Modifieddate = DateTime.Now;
                    _context.SaveChanges();

                    var customer = _context.Customers.FirstOrDefault(x => x.Customerid == customerId);
                    if (customer != null)
                    {
                        string recipientEmail = customer.Email;
                        string status = "Your order has been shipped by the admin. It will be delivered to you 4-5 days.";
                        string body = _authentication.ordermessage(status);
                        string subject = "Order Shipped";

                        _authentication.emailSender(recipientEmail, subject, body);
                    }

                }
                return true;

            }
            else
            {
                return false;
            }
        }

        public bool getDeliveredOrder(int orderId, int customerId)
        {
            if (orderId != 0 && customerId != 0)
            {
                var ord = _context.Orders.FirstOrDefault(x => x.Orderid == orderId);

                if (ord != null)
                {
                    ord.Orderstatusid = 4;
                    ord.Modifiedby = _httpContext.HttpContext.Session.GetInt32("UserId");
                    ord.Modifieddate = DateTime.Now;
                    _context.SaveChanges();

                    var customer = _context.Customers.FirstOrDefault(x => x.Customerid == customerId);
                    if (customer != null)
                    {
                        string recipientEmail = customer.Email;
                        string status = "Your order has been  delivered to you. we hope You received Order!";
                        string body = _authentication.ordermessage(status);
                        string subject = "Order Delivered";

                        _authentication.emailSender(recipientEmail, subject, body);
                    }

                }
                return true;

            }
            else
            {
                return false;
            }
        }

        public bool getDeletedOrder(int orderId, int customerId)
        {
            if (orderId != 0 && customerId != 0)
            {
                var ord = _context.Orders.FirstOrDefault(x => x.Orderid == orderId);

                if (ord != null)
                {
                    ord.Isdeleted = true;
                    ord.Modifiedby = _httpContext.HttpContext.Session.GetInt32("UserId");
                    ord.Modifieddate = DateTime.Now;
                    _context.SaveChanges();

                    var customer = _context.Customers.FirstOrDefault(x => x.Customerid == customerId);
                    if (customer != null)
                    {
                        string recipientEmail = customer.Email;
                        string status = "Your order has been  Deleted as per your request!";
                        string body = _authentication.ordermessage(status);
                        string subject = "Order Deleted";

                        _authentication.emailSender(recipientEmail, subject, body);
                    }

                }
                return true;

            }
            else
            {
                return false;
            }
        }


    }
}
