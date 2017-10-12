using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace PZone.Samples
{
    class Program
    {
        static void Main()
        {
            // Формируем список элементов ключ-значение, где в качестве ключа выступает тип класса значения.
            var items = new List<KeyValuePair<Type, MyClassBase>>
            {
                new KeyValuePair<Type, MyClassBase>(typeof(MyFirstClass), new MyFirstClass("Base", "First")),
                new KeyValuePair<Type, MyClassBase>(typeof(MySecondClass), new MySecondClass("Base", "Second"))
            };

            // Сериализуем список в строку.
            var str = JsonConvert.SerializeObject(items, Formatting.Indented);
            Console.WriteLine(str);

            // Восстанавливаем список из строки. Типы значений разные и заранее не известны, 
            // поэтому в качестве типа значения используем тип JObject. Используется именно тип 
            // JObject, а не Object, потому что у него есть средства приведения типов.
            var itemsTmp = JsonConvert.DeserializeObject<List<KeyValuePair<Type, JObject>>>(str);

            // Формируем исходный список с нужными типами.
            items = new List<KeyValuePair<Type, MyClassBase>>();
            foreach (var pair in itemsTmp)
            {
                var item = (MyClassBase)pair.Value.ToObject(pair.Key);
                items.Add(new KeyValuePair<Type, MyClassBase>(pair.Key, item));
            }

            // Проверяем, что все типы восстановлены правильно.
            foreach (var pair in items)
                Console.WriteLine($"{pair.Key.FullName} {pair.Value.Do()}");

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
