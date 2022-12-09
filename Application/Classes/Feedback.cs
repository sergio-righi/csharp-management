using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Application.Classes
{
    [Serializable]
    public class Feedback
    {
        public EFeedback type { get; set; }
        public string message { get; set; }

        public Feedback(EFeedback type, string message)
        {
            this.type = type;
            this.message = message;
        }
    }

    public static class ResponseExtension
    {
        private const string Message = "feedback";

        public static List<Feedback> GetMessage(this ITempDataDictionary tempData)
        {
            CreateFeedbackTempData(tempData);

            return DeserializeObject(tempData[Message] as string);
        }

        public static void CreateFeedbackTempData(this ITempDataDictionary tempData)
        {
            if (!tempData.ContainsKey(Message))
            {
                tempData[Message] = string.Empty;
            }
        }

        public static void AddFeedback(this ITempDataDictionary tempData, Feedback feedback)
        {
            if (feedback == null)
            {
                throw new ArgumentNullException(nameof(Feedback));
            }

            var deserializeFeedbackList = tempData.GetMessage();

            deserializeFeedbackList.Add(feedback);

            tempData[Message] = SerializeObject(deserializeFeedbackList);
        }

        public static string SerializeObject(List<Feedback> tempData)
        {
            return JsonConvert.SerializeObject(tempData);
        }

        public static List<Feedback> DeserializeObject(string tempData)
        {
            if (tempData.Length == 0)
            {
                return new List<Feedback>();
            }

            return JsonConvert.DeserializeObject<List<Feedback>>(tempData);
        }

        public static IActionResult WithSuccess(this IActionResult result, string message)
        {
            return new FeedbackResult(result, EFeedback.Success, message);
        }

        public static IActionResult WithInfo(this IActionResult result, string message)
        {
            return new FeedbackResult(result, EFeedback.Info, message);
        }

        public static IActionResult WithWarning(this IActionResult result, string message)
        {
            return new FeedbackResult(result, EFeedback.Warning, message);
        }

        public static IActionResult WithError(this IActionResult result, string message)
        {
            return new FeedbackResult(result, EFeedback.Error, message);
        }
    }

    public class FeedbackResult : IActionResult
    {
        public EFeedback TypeId { get; set; }
        public string Message { get; set; }
        public IActionResult InnerResult { get; set; }

        public FeedbackResult(IActionResult innerResult, EFeedback type, string message)
        {
            this.TypeId = type;
            this.Message = message;
            this.InnerResult = innerResult;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            ITempDataDictionaryFactory factory = context.HttpContext.RequestServices.GetService(typeof(ITempDataDictionaryFactory)) as ITempDataDictionaryFactory;

            ITempDataDictionary tempData = factory.GetTempData(context.HttpContext);

            tempData.AddFeedback(new Feedback(TypeId, Message));

            return InnerResult.ExecuteResultAsync(context);
        }
    }
}