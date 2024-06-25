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
using Microsoft.AspNetCore.Http.HttpResults;

namespace Online_Bookstore_Management_System.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ICustomerRepo _customerRepo;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly IAdminDashboardRepo _adminDashboard;
        private readonly IAuthentication _authentication;

        public AdminController(ILogger<AdminController> logger, ICustomerRepo customerRepo,
            IHttpContextAccessor httpcontext, IAdminDashboardRepo adminDashboard,
            IAuthentication authentication)
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
            AdminDashboardModel model = _adminDashboard.GetAdminDashboardData();
            return View(model);
        }
        public IActionResult AdminDashboard2()
        {
            AdminDashboardModel model = _adminDashboard.GetAdminDashboardData();

            return PartialView("_AdminDashData", model);
        }

        public IActionResult GetResetPassword(string email)
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
        public IActionResult AdminResetPassword(AdminProfileModel data)
        {
            ResetPasswordModel model = new ResetPasswordModel()
            {
                Password = data.Password,
                Password2 = data.ConfirmPassword,
                Email = data.Email,
                UserId = data.UserId,
            };

            if (_authentication.ResetPasswordPost(model))
            {
                return Json(new { code = 401 });
            }
            return Json(new { code = 401 });
        }

        public IActionResult GetAdminBookList(AdminBookListModel model)
        {
            _adminDashboard.GetBookList(model);
            return PartialView("_AdminBookList", model);
        }
        public IActionResult GetFilterBookList(AdminBookListModel model)
        {
            _adminDashboard.GetBookList(model);
            return PartialView("_AdminBookList", model);
        }
        public IActionResult ViewBookData(int bookId)
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");

            viewBookModel model = _adminDashboard.ViewBookDetails(bookId, userId);
            return PartialView("_ViewBookDetail", model);
        }
        public IActionResult GetAddBook()
        {
            viewBookModel model = _adminDashboard.GetAddBook();

            return PartialView("_AddBookModal", model);
        }
        public IActionResult AddBook(viewBookModel model)
        {
            if (_adminDashboard.IsBookExist(model.Title))
            {
                return Json(new { code = 401 });
            }
            else
            {
                int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
                _adminDashboard.AddBook(model, userId);
                return Json(new { code = 402 });
            }

        }
        public IActionResult UpdateBook(viewBookModel model)
        {
            if (_adminDashboard.IsBookExist(model.Title))
            {
                int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
                _adminDashboard.UpdateBook(model, userId);
                return Json(new { code = 401 });
            }
            else
            {

                return Json(new { code = 402 });
            }

        }
        public IActionResult GetEditBook(int bookId)
        {
            viewBookModel model = _adminDashboard.GetEditBook(bookId);
            return PartialView("_AddBookModal", model);
        }
        public IActionResult GetDeleteBook(int bookId)
        {
            if (_adminDashboard.GetDeleteBook(bookId))
            {

                return Json(new { code = 401 });
            }
            return Json(new { code = 402 });
        }
        public IActionResult GetAdminProfile()
        {
            int? uId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            if (uId != null)
            {
                AdminProfileModel profile = _adminDashboard.GetAdminProfile(uId);
                return PartialView("_AdminProfile", profile);
            }
            return View();
        }
        public IActionResult EditAdminProfile(AdminProfileModel profile)
        {

            try
            {
                bool result = _adminDashboard.EditAdminProfile(profile);
                int code = result ? 401 : 402;
                return Json(new { code });
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Exception occurred: {ex.Message}");

                return Json(new { error = "An error occurred while editing the admin profile.", code = 500 });
            }



        }

        public IActionResult GetAuthorList()
        {
            AuthorListModel model = _adminDashboard.GetAuthorList();
            return PartialView("_AuthorsList", model);
        }
        public IActionResult GetCategoryList()
        {
            CategoryListModel model = _adminDashboard.GetCategoriesList();

            return PartialView("_CategoryList", model);
        }
        public IActionResult GetAddAuthor(int AuthorId)
        {
            AuthorListModel model = _adminDashboard.GetEditAuthor(AuthorId);
            return PartialView("_AddAuthorModal", model);
        }
        public IActionResult GetAddCategory(int categoryId)
        {
            CategoryListModel model = _adminDashboard.GetAddCategory(categoryId);
            return PartialView("_AddCategoryModal", model);
        }

        public IActionResult GetDeleteAuthor(int AuthorId)
        {
            try
            {
                bool result = _adminDashboard.GetDeleteAuthor(AuthorId);
                int code = result ? 401 : 402;
                return Json(new { code });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");

                return Json(new { error = "An error occurred while deleting the Author.", code = 500 });
            }
        }
        public IActionResult GetDeleteCategory(int categoryId)
        {
            try
            {
                bool result = _adminDashboard.GetDeleteCategory(categoryId);
                int code = result ? 401 : 402;

                return Json(new { code });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");

                return Json(new { error = "An error occurred while deleting the category.", code = 500 });
            }
        }

        public IActionResult AddOrUpdateAuthor(AuthorListModel model)
        {
            try
            {
                bool result = _adminDashboard.AddOrUpdateAuthor(model);
                int code = result ? 401 : 402;

                return Json(new { code });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");

                return Json(new { error = "An error occurred while editing the Author.", code = 500 });
            }

        }
        public IActionResult AddOrUpdateCategory(CategoryListModel model)
        {
            try
            {
                bool result = _adminDashboard.AddOrUpdateCategory(model);
                int code = result ? 401 : 402;

                return Json(new { code });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");

                return Json(new { error = "An error occurred while editing the Category.", code = 500 });
            }

        }

        public IActionResult GetChartData()
        {
            DateTime date = DateTime.Now;
            AdminDashboardModel model = _adminDashboard.GetChartData(date);

            return Json(new { MonthlySales = model.MonthlySales, categoryList = model.Categories, NumberOfBooks = model.NoOfBooks, dailySales = model.DailySales });

        }

        public IActionResult GetOrderList()
        {
            OrderListModel model = _adminDashboard.GetOrderListData();
            return PartialView("_OrderListView", model);
        }
        public IActionResult HandleOrderAction(int tempId, int orderId, int customerId)
        {
            try
            {
                bool result = false;
                int code = 402;
                result = _adminDashboard.HandleOrderAction(orderId, customerId, tempId);
                code = result ? 401 : 402;
                return Json(new { code });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return Json(new { error = "An error occurred while processing the order action.", code = 500 });
            }

        }


    }
}
