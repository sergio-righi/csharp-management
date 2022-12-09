
using Application.Classes;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tool.Utilities;

namespace Application.Helpers
{
    public static class FeedbackHelper
    {
        public static IHtmlContent Feedback(this IHtmlHelper helper, EFeedback type, string content, bool removable = true) => new HtmlString
        (
            AppUtil.Message(type, content, removable)
        );
    }
}