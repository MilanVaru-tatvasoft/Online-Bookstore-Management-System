using DataAccess.CustomModels;
using DataAccess.DataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public interface IAuthentication
    {
        public bool ValidateLogin(string email, string password);
        public bool ResetPasswordPost(ResetPasswordModel model);
        public User GetSessionData(string email);
        public void EmailSender(string email, string subject, string message);

        public bool ResetPasswordMail(string email);
        public string OrderMailMessageBody(string status);
        public void StoreProfilePhoto(IFormFile file, int userId);
    }
}
