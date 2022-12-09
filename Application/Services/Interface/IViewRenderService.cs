using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface IViewRenderService
    {
        Task<string> RenderToString(string viewName, object model, string prefix = null);
    }
}
