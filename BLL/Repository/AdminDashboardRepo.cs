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
            List<Book> bookList = _context.Books.Where(x => x.Isdeleted != true).ToList();
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

        public int isAuthorExist(string? AuthorName)
        {
            int? AuthorId = _context.Authors.FirstOrDefault(x => x.Name == AuthorName && x.Isdeleted != true)?.Authorid;

            if (AuthorId == null)
            {
                Author author = new Author() { Name = AuthorName };
                _context.Authors.Add(author);
                _context.SaveChanges();
                AuthorId = author.Authorid;
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
                author = _context.Authors.ToList(),
                categories = _context.Categories.ToList(),
            };
            return model;
        }
        public void addBook(viewBookModel model, int? userId)
        {
            int? AuthorId = isAuthorExist(model.authorName);
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
                authorName = _context.Authors?.FirstOrDefault(x => x.Authorid == book.Authorid).Name,
                publisherName = _context.Publishers?.FirstOrDefault(x => x.Publisherid == book.Publisherid).Name,
                categoryName = _context.Categories?.FirstOrDefault(x => x.Categoryid == book.Categoryid).Categoryname,
                bookPic = book.Bookphoto,
                Stockquantity = book.Stockquantity,
                author = _context.Authors.ToList(),
                categories = _context.Categories.ToList(),
            };
            return model;
        }
        public void updateBook(viewBookModel model, int? userId)
        {
            Book? book = _context.Books.FirstOrDefault(x => x.Bookid == model.bookId);

            int? AuthorId = isAuthorExist(model.authorName);
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
                FirstName = user.Firstname,
                LastName = user.Lastname,
                Email = user.Email,
                Contact = user.Phonenumber,
                Address = user.Address,
                birthdate = user.Birthdate,
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

                Admin? admin = _context.Admins.FirstOrDefault(x => x.Email == user.Email);
                if (admin != null)
                {
                    admin.Firstname = profile.FirstName;
                    admin.Lastname = profile.LastName;
                    admin.Address = profile.Address;
                    admin.Email = profile.Email;
                    admin.City = profile.city;
                    admin.Gender = profile.gender;
                    admin.Phonenumber = profile.Contact;
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
                authors = _context.Authors.Where(x => x.Isdeleted != true).ToList(),
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

        public AuthorListmodel getEditAuthor(int authorId)
        {
            Author? author = _context.Authors.FirstOrDefault(x => x.Authorid == authorId);
            AuthorListmodel model = new AuthorListmodel();
            if (author != null && authorId != 0)
            {
                model.authorId = author.Authorid;
                model.autherName = author.Name;
                model.birthdate = author.Birthdate;
                model.Bio = author.Bio;
            }
            else
            {
                model.authorId = 0;
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

        public bool getDeleteAuthor(int authorId)
        {
            if (authorId != 0)
            {
                Author? author = _context.Authors.FirstOrDefault(x => x.Authorid == authorId);
                author.Isdeleted = true;
                _context.Authors.Update(author);
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
            if (model.authorId != 0)
            {
                Author? author = _context.Authors.FirstOrDefault(x => x.Authorid == model.authorId);
                author.Name = model.autherName;
                author.Bio = model.Bio;
                author.Birthdate = model.birthdate;
                author.Isdeleted = false;
                _context.Authors.Update(author);
                _context.SaveChanges();
                return true;
            }
            else
            {
                if(_context.Authors.Any(x=>x.Name.Trim() != model.autherName.Trim()))
                {
                    Author auther = new Author()
                    {
                        Name = model.autherName, Bio = model.Bio,Birthdate= model.birthdate, Isdeleted = false
                    };
                    _context.Authors.Add(auther);
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


    }
}
