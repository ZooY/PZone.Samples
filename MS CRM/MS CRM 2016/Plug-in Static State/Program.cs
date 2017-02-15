using System;
using Microsoft.Xrm.Sdk;


namespace PZone.Samples
{
    /// <summary>
    /// Проверка сохранения состояния класса плагина между процессами 
    /// выполнения.
    /// </summary>
    /// <remarks>
    /// Если зарегистрировать плагин на Pre-Update контакта, то будет видно, 
    /// что при обновлении любого контакта значение, утановленное для поля 
    /// _testValue сохраняется (для проверки этого, значение записывается в 
    /// указанное в конфигурации поле).
    /// Также видно, что для каждого шага (Step) создается отдельный экземпляр 
    /// класса плагина.
    /// </remarks>
    public class StaticStatePlugin : IPlugin
    {
        private Guid? _testValue;
        private readonly string _fieldName;


        public StaticStatePlugin(string unsecureConfig)
        {
            _fieldName = unsecureConfig;
        }


        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var entity = (Entity)context.InputParameters["Target"];

            if (!_testValue.HasValue)
                _testValue = Guid.NewGuid();

            entity[_fieldName] = _testValue.Value.ToString();
        }
    }
}