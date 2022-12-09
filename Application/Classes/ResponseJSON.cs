using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Application.Classes
{
    public class ResponseJSON
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string Message { get; set; }
        public string Content { get; set; }
        public bool IsSuccess { get; set; }
        public string Feedback
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Message))
                {
                    return AppUtil.Message(IsSuccess ? EFeedback.Success : EFeedback.Warning, Message, true);
                }

                return null;
            }
        }
    }
}
