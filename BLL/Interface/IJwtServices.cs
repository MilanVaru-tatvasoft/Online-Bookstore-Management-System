using DataAccess.CustomModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public interface IJwtServices
    {
        public string GenerateJwtToken(string email,string roleId);

       // public CookieDataModel GetUserDetails(string token);
        public bool Validate(string jwtToken, out JwtSecurityToken jwtSecurityToken);

    }
}
