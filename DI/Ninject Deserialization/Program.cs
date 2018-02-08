using System;
using Newtonsoft.Json;
using Ninject;


namespace PZone.Samples
{
    class Program
    {
        static void Main()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IData>().To<Data>();
            kernel.Bind<IService>().To<Service>();

            var contractResolver = new NinjectContractResolver(kernel);
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                Formatting = Formatting.Indented
            };

            //var service = new Service();
            //IData data = new Data(service) { StringValue = "Тестовое значение", IntValue = 5 };
            //var json = JsonConvert.SerializeObject(data, jsonSettings);
            //Console.WriteLine(json);
            //data.Service.Do();
            var json = @"{ ""StringValue"": ""Тестовое значение"", ""IntValue"": 5 }";

            var data = JsonConvert.DeserializeObject<IData>(json, jsonSettings);
            Console.WriteLine(data.StringValue);
            Console.WriteLine(data.IntValue);
            data.Service.Do();

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}