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
            return PartialView("_AddBookModal");
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

    }
}
