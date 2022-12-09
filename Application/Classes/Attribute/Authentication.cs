using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Web;

namespace Application.Classes.Attribute
{
    //public interface IAuthenticationFilter
    //{
    //    void OnAuthentication(AuthenticationContext filterContext);

    //    void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext);
    //}

    public class AuthenticationAttribute : ActionFilterAttribute
    {
        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    if (!Session.Opened())
        //    {
        //        throw new HttpException((int)HttpStatusCode.Unauthorized, Message.SessionExpired);
        //    }
        //}
        //public void OnAuthentication(AuthenticationContext filterContext)
        //{
        //    //For demo purpose only. In real life your custom principal might be retrieved via different source. i.e context/request etc.
        //    //filterContext.Principal = new MyCustomPrincipal(filterContext.HttpContext.User.Identity, new[] { "Admin" }, "Red");
        //}

        //public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        //{
        //    //var color = ((MyCustomPrincipal)filterContext.HttpContext.User).HairColor;

        //    //var user = filterContext.HttpContext.User;

        //    //if (!user.Identity.IsAuthenticated)
        //    //{
        //    //    filterContext.Result = new HttpUnauthorizedResult();
        //    //}

        //    if (Sessao.VerificarSessao())
        //    {
        //        filterContext.Result = new HttpUnauthorizedResult();
        //    }
        //}
    }
}