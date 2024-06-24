using BusinessLogic.Interface;
using DataAccess.CustomModels;
using DataAccess.DataContext;
using DataAccess.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
        public AdminDashboardModel GetAdminDashboardData()
        {
            int? userid = _httpContext.HttpContext.Session.GetInt32("UserId");


            AdminDashboardModel model = new AdminDashboardModel();
            var orders = _context.Orders.Where(x => x.Isdeleted != true).ToList();
            var statuses = _context.Statuses.ToList();

            var ordersByStatus = statuses.Select(status => new
            {
                Status = status,
                OrderCount = orders.Count(order => order.Orderstatusid == status.StatusId)
            }).ToList();

            model.NewOrders = ordersByStatus.FirstOrDefault(x => x.Status.StatusId == 1)?.OrderCount ?? 0;
            model.ProcessingOrders = ordersByStatus.FirstOrDefault(x => x.Status.StatusId == 2)?.OrderCount ?? 0;
            model.ShippedOrders = ordersByStatus.FirstOrDefault(x => x.Status.StatusId == 3)?.OrderCount ?? 0;
            model.DeliveredOrders = ordersByStatus.FirstOrDefault(x => x.Status.StatusId == 4)?.OrderCount ?? 0;
            model.NumberOfBooks = _context.Books.Where(x => x.Isdeleted != true).Count();
            model.NumberOfCustomers = _context.Customers.Count();
            model.user = _context.Users.FirstOrDefault(x => x.Userid == userid);

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



            model.SaleOfThisMonth = sellofThisMonth;
            return model;

        }
        public OrderListModel getOrderListData()
        {
            OrderListModel orderList = new OrderListModel();

            var orderIds = _context.Orders
                                    .Where(x => x.Isdeleted != true)
                                    .Select(x => x.Orderid)
                                    .ToList();


            orderList.OrderDetails = _context.Orderdetails
                                                .Where(x => orderIds.Contains((int)x.Orderid))
                                                .ToList();
            orderList.Orders = _context.Orders
                                                .Where(x => orderIds.Contains((int)x.Orderid))
                                                .ToList();
            orderList.Books = _context.Books
                                                .ToList();
            orderList.Authors = _context.Authors
                                               .ToList();
            orderList.Categories = _context.Categories
                                               .ToList();
            orderList.Customers = _context.Customers
                                                .ToList();
            orderList.Statuses = _context.Statuses
                                               .ToList();

            return orderList;
        }

        public AdminBookListModel GetBookList(AdminBookListModel model)
        {

            model.Categories = _context.Categories.ToList();
            var booksQuery = _context.Books.Include(x => x.Author).Include(y => y.Category)
                                             .Where(x => x.Isdeleted != true).OrderBy(c => c.Bookid);

            var bookList = booksQuery.ToList();
            model.Authors = _context.Authors.ToList();

            if (model.Search1 != null)
            {
                bookList = bookList.Where(r => r.Title.Trim().ToLower().Contains(model.Search1.Trim().ToLower())).ToList();
            }

            if (model.Search2 != null && model.Search2.Count != 0)
            {
                bookList = bookList.Where(r => model.Search2.Contains((int)r.Authorid)).ToList();
            }

            if (model.Search3 != null && model.Search3.Count != 0)
            {
                bookList = bookList.Where(r => model.Search3.Contains((int)r.Categoryid)).ToList();
            }

            if (model.Search4 != null)
            {
                bookList = bookList.Where(r => r.Title.Trim().ToLower().Contains(model.Search4.Trim().ToLower())).ToList();
            }
            List<AdminBookList> AdminBookList = new List<AdminBookList>();
            foreach (var book in bookList)
            {
                AdminBookList item = new AdminBookList();

                item.BookId = book.Bookid;
                item.Title = book.Title;
                item.BookPhoto = book.Bookphoto;
                item.AuthorId = (int)book.Authorid;
                item.AuthorName = book.Author.Name;
                item.CategoryId = (int)book.Categoryid;
                item.CategoryName = book.Category.Categoryname;
                item.Price = book.Price;
                item.Stock = book.Stockquantity;

                AdminBookList.Add(item);

            }

            model.AdminBookLists = AdminBookList;
            return model;
        }

        public viewBookModel ViewBookDetails(int bookId, int? userId)
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
                publisherName = book.Publisher,
                bookPic = book.Bookphoto,


            };
            return model;
        }

        private int GetAutherId(string? AuthorName)
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
        private int GetCategoryId(string? categoryName)
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
        public bool IsBookExist(string? bookTitle)
        {
            return _context.Books.Any(i => i.Title == bookTitle && i.Isdeleted != true);
        }
        public viewBookModel GetAddBook()
        {
            viewBookModel model = new viewBookModel()
            {
                Author = _context.Authors.ToList(),
                categories = _context.Categories.ToList(),
            };
            return model;
        }
        public void AddBook(viewBookModel model, int? userId)
        {
            int authorId = GetAutherId(model.AuthorName);
            int categoryId = GetCategoryId(model.categoryName);

            Book book = new Book
            {
                Title = model.Title,
                Authorid = authorId,
                Bookphoto = model.bookPhoto.FileName,
                Publisher = model.publisherName,
                Categoryid = categoryId,
                Noofpages = model.pageNumber,
                Price = (decimal)model.price,
                Stockquantity = (int)model.Stockquantity,
                Createdby = userId,
                Createddate = DateTime.Now,
                Description = model.description
            };

            if (model.bookPhoto != null)
            {
                StoreFileAsync(model.bookPhoto, model.Title);
            }

            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void UpdateBook(viewBookModel model, int? userId)
        {
            Book book = _context.Books.FirstOrDefault(x => x.Bookid == model.bookId);

            int authorId = GetAutherId(model.AuthorName);
            int categoryId = GetCategoryId(model.categoryName);


            book.Title = model.Title;
            book.Authorid = authorId;
            book.Bookphoto = model.bookPhoto?.FileName ?? model.bookPic;
            book.Publisher = model.publisherName;
            book.Categoryid = categoryId;
            book.Noofpages = model.pageNumber;
            book.Price = (decimal)model.price;
            book.Stockquantity = (int)model.Stockquantity;
            book.Createdby = userId;
            book.Createddate = DateTime.Now;
            book.Description = model.description;

            if (model.bookPhoto != null)
            {
                StoreFileAsync(model.bookPhoto, book.Title);
            }

            _context.Books.Update(book);
            _context.SaveChanges();
        }

        public viewBookModel GetEditBook(int bookId)
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
                publisherName = book.Publisher,
                categoryName = _context.Categories?.FirstOrDefault(x => x.Categoryid == book.Categoryid).Categoryname,
                bookPic = book.Bookphoto,
                Stockquantity = book.Stockquantity,
                Author = _context.Authors.ToList(),
                categories = _context.Categories.ToList(),
            };
            return model;
        }

        public async Task StoreFileAsync(IFormFile file, string bookTitle)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is null or empty.");
            }

            string sanitizedTitle = string.Join("_", bookTitle.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

            string fileName = $"{sanitizedTitle}{Path.GetExtension(file.FileName)}";
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BookListCollections");
            string filePath = Path.Combine(directoryPath, fileName);

            Directory.CreateDirectory(directoryPath);

            if (File.Exists(filePath))
            {
                throw new IOException($"File {fileName} already exists.");
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

        }


        public bool GetDeleteBook(int bookId)
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

        public AdminProfileModel GetAdminProfile(int? uId)
        {
            User user = _context.Users.FirstOrDefault(x => x.Userid == uId);
            AdminProfileModel Profile = new AdminProfileModel()
            {
                UserId = user.Userid,
                FirstName = user.Firstname,
                LastName = user.Lastname,
                Email = user.Email,
                Contact = user.Phonenumber,
                Address = user.Address,
                BirthDate = user.Birthdate,
                City = user.City,
                Gender = user.Gender,
                UserPhotoName = user.Profilephoto,

            };
            return Profile;
        }
        public bool EditAdminProfile(AdminProfileModel profile)
        {
            User? user = _context.Users.FirstOrDefault(x => x.Userid == profile.UserId);
            if (user != null)
            {
                user.Firstname = profile.FirstName;
                user.Address = profile.Address;
                user.Lastname = profile.LastName;
                user.Email = profile.Email;
                user.Birthdate = profile.BirthDate;
                user.City = profile.City;
                user.Gender = profile.Gender;
                user.Phonenumber = profile.Contact;


                _context.Users.Update(user);
                _context.SaveChanges();
                if (profile.UserProfilePhoto != null)
                {
                    _authentication.StoreProfilePhoto(profile.UserProfilePhoto, profile.UserId);
                }

                Admin? admin = _context.Admins.FirstOrDefault(x => x.Email == user.Email);
                if (admin != null)
                {
                    admin.Firstname = profile.FirstName;
                    admin.Lastname = profile.LastName;
                    admin.Address = profile.Address;
                    admin.Email = profile.Email;
                    admin.City = profile.City;
                    admin.Gender = profile.Gender;
                    admin.Phonenumber = profile.Contact;
                    _context.Admins.Update(admin);
                    _context.SaveChanges();
                }

                return true;
            }
            return false;

        }

        public AuthorListModel GetAuthorList()
        {
            AuthorListModel model = new AuthorListModel()
            {
                Authors = _context.Authors.Where(x => x.Isdeleted != true).ToList(),
            };
            return model;
        }
        public CategoryListModel getCategoriesList()
        {
            CategoryListModel model = new CategoryListModel()
            {
                Categories = _context.Categories.Where(x => x.Isdeleted != true).ToList(),
            };
            return model;
        }

        public AuthorListModel GetEditAuthor(int AuthorId)
        {
            Author? Author = _context.Authors.FirstOrDefault(x => x.Authorid == AuthorId);
            AuthorListModel model = new AuthorListModel();
            if (Author != null && AuthorId != 0)
            {
                model.AuthorId = Author.Authorid;
                model.AuthorName = Author.Name;
                model.BirthDate = Author.Birthdate;
                model.Bio = Author.Bio;
            }
            else
            {
                model.AuthorId = 0;
            }

            return model;
        }

        public CategoryListModel GetAddCategory(int categoryId)
        {
            Category? category = _context.Categories.FirstOrDefault(x => x.Categoryid == categoryId);
            CategoryListModel model = new CategoryListModel();
            if (category != null && categoryId != 0)
            {
                model.CategoryId = category.Categoryid;
                model.CategoryName = category.Categoryname;
            }
            else
            {
                model.CategoryId = 0;
            }

            return model;
        }

        public bool GetDeleteAuthor(int AuthorId)
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

        public bool GetDeleteCategory(int categoryId)
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

        public bool AddOrUpdateAuthor(AuthorListModel model)
        {
            if (model.AuthorId != 0)
            {
                Author? Author = _context.Authors.FirstOrDefault(x => x.Authorid == model.AuthorId);
                Author.Name = model.AuthorName;
                Author.Bio = model.Bio;
                Author.Birthdate = model.BirthDate;
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
                        Birthdate = model.BirthDate,
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
            if (model.CategoryId != 0)
            {
                Category? ca = _context.Categories.FirstOrDefault(x => x.Categoryid == model.CategoryId);
                ca.Categoryname = model.CategoryName;
                ca.Isdeleted = false;
                _context.Categories.Update(ca);
                _context.SaveChanges();
                return true;
            }
            else
            {
                if (_context.Categories.Any(x => x.Categoryname.Trim() != model.CategoryName.Trim()))
                {
                    Category category = new Category()
                    {
                        Categoryname = model.CategoryName,
                        Isdeleted = false
                    };
                    _context.Categories.Add(category);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public AdminDashboardModel GetChartData(DateTime date)
        {
            AdminDashboardModel model = new AdminDashboardModel();

            var monthlyOrders = _context.Orders
                .Where(o => o.Orderdate.Year == date.Year && o.Orderdate.Month >= 1 && o.Orderdate.Month <= 12)
                .GroupBy(o => o.Orderdate.Month)
                .Select(g => new { Month = g.Key, TotalAmount = g.Sum(o => o.Totalamount) })
                .OrderBy(g => g.Month)
                .ToList();

            List<decimal> saleslist = new List<decimal>();
            for (int month = 1; month <= 12; month++)
            {
                decimal monthlySales = monthlyOrders.FirstOrDefault(x => x.Month == month)?.TotalAmount ?? 0;
                saleslist.Add(monthlySales);
            }

            var dailyOrders = _context.Orders
                .Where(o => o.Orderdate.Year == date.Year && o.Orderdate.Month == date.Month)
                .GroupBy(o => o.Orderdate.Day)
                .Select(g => new { Day = g.Key, TotalAmount = g.Sum(o => o.Totalamount) })
                .OrderBy(g => g.Day)
                .ToList();

            List<decimal> dailySalesList = new List<decimal>();
            for (int day = 1; day <= DateTime.DaysInMonth(date.Year, date.Month); day++)
            {
                decimal dailySales = dailyOrders.FirstOrDefault(x => x.Day == day)?.TotalAmount ?? 0;
                dailySalesList.Add(dailySales);
            }

            List<string> categories = _context.Categories
                .Where(x => x.Isdeleted != true)
                .OrderBy(x => x.Categoryid)
                .Select(i => i.Categoryname)
                .ToList();

            List<int> booksSoldPerCategory = categories
                .Select(category => _context.Orders
                    .Where(o => o.Orderdate.Year == date.Year && o.Orderdetails.Any(od => od.Book.Category.Categoryname == category))
                    .SelectMany(o => o.Orderdetails.Where(od => od.Book.Category.Categoryname == category))
                    .Sum(od => od.Quantity))
                .ToList();

            model.MonthlySales = saleslist;
            model.DailySales = dailySalesList;
            model.Categories = categories;
            model.NoOfBooks = booksSoldPerCategory;

            return model;
        }

        public bool GetAcceptorder(int orderId, int customerId)
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
                        string body = _authentication.OrderMailMessageBody(status);
                        string subject = "Order Acceptance";

                        _authentication.EmailSender(recipientEmail, subject, body);
                    }

                }
                return true;

            }
            else
            {
                return false;
            }
        }
        public bool GetShippedorder(int orderId, int customerId)
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
                        string body = _authentication.OrderMailMessageBody(status);
                        string subject = "Order Shipped";

                        _authentication.EmailSender(recipientEmail, subject, body);
                    }

                }
                return true;

            }
            else
            {
                return false;
            }
        }

        public bool GetDeliveredOrder(int orderId, int customerId)
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
                        string body = _authentication.OrderMailMessageBody(status);
                        string subject = "Order Delivered";

                        _authentication.EmailSender(recipientEmail, subject, body);
                    }

                }
                return true;

            }
            else
            {
                return false;
            }
        }

        public bool GetDeletedOrder(int orderId, int customerId)
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
                        string body = _authentication.OrderMailMessageBody(status);
                        string subject = "Order Deleted";

                        _authentication.EmailSender(recipientEmail, subject, body);
                    }

                }
                return true;

            }
            else
            {
                return false;
            }
        }

        public bool HandleOrderAction(int orderId, int customerId, int tempId)
        {
            if (orderId != 0 && customerId != 0)
            {
                var ord = _context.Orders.FirstOrDefault(x => x.Orderid == orderId);

                if (ord != null)
                {
                    switch (tempId)
                    {
                        case 1:
                            ord.Orderstatusid = 2;
                            break;
                        case 2:
                            ord.Orderstatusid = 3;
                            break;
                        case 3:
                            ord.Orderstatusid = 4;
                            break;
                        case 4:
                            ord.Isdeleted = true;
                            break;
                        default:
                            return false;
                    }

                    ord.Modifiedby = _httpContext.HttpContext.Session.GetInt32("UserId");
                    ord.Modifieddate = DateTime.Now;
                    _context.SaveChanges();

                    var customer = _context.Customers.FirstOrDefault(x => x.Customerid == customerId);
                    if (customer != null)
                    {
                        string recipientEmail = customer.Email;
                        string status = GetStatusMessage(tempId);
                        string body = _authentication.OrderMailMessageBody(status);
                        string subject = GetSubject(tempId);

                        _authentication.EmailSender(recipientEmail, subject, body);
                    }

                    return true;
                }
            }

            return false;
        }

        private string GetStatusMessage(int tempId)
        {
            switch (tempId)
            {
                case 1:
                    return "Your order has been accepted by the admin. It will be delivered to you very soon.";
                case 2:
                    return "Your order has been shipped by the admin. It will be delivered to you 4-5 days.";
                case 3:
                    return "Your order has been delivered to you. We hope you received your order!";
                case 4:
                    return "Your order has been deleted as per your request.";
                default:
                    return "";
            }
        }

        private string GetSubject(int tempId)
        {
            switch (tempId)
            {
                case 1:
                    return "Order Acceptance";
                case 2:
                    return "Order Shipped";
                case 3:
                    return "Order Delivered";
                case 4:
                    return "Order Deleted";
                default:
                    return "";
            }
        }


    }
}
