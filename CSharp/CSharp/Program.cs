using System;


namespace CSharp
{


    class Program
    {
        static void Main(string[] args)
        {
            #region ENUM

            // Parse int to enum

            var intValueStr = "1";
            var success = SomeEnum.TryParse(intValueStr, out SomeEnum someEnumValue);
            Console.WriteLine($"success = {success}, someEnumValue = {(int)someEnumValue} {someEnumValue.ToString()}");


            #endregion


            Console.Read();
        }
    }
}