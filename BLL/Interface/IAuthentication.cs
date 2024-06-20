using DataAccess.CustomModels;
using DataAccess.DataModels;
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
        public bool resetPassword(ResetPasswordModel model);
        public User GetSessionData(string email);
        public void emailSender(string email, string subject, string message);

        public bool sendmail(string email);
        public string ordermessage(string status);
    }
}
