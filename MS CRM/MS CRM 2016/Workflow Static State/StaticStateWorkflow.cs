using System;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;


namespace PZone.Samples
{
    /// <summary>
    /// Проверка сохранения состояния класса действия между процессами 
    /// выполнения.
    /// </summary>
    /// <remarks>
    /// Если использовать действие в Workflow (с выводом его результата), то 
    /// будет видно, что при каждом выполнении WF действие будет возвращать 
    /// одно и тоже значение.
    /// Для каждого использования действия в рамках одного WF, результат у 
    /// каждого действия будет свой, но один и тот же для каждого места 
    /// использования.
    /// Результат не зависит от способа вызова WF или его синхронности.
    /// </remarks>
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