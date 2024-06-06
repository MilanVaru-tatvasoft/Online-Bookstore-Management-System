using BusinessLogic.Interface;
using DataAccess.CustomModels;
using DataAccess.DataContext;
using DataAccess.DataModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.OutputCaching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BusinessLogic.Repository
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly ApplicationDbContext _context;
        public CustomerRepo(ApplicationDbContext context)
        {
            _context = context;

        }

        public Customer_MainPage getdata(Customer_MainPage model)
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
        public bool validateLogin(string email, string password)
        {
            var userData = _context.Users.FirstOrDefault(u => u.Email == email);
            if (userData != null)
            {
                if (userData.Passwordhash == password)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;

        }
        public bool resetPassword(ResetPasswordModel model)
        {
            User user = _context.Users.FirstOrDefault(x => x.Email == model.Email);
            if (user != null)
            {
                user.Passwordhash = model.Password;
                _context.SaveChanges();

                Customer customer = _context.Customers.FirstOrDefault(x => x.Userid == user.Userid);
                if (customer != null)
                {
                    customer.Passwordhash = model.Password;
                    _context.SaveChanges();
                    
                }

                return true;
            }
            return false;
        }
        public Admin getAdminData(string email)
        {
            Admin admin = _context.Admins.FirstOrDefault(x=>x.Email == email);
            return admin;
        } 
        public Customer getCustomerData(string email)
        {
            Customer customer = _context.Customers.FirstOrDefault(x=>x.Email == email);
            return customer;
        }
        public bool registerPost(RegisterVm model)
        {
            if (_context.Users.Any(x => x.Email == model.email))
            {
                return false;
            }
            else
            {
                User user = new User()
                {
                    Firstname = model.firstname,
                    Lastname = model.lastname,
                    Phonenumber = model.Contact,
                    Roleid = 2,
                    Email = model.email,
                    Birthdate = model.birthdate,
                    Gender = model.Gender,
                    City = model.city,
                    Address = model.Address,
                    Passwordhash = model.Password,
                    IsDeleted = false,
                    Createddate = DateTime.Now,

                };
                _context.Users.Add(user);
                _context.SaveChanges();

                Customer customer = new Customer()
                {
                    Name = model.firstname + " " + model.lastname,
                    Email = model.email,
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

        public User getSessionData(string email)
        {
            User? user = _context.Users.FirstOrDefault(x => x.Email == email);
            return user;
        }

        public UserProfile getUserProfile(int? uId)
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
                birthdate = user.Birthdate,
                city = user.City,
                gender = user.Gender,

            };
            return userProfile;
        }
        public bool editUserProfile(UserProfile profile)
        {
            User? user = _context.Users.FirstOrDefault(x => x.Userid == profile.UserId);
            if (user != null)
            {
                user.Firstname = profile.FirstName;
                user.Address = profile.Address;
                user.Lastname = profile.LastName;
                user.Email = profile.Email;
                user.Birthdate = profile.birthdate;
                user.City = profile.city;
                user.Gender = profile.gender;
                user.Phonenumber = profile.Contact;
                _context.Users.Update(user);
                _context.SaveChanges();

                Customer customer = _context.Customers.FirstOrDefault(x => x.Email == user.Email);
                if (customer != null)
                {
                    customer.Name = profile.FirstName + " " + profile.LastName;
                    customer.Address = profile.Address;
                    customer.Email = profile.Email;
                    customer.City = profile.city;
                    //customer.Gender = profile.gender;
                    customer.Phonenumber = profile.Contact;
                    _context.Customers.Update(customer);
                    _context.SaveChanges();
                }

                return true;
            }
            return false;

        }

        public viewBookModel viewBookDetails(int bookId, int? userId)
        {
            Customer? customer = _context.Customers.FirstOrDefault(x => x.Userid == userId);
            Book? book = _context.Books.FirstOrDefault(x => x.Bookid == bookId);
            Addtocart? cart = _context.Addtocarts?.FirstOrDefault(o => o.Customerid == customer.Customerid && o.Bookid == bookId);
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
                cartId = (int)(cart != null && cart?.Cartid != null ? cart.Cartid : 0),
                Addtocarts = _context.Addtocarts?.Where(x => x.Customerid == customer.Customerid).ToList(),
                
                
            };
            return model;
        }
        public OrderData getOrderDetails(int bookId, int? UserId)
        {
            Book? book = _context.Books.FirstOrDefault(x => x.Bookid == bookId);
            Customer? customer = _context.Customers.FirstOrDefault(x => x.Userid == UserId);

            OrderData orderdata = new OrderData()
            {
                bookId = bookId,
                BookName = book.Title,
                Price = book.Price,
                AuthorName = _context.Authors?.FirstOrDefault(i => i.Authorid == book.Authorid).Name,
                customerId = customer.Customerid,
                CustomerName = customer.Name,
                Email = customer.Email,
                PhoneNumber = customer.Phonenumber,
                OrderAddress = customer.Address,
                city = customer.City,
                Quantity = 1,
                TotalAmount = book.Price,
                UpdateCustomerName = customer.Name,
                UpdateEmail = customer.Email,
                UpdatePhoneNumber = customer.Phonenumber,
                UpdateOrderAddress = customer.Address,
                updateCity = customer.City,
                UpdateQuantity = 1,
            };

            return orderdata;
        }

        public void GetAddToCart(int bookId, int? userId, int cartId)
        {
            int? custId = _context.Customers.FirstOrDefault(x => x.Userid == userId).Customerid;
            var cartData = _context.Addtocarts.FirstOrDefault(x => x.Cartid == cartId);
            if (cartData == null)
            {
                Addtocart addtocart = new Addtocart()
                {
                    Customerid = custId.Value,
                    Bookid = bookId,
                    Isremoved = false,
                    CreatedDate = DateTime.Now,

                };
                _context.Addtocarts.Add(addtocart);
                _context.SaveChanges();
            }
            else
            {
                cartData.Isremoved = false;
                cartData.UpdatedDate = DateTime.Now;
                _context.SaveChanges();
            }
        }
        public void GetRemoveFromCart(int cartId, int? userId)
        {
            int? custId = _context.Customers.FirstOrDefault(x => x.Userid == userId).Customerid;
            var cartData = _context.Addtocarts.FirstOrDefault(x => x.Cartid == cartId);

            cartData.Isremoved = true;
            cartData.UpdatedDate = DateTime.Now;
            _context.SaveChanges();

        }
        public void confirmOrder(OrderData data, int? userId)
        {
            int? custId = _context.Customers.FirstOrDefault(x => x.Userid == userId).Customerid;
            Book? book = _context.Books.FirstOrDefault(x => x.Bookid == data.bookId);
            Order order = new Order()
            {
                Customername = data.CustomerName,
                Email = data.Email,
                Phonenumber = data.PhoneNumber,
                Address = data.OrderAddress,
                City = data.city,
                Customerid = custId,
                Orderdate = DateTime.Today.Date,
                Orderstatusid = 1,
                Createdby = userId,
                Createddate = DateTime.Now,
                Isdeleted = false,
                Totalamount = ((decimal)(data.Price * (decimal)data.Quantity))
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            decimal quantity = (decimal)data.Quantity;
           
            Orderdetail orderdetail = new Orderdetail()
            {
                Orderid = order.Orderid,
                Bookid = data.bookId,
                Price = book.Price,
                Quantity = (int)data.Quantity,
                Totalamount = (data.Price * quantity), 
                Createdby = userId.ToString(),
                Createddate= DateTime.Now,
                
            };
            _context.Orderdetails.Add(orderdetail);
            _context.SaveChanges();

        }
        public void UpdateOrder(OrderData data, int? userId)
        {


        }


    }
}


