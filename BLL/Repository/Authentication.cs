using BusinessLogic.Interface;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;



namespace BusinessLogic.Repository
{
    public class Authentication:IAuthentication
    {
    }
    public class Authorize : Attribute,IAuthorizationFilter
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
