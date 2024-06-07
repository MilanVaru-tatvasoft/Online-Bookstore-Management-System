using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Interface;
using DataAccess.CustomModels;
using BusinessLogic.Repository;

using DataAccess.DataContext;
using DataAccess.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Online_Bookstore_Management_System.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ICustomerRepo _customerRepo;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly IAdminDashboardRepo _adminDashboard;

        public AdminController(ILogger<AdminController> logger, ICustomerRepo customerRepo, IHttpContextAccessor httpcontext, IAdminDashboardRepo adminDashboard)
        {
            _logger = logger;
            _customerRepo = customerRepo;
            _httpcontext = httpcontext;
            _adminDashboard = adminDashboard;
        }


        [Authorize("Admin")]
        public IActionResult AdminDashboard()
        {
            return View();
        }
        public IActionResult AdminDashboard2()
        {
            AdminDashboardModel model = new AdminDashboardModel();
            model = _adminDashboard.getAdminDashboardData();
            return PartialView("_AdminDashData", model);
        }
        public IActionResult getAdminBookList(AdminBookListmodel model)
        {
            _adminDashboard.getBookList(model);
            return PartialView("_AdminBookList", model);
        }
        public IActionResult searchBooks(AdminBookListmodel model)
        {
            _adminDashboard.getBookList(model);
            return PartialView("_AdminBookList", model);
        }
        public IActionResult ViewBookDetails(int bookId)
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");

            viewBookModel model = _adminDashboard.viewBookDetails(bookId, userId);
            return PartialView("_ViewBookDetail", model);
        }
        public IActionResult getAddBook()
        {
            viewBookModel model = _adminDashboard.getAddBook();

            return PartialView("_AddBookModal", model);
        }
        public IActionResult AddBook(viewBookModel model)
        {
            if (_adminDashboard.isbookExist(model.Title))
            {
                return Json(new { code = 401 });
            }
            else
            {
                int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
                _adminDashboard.addBook(model, userId);
                return Json(new { code = 402 });
            }

        }
        public IActionResult updateBook(viewBookModel model)
        {
            if (_adminDashboard.isbookExist(model.Title))
            {
                int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
                _adminDashboard.updateBook(model, userId);
                return Json(new { code = 401 });
            }
            else
            {

                return Json(new { code = 402 });
            }

        }
        public IActionResult getEditBook(int bookId)
        {
            viewBookModel model = _adminDashboard.getEditBook(bookId);
            return PartialView("_AddBookModal", model);
        }
        public IActionResult getDeleteBook(int bookId)
        {
            if (_adminDashboard.getDeleteBook(bookId))
            {

                return Json(new { code = 401 });
            }
            return Json(new { code = 402 });
        }
        public IActionResult getAdminProfile()
        {
            int? uId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            if (uId != null)
            {
                AdminProfileModel profile = _adminDashboard.getAdminProfile(uId);
                return PartialView("_AdminProfile", profile);
            }
            return View();
        }
        public IActionResult editAdminProfile(AdminProfileModel profile)
        {
            if (ModelState.IsValid)
            {
                bool result = _adminDashboard.editAdminProfile(profile);
                if (result)
                {
                    return Json(new { code = 401 });
                }
                return Json(new { code = 402 });
            }
            return View();
        }

        public IActionResult getAutherList()
        {
            AuthorListmodel model = _adminDashboard.GetAuthorList();
            return PartialView("_AuthersList", model);
        }
        public IActionResult getCategoryList()
        {
            CategoryListModel model = _adminDashboard.getCategoriesList();

            return PartialView("_CategoryList", model);
        }
        public IActionResult getAddAuthor(int authorId)
        {
            AuthorListmodel model = _adminDashboard.getEditAuthor(authorId);
            return PartialView("_AddAutherModal", model);
        }
        public IActionResult getAddCategory(int categoryId)
        {
            CategoryListModel model = _adminDashboard.getAddCategory(categoryId);
            return PartialView("_AddCategoryModal", model);
        }

        public IActionResult getDeleteAuthor(int authorId)
        {
            if (_adminDashboard.getDeleteAuthor(authorId))
            {

                return Json(new { code = 401 });
            }
            return Json(new { code = 402 });
        }
        public IActionResult getDeleteCategory(int categoryId)
        {
            if (_adminDashboard.getDeleteCategory(categoryId))
            {

                return Json(new { code = 401 });
            }
            return Json(new { code = 402 });
        }

        public IActionResult addOrUpdateAuthor(AuthorListmodel model)
        {
            if (_adminDashboard.AddOrUpdateAuthor(model))
            {

                return Json(new { code = 401 });
            }
            return Json(new { code = 402 });

        }
        public IActionResult addOrUpdateCategory(CategoryListModel model)
        {
            if (_adminDashboard.AddOrUpdateCategory(model))
            {

                return Json(new { code = 401 });
            }
            return Json(new { code = 402 });

        }
    }
}
