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
        public bool validateLogin(string email, string password);
        public bool resetPassword(ResetPasswordModel model);
        public User getSessionData(string email);
    }
}
