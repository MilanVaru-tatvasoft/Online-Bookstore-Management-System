
using BusinessLogic.Interface;
using BusinessLogic.Repository;
using DataAccess.CustomModels;
using DataAccess.DataModels;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Online_Bookstore_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICustomerRepo _customerRepo;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly IJwtServices _jwtServices;
        private readonly IAuthentication _authentication;


        public HomeController(ILogger<HomeController> logger, ICustomerRepo customerRepo, IJwtServices jwtServices, IHttpContextAccessor httpcontext, IAuthentication authentication)
        {
            _logger = logger;
            _customerRepo = customerRepo;
            _httpcontext = httpcontext;
            _jwtServices = jwtServices;
            _authentication = authentication;
        }


        #region Private Methods
        private void SetInvalidLoginMessage()
        {
            TempData["ToastMessage"] = "Invalid login credentials.";
        }

        private void SetToastMessage(string message)
        {
            TempData["ToastMessage"] = message;
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RegisterPage()
        {
            return View();
        }
        [HttpPost]

        public IActionResult LoginPostMethod(LoginModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.LoginEmail) || string.IsNullOrEmpty(model.Password))
                {
                    SetInvalidLoginMessage();
                    return RedirectToAction("Index");
                }

                User user = _authentication.GetSessionData(model.LoginEmail);

                if (user == null || !_authentication.ValidateLogin(model.LoginEmail, model.Password))
                {
                    SetInvalidLoginMessage();
                    return RedirectToAction("Index");
                }

                string fullName = $"{user.Firstname} {user.Lastname}";
                string role = (user.Roleid == 1) ? "Admin" : "Customer";
                var jwtToken = _jwtServices.GenerateJwtToken(model.LoginEmail, role);

                Response.Cookies.Append("jwt", jwtToken);
                _httpcontext.HttpContext.Session.SetString("UserName", fullName);
                _httpcontext.HttpContext.Session.SetInt32("UserId", user.Userid);
                _httpcontext.HttpContext.Session.SetString("Token", jwtToken);

                if (user.Roleid == 1)
                {
                    Admin admin = _customerRepo.GetAdminData(model.LoginEmail);
                    if (admin != null)
                    {
                        _httpcontext.HttpContext.Session.SetInt32("AdminId", admin.Adminid);
                        SetToastMessage("Logged In As An Admin!");
                        return RedirectToAction("AdminDashboard", "Admin");
                    }
                    else
                    {
                        throw new Exception("Admin data not found."); 
                    }
                }
                else if (user.Roleid == 2)
                {
                    Customer customer = _customerRepo.GetCustomerData(model.LoginEmail);
                    if (customer != null)
                    {
                        _httpcontext.HttpContext.Session.SetInt32("CustomerId", customer.Customerid);
                        SetToastMessage($"Welcome, {customer.Name}!");
                        return RedirectToAction("CustomerDashboard");
                    }
                    else
                    {
                        throw new Exception("Customer data not found.");
                    }
                }

                SetInvalidLoginMessage();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                TempData["ToastMessage"] = "An error occurred while processing your request.";

                return RedirectToAction("Index");
            }
        }


        [Authorize("Customer")]
        public IActionResult CustomerDashboard()
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            
            CustomerMainPage dashData = new CustomerMainPage();
            dashData = _customerRepo.GetCustomerDashboardData(dashData, userId, 1);

            return View(dashData);
        }

        public IActionResult CustomerMainPage()
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            CustomerMainPage dashData = new CustomerMainPage();
            dashData = _customerRepo.GetCustomerDashboardData(dashData, userId, 1);
            return PartialView("_CustomerMainPage", dashData);
        }

        public IActionResult CustomerDashBookList(CustomerMainPage model)
        {
            try
            {
                int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
                List<DashboardList> list = _customerRepo.GetCustomerDashBookList(model, userId, model.PageNumber);
                model.BookCount = _customerRepo.GetFilterBookCount(model);
                if (list != null && list.Count > 0)
                {
                    return Json(list);
                }
                else
                {
                    return Json(new List<DashboardList>());
                }
            }
            catch (Exception ex)
            {

                return Json(new { error = "An error occurred while fetching data.", ex.Message });
            }


        }

        public IActionResult GetRegistrationform()
        {
            return RedirectToAction("RegisterPage");
        }

        public IActionResult RegisterPost(UserProfile model)
        {
            bool result = _customerRepo.RegisterPost(model);
            if (result)
            {
                return Json(new { code = 401 });
            }
            return Json(new { code = 402 });
        }

        public IActionResult GetOrderHistory()
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            OrderData model = _customerRepo.GetOrderHistoy(userId);
            return PartialView("_MyOrdersPage", model);

        }

        public IActionResult GetUserProfile()
        {
            int? uId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            if (uId != null)
            {
                UserProfile userProfile = _customerRepo.GetUserProfile(uId);
                return PartialView("_CustomerProfile", userProfile);
            }
            return View();
        }

        public IActionResult EditUserProfile(UserProfile profile)
        {

            bool result = _customerRepo.EditUserProfile(profile);
            if (result)
            {
                return Json(new { code = 401 });
            }
            return Json(new { code = 402 });


        }

        public IActionResult ViewBookDetails(int bookId)
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");

            viewBookModel model = _customerRepo.ViewBookDetails(bookId, userId);
            return PartialView("_viewBooksPage", model);
        }

        public IActionResult GetAddToCart(int bookId, int cartId, int quantity)
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            _customerRepo.GetAddToCart(bookId, userId, cartId, quantity);
            return Json(new { code = 401 });
        }

        public IActionResult GetRemoveFromCart(int cartId)
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            _customerRepo.GetRemoveFromCart(cartId, userId);
            return Ok();
        }

        public IActionResult Logout()
        {
            _httpcontext.HttpContext.Session.Clear();
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Index");

        }

        public IActionResult ResetPasswordPage(string email)
        {
            ResetPasswordModel model = new ResetPasswordModel()
            {
                Email = email,
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult ResetPassword(UserProfile data)
        {
            ResetPasswordModel model = new ResetPasswordModel()
            {
                Password = data.Password,
                Password2 = data.Password2,
                Email = data.Email,
                UserId = data.UserId,
            };
            if (_authentication.ResetPasswordPost(model))
            {
                return Json(new { code = 401 });
            }
            return Json(new { code = 401 });
        }

        public IActionResult ForgotPasswordModal()
        {
            return PartialView("_PasswordRecoveryModal");
        }

        public IActionResult ForgotPassword(string Email)
        {
            if (_authentication.ResetPasswordMail(Email))
            {

                return Json(new { code = 401 });
            }
            else
            {

                return Json(new { code = 402 });
            }
        }

        public IActionResult SubmitReviewAndRating(viewBookModel model)
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            _customerRepo.GetSubmitReviewAndRating(model, userId);
            return Ok(model.bookId);


        }

        public IActionResult GetOrderDatailsPage(int bookId)
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            OrderData model = _customerRepo.GetOrderDetails(bookId, userId);
            return PartialView("_OrderBook", model);
        }

        public IActionResult GetCartList()
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            OrderData model = new OrderData();
            model = _customerRepo.GetCartList(model, userId);
            return PartialView("_MyCartList", model);
        }

        public IActionResult GetBuyNow(OrderData data)
        {
            return PartialView("_ConfirmOrder", data);
        }

        public IActionResult GetCheckout(OrderData data)
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");

            data = _customerRepo.GetCartList(data, userId);

            return PartialView("_ConfirmOrder", data);

        }

        public IActionResult GetPayment(OrderData data)
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            if (data.BookName == null)
            {
                OrderData model = _customerRepo.GetCartList(data, userId);
                int orderId = _customerRepo.confirmOrder(model, userId);
                model.OrderId = orderId;
                return PartialView("_PaymentPage", model);
            }
            else
            {
                int orderId = _customerRepo.confirmOrder(data, userId);
                data.OrderId = orderId;
                return PartialView("_PaymentPage", data);
            }



        }

        public IActionResult GetPaymentDone(string paymentType, int OrderId)
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            bool status = _customerRepo.GetPaymentDone(paymentType, OrderId, userId);

            return Ok();
        }

        public IActionResult GeneratePDF([FromQuery] int orderId)
        {
            var PayBill = _customerRepo.getBillDetails(orderId);

            if (PayBill == null)
            {
                return NotFound();
            }


            return new ViewAsPdf("PaymentBill", PayBill)
            {
                FileName = "Bill_BookStore.pdf"
            };

        }

        public IActionResult PaymentBill()
        {
            return View();
        }




    }
}