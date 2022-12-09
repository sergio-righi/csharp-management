using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Web;
using Tool.Utilities;

namespace Application.Classes.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizationAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly ERole[] _roles;

        public AuthorizationAttribute(params ERole[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //var user = context.HttpContext.User;

            //if (!user.Identity.IsAuthenticated)
            //{
            //    // it isn't needed to set unauthorized result 
            //    // as the base class already requires the user to be authenticated
            //    // this also makes redirect to a login page work properly
            //    // context.Result = new UnauthorizedResult();
            //    return;
            //}

            //// you can also use registered services
            //var someService = context.HttpContext.RequestServices.GetService<ISomeService>();

            //var isAuthorized = someService.IsUserAuthorized(user.Identity.Name, _roles);

            //if (!isAuthorized)
            //{
            //    context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
            //    return;
            //}
        }
    }
}