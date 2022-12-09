
using Application.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ViewRenderService : IViewRenderService
    {
        private readonly IRazorViewEngine RazorViewEngine;
        private readonly ITempDataProvider TempDataProvider;
        private readonly IHttpContextAccessor ContextAccessor;

        public ViewRenderService(IRazorViewEngine razorViewEngine,
                                 ITempDataProvider tempDataProvider,
                                 IHttpContextAccessor contextAccessor)
        {
            RazorViewEngine = razorViewEngine;
            ContextAccessor = contextAccessor;
            this.TempDataProvider = tempDataProvider;
        }

        public async Task<string> RenderToString(string viewName, object model, string prefix)
        {
            var actionContext = GetActionContext();

            using (var sw = new StringWriter())
            {
                var viewResult = this.FindView(actionContext, viewName);

                if (viewResult == null)
                {
                    throw new ArgumentNullException($"{viewName} does not match any available view");
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                };

                if (!string.IsNullOrWhiteSpace(prefix))
                {
                    viewDictionary.TemplateInfo.HtmlFieldPrefix = prefix;
                }

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, TempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.RenderAsync(viewContext);

                return sw.ToString();
            }
        }

        private IView FindView(ActionContext actionContext, string partialName)
        {
            var getPartialResult = RazorViewEngine.GetView(null, partialName, false);
            if (getPartialResult.Success)
            {
                return getPartialResult.View;
            }

            var findPartialResult = RazorViewEngine.FindView(actionContext, partialName, false);
            if (findPartialResult.Success)
            {
                return findPartialResult.View;
            }

            var searchedLocations = getPartialResult.SearchedLocations.Concat(findPartialResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find partial '{partialName}'. The following locations were searched:" }.Concat(searchedLocations));

            throw new InvalidOperationException(errorMessage);
        }

        private ActionContext GetActionContext()
        {
            return new ActionContext(ContextAccessor.HttpContext, ContextAccessor.HttpContext.GetRouteData(), new ActionDescriptor());

        }
    }
}
