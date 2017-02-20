using System;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;


namespace PZone.Samples
{
    public class StaticStateWorkflow: CodeActivity
    {
        private string _message;


        [RequiredArgument]
        [Output("Message")]
        public OutArgument<string> Message { get; set; }


        protected override void Execute(CodeActivityContext context)
        {
            if (string.IsNullOrWhiteSpace(_message))
                _message = Guid.NewGuid().ToString();
            Message.Set(context, _message);
        }
    }
}