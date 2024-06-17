
using BusinessLogic.Interface;
using BusinessLogic.Repository;
using DataAccess.CustomModels;
using DataAccess.DataModels;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;


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

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult loginPostMethod(LoginVm model)
        {
            if (!string.IsNullOrEmpty(model.loginEmail) && !string.IsNullOrEmpty(model.password))
            {
                if (_authentication.validateLogin(model.loginEmail, model.password))
                {
                    User user = _authentication.getSessionData(model.loginEmail);
                    string roleId;
                    if (user.Roleid == 1) { roleId = "Admin"; } else { roleId = "customer"; }
                    string name = user.Firstname + " " + user.Lastname;
                    var jwtToken = _jwtServices.GenerateJwtToken(model.loginEmail, roleId);
                    Response.Cookies.Append("jwt", jwtToken);
                    _httpcontext.HttpContext.Session.SetString("UserName", name);
                    _httpcontext.HttpContext.Session.SetInt32("UserId", user.Userid);
                    _httpcontext.HttpContext.Session.SetString("Token", jwtToken);


                    if (user.Roleid == 1)
                    {
                        Admin admin = _customerRepo.getAdminData(model.loginEmail);
                        _httpcontext.HttpContext.Session.SetInt32("AdminId", admin.Adminid);

                        return RedirectToAction("AdminDashboard", "Admin");
                    }
                    else if (user.Roleid == 2)
                    {
                        Customer customer = _customerRepo.getCustomerData(model.loginEmail);
                        _httpcontext.HttpContext.Session.SetInt32("customerId", customer.Customerid);
                        return RedirectToAction("customerDashboard");

                    }
                    else
                    {
                        return RedirectToAction("Index");

                    }
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [Authorize("customer")]
        public IActionResult CustomerDashboard()
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            Customer_MainPage model = new Customer_MainPage();
            int pageNumber = 0;
            model = _customerRepo.getdata(model, userId, pageNumber);
            return View(model);
        }
        public IActionResult getcustDash(Customer_MainPage model)
        {

            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            int pageNumber = 1;
            _customerRepo.getdata(model, userId, pageNumber);
            return PartialView("_CustomerMainPage", model);
        }
        public IActionResult getDashboardData(int pageNumber)
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            Customer_MainPage model = new Customer_MainPage();
            _customerRepo.getdata(model, userId, pageNumber);
            return Json(model.bookList);
        }

        public IActionResult getregistrationform()
        {
            return RedirectToAction("RegisterPage");
        }
        public IActionResult RegisterPage()
        {
            return View();
        }
        public IActionResult registerPost(RegisterVm model)
        {
            bool result = _customerRepo.registerPost(model);
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
                return PartialView("_MyOrdersPage" ,model);
            
        }
        public IActionResult getUserProfile()
        {
            int? uId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            if (uId != null)
            {
                UserProfile userProfile = _customerRepo.getUserProfile(uId);
                return PartialView("_CustomerProfile", userProfile);
            }
            return View();
        }
        public IActionResult editUserProfile(UserProfile profile)
        {
            if (ModelState.IsValid)
            {
                bool result = _customerRepo.editUserProfile(profile);
                if (result)
                {
                    return Json(new { code = 401 });
                }
                return Json(new { code = 402 });
            }
            return View();
        }
        public IActionResult ViewBooksPage(int bookId)
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");

            viewBookModel model = _customerRepo.viewBookDetails(bookId, userId);
            return PartialView("_viewBooksPage", model);
        }

        public IActionResult searchBooks(Customer_MainPage model)
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            int pageNumber = 1;
            _customerRepo.getdata(model, userId, pageNumber);
            return PartialView("_CustomerMainPage", model);
        }

        public IActionResult getAddToCart(int bookId, int cartId, int quantity)
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            _customerRepo.GetAddToCart(bookId, userId, cartId, quantity);
            return Json(new { code = 401 });
        }
        public IActionResult getRemoveFromCart(int cartId)
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
        public IActionResult ResetPassword(ResetPasswordModel model)
        {

            if (_authentication.resetPassword(model))
            {
                return Json(new { code = 401 });
            }
            return Json(new { code = 401 });
        }

        public IActionResult ForgotPasswordModal()
        {
            return PartialView("_PasswordRecoveryModal");
        }
        public IActionResult forgotpassword(string Email)
        {
            if (_authentication.sendmail(Email))
            {

                return Json(new { code = 401 });
            }
            else
            {

                return Json(new { code = 402 });
            }
        }
        public IActionResult submitReviewAndRating(viewBookModel model)
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            _customerRepo.getSubmitReviewAndRating(model, userId);
            return Ok(model.bookId);


        }
        public IActionResult GetOrderDatailsPage(int bookId)
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            OrderData model = _customerRepo.getOrderDetails(bookId, userId);
            return PartialView("_OrderBook", model);
        }

        public IActionResult getCartList()
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            OrderData model = _customerRepo.getCartList(userId);
            return PartialView("_MyCartList", model);
        }


        public IActionResult getBuyNow(OrderData data)
        {
            return PartialView("_ConfirmOrder", data);
        }

        public IActionResult getCheckout(OrderData data)
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");

            data = _customerRepo.getCartList(userId);

            return PartialView("_ConfirmOrder", data);

        }


        public IActionResult getPayment(OrderData data)
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            if (data.BookName == null)
            {
               OrderData model = _customerRepo.getCartList(userId);
                model = data;
                int orderId = _customerRepo.confirmOrder(model, userId);
                model.Orderid = orderId;
                return PartialView("_PaymentPage", data);
            }
            else
            {
                int orderId = _customerRepo.confirmOrder(data, userId);
                data.Orderid = orderId;
                return PartialView("_PaymentPage", data);
            }


          
        }

       

        public IActionResult getPaymentDone(string paymentType, int OrderId)
        {
            int? userId = _httpcontext.HttpContext.Session.GetInt32("UserId");
            bool status = _customerRepo.getPaymentDone(paymentType, OrderId, userId);

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