using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Interface;
using DataAccess.CustomModels;
using BusinessLogic.Repository;

using DataAccess.DataContext;
using DataAccess.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Newtonsoft.Json;

namespace Online_Bookstore_Management_System.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ICustomerRepo _customerRepo;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly IAdminDashboardRepo _adminDashboard;
        private readonly IAuthentication _authentication;

        public AdminController(ILogger<AdminController> logger, ICustomerRepo customerRepo, IHttpContextAccessor httpcontext, IAdminDashboardRepo adminDashboard, IAuthentication authentication)
        {
            _logger = logger;
            _customerRepo = customerRepo;
            _httpcontext = httpcontext;
            _adminDashboard = adminDashboard;
            _authentication = authentication;
        }


        [Authorize("Admin")]
        public IActionResult AdminDashboard()
        {
            return View();
        }
        public IActionResult AdminDashboard2()
        {
            AdminDashboardModel model = new AdminDashboardModel();
            return PartialView("_AdminDashData", model);
        }
        public IActionResult GetOrderList()
        {
            OrderListModel model = new OrderListModel();
            model = _adminDashboard.getOrderListData();
            return PartialView("_OrderListView", model);
        }

        public IActionResult getResetPassword(string email)
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            ResetPasswordModel model = new ResetPasswordModel()
            {
                Email = email,
                UserId = userId,
            };
            return View("_AdminResetPasswordPage", model);
        }
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordModel model)
        {

            if (_authentication.resetPassword(model))
            {
                return Json(new { code = 401 });
            }
            return Json(new { code = 401 });
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

        public IActionResult getAuthorList()
        {
            AuthorListmodel model = _adminDashboard.GetAuthorList();
            return PartialView("_AuthorsList", model);
        }
        public IActionResult getCategoryList()
        {
            CategoryListModel model = _adminDashboard.getCategoriesList();

            return PartialView("_CategoryList", model);
        }
        public IActionResult getAddAuthor(int AuthorId)
        {
            AuthorListmodel model = _adminDashboard.getEditAuthor(AuthorId);
            return PartialView("_AddAuthorModal", model);
        }
        public IActionResult getAddCategory(int categoryId)
        {
            CategoryListModel model = _adminDashboard.getAddCategory(categoryId);
            return PartialView("_AddCategoryModal", model);
        }

        public IActionResult getDeleteAuthor(int AuthorId)
        {
            if (_adminDashboard.getDeleteAuthor(AuthorId))
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

        public IActionResult getViewOrder(int orderDetailId)
        {
            return PartialView("_ViewOrderPage");
        }

        public IActionResult getChartData()
        {
            int year = DateTime.Now.Year;
            AdminDashboardModel model = _adminDashboard.getmonthSales(year);

            return Json(new { MonthlySales = model.monthlySales, categoryList = model.categorties, NumberOfBooks = model.noofbooks });

        }
    }
}
