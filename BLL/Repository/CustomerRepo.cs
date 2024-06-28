using BusinessLogic.Interface;
using DataAccess.CustomModels;
using DataAccess.DataContext;
using DataAccess.DataModels;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace BusinessLogic.Repository
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthentication _authentication;
        public CustomerRepo(ApplicationDbContext context, IAuthentication authentication)
        {
            _context = context;
            _authentication = authentication;
        }

        public CustomerMainPage GetCustomerDashboardData(CustomerMainPage model, int? userId, int pageNumber)
        {
            CustomerMainPage _MainPage = new CustomerMainPage();

            int? customerId = _context.Customers
                                     .FirstOrDefault(x => x.Userid == userId)
                                     ?.Customerid;

            var categories = _context.Categories.ToList();
            var authors = _context.Authors.ToList();

            var addtocarts = _context.Addtocarts
                                     .Where(x => x.Customerid == customerId && x.Isremoved != true && x.Checkout != true)
                                     .ToList();
            int pageSize = 6;

            var booksQuery = _context.Books.Include(x => x.Author).Include(y => y.Category)
                                   .Where(x => x.Isdeleted != true).OrderBy(c => c.Bookid);

            var booksList = booksQuery.ToList();

            _MainPage.Authors = authors;
            _MainPage.Categories = categories;
            _MainPage.UserId = userId;
            _MainPage.BookCount = booksList.Count();
            _MainPage.itemCount = addtocarts.Count();
            _MainPage.user = _context.Users.FirstOrDefault(x => x.Userid == userId);



            return _MainPage;
        }
        public List<DashboardList> GetCustomerDashBookList(CustomerMainPage model, int? userId, int pageNumber)
        {

            int pageSize = 6;
            int? customerId = _context.Customers
                                    .FirstOrDefault(x => x.Userid == userId)
                                    ?.Customerid;
            var booksQuery = _context.Books.Include(x => x.Author).Include(y => y.Category)
                                   .Where(x => x.Isdeleted != true).OrderBy(c => c.Bookid);

            var booksList = booksQuery.ToList();



            if (!string.IsNullOrEmpty(model.searchByBookName))
            {
                booksList = booksList.Where(r => r.Title.Trim().ToLower().Contains(model.searchByBookName.Trim().ToLower())).ToList();
            }

            if (model.filterAuthors != null && model.filterAuthors.Count != 0)
            {
                booksList = booksList.Where(r => model.filterAuthors.Contains((int)r.Authorid)).ToList();
            }

            if (model.filterCategory != null && model.filterCategory.Count != 0)
            {
                booksList = booksList.Where(r => model.filterCategory.Contains((int)r.Categoryid)).ToList();
            }
            if (!string.IsNullOrEmpty(model.searchByPublisher))
            {
                booksList = booksList.Where(r => r.Title.Trim().ToLower().Contains(model.searchByPublisher.Trim().ToLower())).ToList();
            }


            int bookCount = booksList.Count;
            model.BookCount = bookCount;
            int lastPageNumber = (int)Math.Ceiling((double)bookCount / pageSize);

            List<DashboardList> dashboardList = new List<DashboardList>();

            foreach (var book in booksList)
            {
                var reviews = _context.Ratingreviews.Where(x => x.BookId == book.Bookid).ToList();
                decimal i = 0;
                if (reviews.Count != 0) { i = (decimal)reviews.Average(x => x.Rating); }


                DashboardList item = new DashboardList();
                item.BookId = book.Bookid;
                item.Title = book.Title;
                item.BookPhoto = book.Bookphoto;
                item.AuthorId = (int)book.Authorid;
                item.AuthorName = book.Author.Name;
                item.CategoryId = (int)book.Categoryid;
                item.CategoryName = book.Category.Categoryname;
                item.AvgRating = i;
                item.discount = (decimal)book.Discount;

                item.Price = book.Price;

                dashboardList.Add(item);
            }

            if (pageNumber < 1 || pageNumber > lastPageNumber + 1)
            {
                model.BookCount = 0;
                return null;
            }


            int skipCount = (pageNumber - 1) * pageSize;
            dashboardList = dashboardList.Skip(skipCount).Take(pageSize).ToList();

            return dashboardList;

        }

        public int GetFilterBookCount(CustomerMainPage model)
        {
            var booksQuery = _context.Books.Include(x => x.Author).Include(y => y.Category)
                                  .Where(x => x.Isdeleted != true).OrderBy(c => c.Bookid);

            var booksList = booksQuery.ToList();



            if (!string.IsNullOrEmpty(model.searchByBookName))
            {
                booksList = booksList.Where(r => r.Title.Trim().ToLower().Contains(model.searchByBookName.Trim().ToLower())).ToList();
            }

            if (model.filterAuthors != null && model.filterAuthors.Count != 0)
            {
                booksList = booksList.Where(r => model.filterAuthors.Contains((int)r.Authorid)).ToList();
            }

            if (model.filterCategory != null && model.filterCategory.Count != 0)
            {
                booksList = booksList.Where(r => model.filterCategory.Contains((int)r.Categoryid)).ToList();
            }

            if (!string.IsNullOrEmpty(model.searchByPublisher))
            {
                booksList = booksList.Where(r => r.Title.Trim().ToLower().Contains(model.searchByPublisher.Trim().ToLower())).ToList();
            }
            return booksList.Count;
        }

        public Admin GetAdminData(string email)
        {
            Admin admin = _context.Admins.FirstOrDefault(x => x.Email == email);
            return admin;
        }
        public Customer GetCustomerData(string email)
        {
            Customer customer = _context.Customers.FirstOrDefault(x => x.Email == email);
            return customer;
        }
        public bool RegisterPost(UserProfile model)
        {
            if (_context.Users.Any(x => x.Email.ToLower() == model.Email.ToLower()))
            {
                return false;
            }
            else
            {
                User user = new User()
                {
                    Firstname = model.FirstName,
                    Lastname = model.LastName,
                    Phonenumber = model.Contact,
                    Roleid = 2,
                    Email = model.Email,
                    Birthdate = model.Birthdate,
                    Gender = model.Gender,
                    City = model.City,
                    Address = model.Address,
                    Passwordhash = model.Password,
                    IsDeleted = false,
                    Createddate = DateTime.Now,
                };
                _context.Users.Add(user);
                _context.SaveChanges();

                if (model.UserProfilePhoto != null)
                {
                    _authentication.StoreProfilePhoto(model.UserProfilePhoto, user.Userid);
                }
                _context.Users.Update(user);
                _context.SaveChanges();

                Customer customer = new Customer()
                {
                    Name = model.FirstName + " " + model.LastName,
                    Email = model.Email,
                    Passwordhash = model.Password,
                    Address = model.Address,
                    Phonenumber = model.Contact,
                    Createdby = 2,
                    Createddate = DateTime.Now,
                    Userid = user.Userid,
                };
                _context.Customers.Add(customer);
                _context.SaveChanges();

                return true;
            }
        }

        public UserProfile GetUserProfile(int? uId)
        {
            User user = _context.Users.FirstOrDefault(x => x.Userid == uId);
            UserProfile userProfile = new UserProfile()
            {
                UserId = user.Userid,
                FirstName = user.Firstname,
                LastName = user.Lastname,
                Email = user.Email,
                Contact = user.Phonenumber,
                Address = user.Address,
                Birthdate = user.Birthdate,
                City = user.City,
                Gender = user.Gender,
                UserPhotoName = user.Profilephoto

            };
            return userProfile;
        }
        public bool EditUserProfile(UserProfile profile)
        {
            User? user = _context.Users.FirstOrDefault(x => x.Userid == profile.UserId);
            if (user != null)
            {

                user.Firstname = profile.FirstName;
                user.Address = profile.Address;
                user.Lastname = profile.LastName;
                user.Email = profile.Email;
                user.Birthdate = profile.Birthdate;
                user.City = profile.City;
                user.Gender = profile.Gender;
                user.Phonenumber = profile.Contact;

                if (profile.UserProfilePhoto != null)
                {
                    _authentication.StoreProfilePhoto(profile.UserProfilePhoto, profile.UserId);
                }
                _context.Users.Update(user);
                _context.SaveChanges();

                Customer customer = _context.Customers.FirstOrDefault(x => x.Email == user.Email);
                if (customer != null)
                {
                    customer.Name = profile.FirstName + " " + profile.LastName;
                    customer.Address = profile.Address;
                    customer.Email = profile.Email;
                    customer.City = profile.City;
                    customer.Phonenumber = profile.Contact;
                    _context.Customers.Update(customer);
                    _context.SaveChanges();
                }

                return true;
            }
            return false;

        }

        public viewBookModel ViewBookDetails(int bookId, int? userId)
        {
            Customer? customer = _context.Customers.FirstOrDefault(x => x.Userid == userId);
            Book? book = _context.Books.FirstOrDefault(x => x.Bookid == bookId);
            Addtocart? cart = _context.Addtocarts?.FirstOrDefault(o => o.Customerid == customer.Customerid && o.Bookid == bookId && o.Isremoved != true && o.Checkout != true);
            viewBookModel model = new viewBookModel()
            {
                bookId = bookId,
                Title = book.Title,
                description = book.Description,
                pageNumber = book.Noofpages,
                price = book.Price,
                AuthorName = _context.Authors?.FirstOrDefault(x => x.Authorid == book.Authorid).Name,
                //publisherName = book.Publisher,
                bookPic = book.Bookphoto,
                Stockquantity = book.Stockquantity,
                quantity = 1,
                cartId = (int)(cart != null && cart?.Cartid != null ? cart.Cartid : 0),
                Addtocarts = _context.Addtocarts?.Where(x => x.Customerid == customer.Customerid).ToList(),
                itemCount = _context.Addtocarts?.Where(x => x.Customerid == customer.Customerid && x.Isremoved != true && x.Checkout != true).ToList().Count(),
                reviews = _context.Ratingreviews.Where(x => x.BookId == bookId).OrderByDescending(x => x.RatingDate).ToList(),
                customers = _context.Customers.ToList(),


            };
            return model;
        }
        public OrderData GetOrderDetails(int bookId, int? UserId)
        {
            Book? book = _context.Books.FirstOrDefault(x => x.Bookid == bookId);
            Customer? customer = _context.Customers.FirstOrDefault(x => x.Userid == UserId);

            OrderData orderdata = new OrderData()
            {
                BookId = bookId,
                BookName = book.Title,
                Price = book.Price,
                AuthorName = _context.Authors?.FirstOrDefault(i => i.Authorid == book.Authorid).Name,
                CustomerId = customer.Customerid,
                CustomerName = customer.Name,
                Email = customer.Email,
                PhoneNumber = customer.Phonenumber,
                OrderAddress = customer.Address,
                City = customer.City,
                Quantity = 1,
                TotalAmount = book.Price,
                UpdateCustomerName = customer.Name,
                UpdateEmail = customer.Email,
                UpdatePhoneNumber = customer.Phonenumber,
                UpdateOrderAddress = customer.Address,
                UpdateCity = customer.City,
                UpdateQuantity = 1,
                OrderDetails = _context.Orderdetails.ToList(),
                Orders = _context.Orders.Where(x => x.Isdeleted != true).ToList(),
            };

            return orderdata;
        }

        public void GetAddToCart(int bookId, int? userId, int cartId, int quantity)
        {
            int? custId = _context.Customers.FirstOrDefault(x => x.Userid == userId).Customerid;
            var cartData = _context.Addtocarts.FirstOrDefault(x => x.Cartid == cartId);
            Book? book = _context.Books.FirstOrDefault(x => x.Bookid == bookId);

            if (quantity == null && quantity == 0)
            {
                quantity = 1;
            }

            if (cartData == null)
            {
                decimal price = (decimal)(_context.Books?.FirstOrDefault(x => x.Bookid == bookId).Price);
                Addtocart addtocart = new Addtocart()
                {
                    Customerid = custId.Value,
                    Bookid = bookId,
                    Price = price,
                    Quantity = quantity,
                    Totalamount = quantity * price,
                    Isremoved = false,
                    CreatedDate = DateTime.Now,
                    Checkout = false,
                };
                _context.Addtocarts.Add(addtocart);
                _context.SaveChanges();
            }
            else
            {
                cartData.Quantity = quantity;
                cartData.Totalamount = (decimal)(quantity * cartData.Price);
                cartData.Isremoved = false;
                cartData.Checkout = false;
                cartData.UpdatedDate = DateTime.Now;
                _context.SaveChanges();
            }


            book.Stockquantity -= (int)quantity;
            _context.Update(book);
        }
        public void GetRemoveFromCart(int cartId, int? userId)
        {
            int? custId = _context.Customers.FirstOrDefault(x => x.Userid == userId).Customerid;
            var cartData = _context.Addtocarts.FirstOrDefault(x => x.Cartid == cartId);

            cartData.Isremoved = true;
            cartData.UpdatedDate = DateTime.Now;
            _context.SaveChanges();

            Book? book = _context.Books.FirstOrDefault(x => x.Bookid == cartData.Bookid);


            book.Stockquantity += (int)cartData.Quantity;
            _context.Update(book);

        }

        public int confirmOrder(OrderData data, int? userId)
        {
            int? custId = _context.Customers.FirstOrDefault(x => x.Userid == userId)?.Customerid;
            Book book = _context.Books.FirstOrDefault(x => x.Bookid == data.BookId);

            if (data.BookId != null && data.BookId != 0)
            {
                data.Quantity = data.Quantity ?? 0;
                decimal quantity = (decimal)data.Quantity;
                decimal totalAmount = (decimal)(data.Price * quantity);

                Order order = new Order()
                {
                    Customerid = custId,
                    Customername = data.CustomerName,
                    Email = data.Email,
                    Phonenumber = data.PhoneNumber,
                    Address = data.OrderAddress,
                    City = data.City,
                    Orderdate = DateTime.Today.Date,
                    Orderstatusid = 1,
                    Createdby = userId,
                    Createddate = DateTime.Now,
                    Isdeleted = false,
                    Totalamount = Math.Round((decimal)(book.Price * (1 - book.Discount / 100)), 2),
                };

                _context.Orders.Add(order);
                _context.SaveChanges(); // Save order

                Orderdetail orderdetail = new Orderdetail()
                {
                    Orderid = order.Orderid,
                    Bookid = data.BookId,
                    Price = book.Price,
                    Quantity = (int)data.Quantity,
                    Totalamount = Math.Round((decimal)(book.Price * (1 - book.Discount / 100)), 2),
                    Createdby = userId.ToString(),
                    Createddate = DateTime.Now
                };

                _context.Orderdetails.Add(orderdetail);

                book.Stockquantity -= (int)data.Quantity;
                _context.Update(book);

                _context.SaveChanges();

                data.Orders = _context.Orders.Where(x => x.Orderid == order.Orderid).ToList();

                return order.Orderid;
            }
            else
            {
                Order order = new Order()
                {
                    Customerid = custId,
                    Customername = data.CustomerName,
                    Email = data.Email,
                    Phonenumber = data.PhoneNumber,
                    Address = data.OrderAddress,
                    City = data.City,
                    Orderdate = DateTime.Today.Date,
                    Orderstatusid = 1,
                    Createdby = userId,
                    Createddate = DateTime.Now,
                    Isdeleted = false,
                    Totalamount = (decimal)data.GrossTotal,
                };

                _context.Orders.Add(order);
                _context.SaveChanges(); // Save order

                foreach (var cart in data.AddToCarts)
                {
                    Orderdetail orderdetail = new Orderdetail()
                    {
                        Orderid = order.Orderid,
                        Bookid = cart.Bookid,
                        Price = (decimal)cart.Price,
                        Quantity = (int)cart.Quantity,
                        Totalamount = Math.Round((decimal)(cart.Book.Price * (1 - cart.Book.Discount / 100)), 2),
                        Createdby = userId.ToString(),
                        Createddate = DateTime.Now
                    };

                    _context.Orderdetails.Add(orderdetail);
                }

                foreach (var cart2 in data.AddToCarts)
                {
                    book = _context.Books.FirstOrDefault(x => x.Bookid == cart2.Bookid);

                    if (book != null)
                    {
                        book.Stockquantity -= (int)cart2.Quantity;
                        _context.Update(book);
                    }

                    cart2.Checkout = true;
                    _context.Addtocarts.Update(cart2);
                    _context.SaveChanges();
                }

                _context.SaveChanges();

                return order.Orderid;
            }
        }

        public OrderData GetCartList(OrderData model, int? UserId)
        {
            var customerId = _context.Customers
     .Where(x => x.Userid == UserId)
     .Select(x => x.Customerid)
     .FirstOrDefault();

            var addtocart = _context.Addtocarts
                .Where(x => x.Customerid == customerId && x.Isremoved != true && x.Checkout != true)
                .Include(a => a.Book)
                .ToList();

            var totalBooks = addtocart.Sum(a => a.Quantity);
            var totalAmount = addtocart.Sum(a => a.Quantity * a.Book.Price);
            var totalAmountAfterDiscounts = addtocart.Sum(a => a.Quantity * (Math.Round((decimal)(a.Book.Price * (1 - a.Book.Discount / 100)))));
            var tax = 5m;
            var shippingAmount = 10m;

            var data = new OrderData
            {

                Categories = _context.Categories.ToList(),
                Authors = _context.Authors.ToList(),
                AddToCarts = addtocart,
                BookList = addtocart.Select(a => a.Book).ToList(),
                ItemCount = _context.Addtocarts.Where(x => x.Customerid == customerId && x.Isremoved != true && x.Checkout != true).Count(),
                TotalBooks = totalBooks,
                TotalAmount = totalAmount,
                TotalAmountAfterDiscounts = totalAmountAfterDiscounts,
                ShippingAmount = shippingAmount,
                Tax = tax,


            };

            if (totalBooks > 0)
            {
                var taxAmount = Math.Round((decimal)totalAmountAfterDiscounts * (tax / 100), 2);
                var grossTotal = Math.Round((decimal)(totalAmountAfterDiscounts + taxAmount + shippingAmount), 2);

                data.Tax = tax;
                data.GrossTotal = grossTotal;
            }
            else
            {
                data.TotalAmountAfterDiscounts = 0;
                data.Tax = 0;
                data.GrossTotal = 0;
                data.ShippingAmount = 0;
            }

            if (model.CustomerName != null)
            {
                data.CustomerName = model.CustomerName;
                data.OrderAddress = model.OrderAddress;
                data.Email = model.Email;
                data.PhoneNumber = model.PhoneNumber;
                data.City = model.City;
            }

            return data;

        }

        public void GetSubmitReviewAndRating(viewBookModel model, int? userId)
        {
            int customerId = _context.Customers.FirstOrDefault(x => x.Userid == userId)?.Customerid ?? 0;

            Ratingreview rate2 = _context.Ratingreviews.FirstOrDefault(x => x.BookId == model.bookId && x.CustomerId == customerId);
            if (rate2 != null)
            {
                rate2.Rating = model.ratingValue;
                rate2.Reviews = model.reviewNote;
                rate2.RatingDate = DateTime.Now;
                _context.Ratingreviews.Update(rate2);
                _context.SaveChanges();
            }
            else
            {
                Ratingreview rate = new Ratingreview()
                {
                    CustomerId = customerId,
                    BookId = (int)model.bookId,
                    Rating = model.ratingValue,
                    Reviews = model.reviewNote,
                    RatingDate = DateTime.Now,

                };
                _context.Ratingreviews.Add(rate);
                _context.SaveChanges();

            }
        }

        public bool GetPaymentDone(string paymentType, int OrderId, int? userId)
        {
            var customer = _context.Customers.FirstOrDefault(x => x.Userid == userId);
            var order = _context.Orders.FirstOrDefault(x => x.Orderid == OrderId);
            if (customer.Customerid != 0)
            {

                Payment payment = new Payment()
                {
                    CustomerId = customer.Customerid,
                    OrderId = (int)OrderId,
                    PaymentMethod = paymentType,
                    PaymentDate = DateTime.Now,
                    Amount = order.Totalamount,
                    PaymentStatus = "Payment Done",

                };
                _context.Payments.Add(payment);
                _context.SaveChanges(true);

                string recipientEmail = customer.Email;
                string status = "Your order has been  placed , ThankYou.";
                string body = _authentication.OrderMailMessageBody(status);
                string subject = "Order Placed";

                _authentication.EmailSender(recipientEmail, subject, body);

                return true;
            }
            return false;
        }

        public PaymentBillDetails getBillDetails(int orderId)
        {
            Order order = _context.Orders.FirstOrDefault(x => x.Orderid == orderId);
            List<Orderdetail> orderDetails = _context.Orderdetails.Where(x => x.Orderid == orderId).Include(a => a.Book).ToList();
            List<int?> bookIds = orderDetails.Select(od => od.Bookid).ToList();

            PaymentBillDetails bill = new PaymentBillDetails();
            var totalBooks = orderDetails.Sum(a => a.Quantity);
            var totalAmount = orderDetails.Sum(a => a.Quantity * a.Book.Price);
            var tax = 3m;
            var totalAmountAfterDiscount = orderDetails.Sum(a => a.Quantity * (Math.Round((decimal)(a.Book.Price * (1 - a.Book.Discount / 100)))));
            var shippingAmount = 10m;
            var taxAmount = totalAmount * (tax / 100);
            var grossTotal = Math.Round((decimal)(totalAmountAfterDiscount + taxAmount + shippingAmount), 2);

            if (order != null)
            {
                bill.CustomerName = order.Customername;
                bill.Orders = order;
                bill.Authors = _context.Authors.ToList();
                bill.OrderDetails = orderDetails.ToList();
                bill.Tax = tax;
                bill.TotalAfterDiscounts = totalAmountAfterDiscount;
                bill.GrossTotal = grossTotal;
                bill.TotalBooks = totalBooks;
                bill.ShippingAmount = shippingAmount;
                bill.BookList = _context.Books.Where(b => bookIds.Contains(b.Bookid)).ToList();
                bill.TotalAmount = totalAmount;
            }

            return bill;


        }

        public OrderData GetOrderHistoy(int? userId)
        {
            Customer? customer = _context.Customers.FirstOrDefault(x => x.Userid == userId);
            List<Order> order = _context.Orders.Where(x => x.Customerid == customer.Customerid && x.Isdeleted != true).OrderBy(r => r.Orderdate).ToList();
            List<Orderdetail> orderDetails = _context.Orderdetails.ToList();
            List<Book> books = _context.Books.ToList();
            List<Author> author = _context.Authors.ToList();
            List<Category> categories = _context.Categories.ToList();
            List<Payment> payments = _context.Payments.ToList();
            List<Status> statuses = _context.Statuses.ToList();

            OrderData orderData = new OrderData()
            {
                OrderDetails = orderDetails,
                Orders = order,
                Payments = payments,
                Categories = categories,
                BookList = books,
                Authors = author,
                CustomerId = customer.Customerid,
                Statuses = statuses,

            };

            return orderData;
        }

    }
}


