using BusinessLogic.Interface;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using DataAccess.DataContext;
using DataAccess.DataModels;
using DataAccess.CustomModels;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net.Mail;
using System.Net;

namespace BusinessLogic.Repository
{
    public class Authentication : IAuthentication

    {
        private readonly ApplicationDbContext _context;
        public Authentication(ApplicationDbContext context)
        {
            _context = context;

        }
        public User getSessionData(string email)
        {
            User? user = _context.Users.FirstOrDefault(x => x.Email == email);
            return user;
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

        public bool sendmail(string Email)
        {
            User? user = _context.Users.FirstOrDefault(x => x.Email == Email);
            if (user != null)
            {
                string senderEmail = "tatva.dotnet.milanvaru@outlook.com";
                string senderPassword = "vpgozcxbptbunspz";
                string recipientEmail = Email;
                string resetPasswordLink = "https://localhost:44369/home/ResetPasswordPage?email=" + recipientEmail;
                string body = $"Click the link below to reset your password:<br/><a href='{resetPasswordLink}'>click Here</a>";
                string subject = "Reset Your Password";
                int result;

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(senderEmail, senderPassword),

                };

                MailMessage mailMessage = new MailMessage(from: senderEmail, to: recipientEmail, subject, body);
                mailMessage.IsBodyHtml = true;

                try
                {
                    client.SendMailAsync(mailMessage);
                    result = 1;
                    Console.WriteLine("Email sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                    result = 0;
                }

                Emaillog emaillog = new Emaillog()
                {
                    Emailid = user.Email,
                    Message = body,
                    Userid = user.Userid,
                    Senddate = DateTime.Now,
                    Issent = result == 1 ? true : false,
                };
                _context.Emaillogs.Add(emaillog);
                _context.SaveChanges();

                return true;
            }
            return false;
        }
    }
    public class Authorize : Attribute, IAuthorizationFilter
    {
        private readonly string _role;

        public Authorize(string role)
        {
            this._role = role;

        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            // var user = SessionUtiles.GetLoggedInUser(context.HttpContext.Session);
            var jwtservice = context.HttpContext.RequestServices.GetService<IJwtServices>();

            var token = context.HttpContext.Request.Cookies["jwt"];


            if (jwtservice == null)
            {
                return;
            }

            if (token == null || !jwtservice.Validate(token, out JwtSecurityToken validetedToken))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Home", Action = "Index" }));
                return;
            }
            var roleclaim = validetedToken.Claims.Where(x => x.Type == "role").FirstOrDefault();
            var roletype = roleclaim.Value;

            if (roleclaim == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Home", Action = "Index" }));
                return;
            }

            if (string.IsNullOrEmpty(roleclaim.Value) || (_role.Any() && !_role.Contains(roleclaim.Value)))
            {

                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Home", Action = "Index" }));
                return;


            }


        }
    }

}
