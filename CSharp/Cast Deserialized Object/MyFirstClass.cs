namespace PZone.Samples
{
    public class MyFirstClass : MyClassBase
    {
        public string FirstValue { get; set; }


        public MyFirstClass(string value, string firstValue) : base(value)
        {
            FirstValue = firstValue;
        }


        public override string Do()
        {
            return FirstValue;
        }
    }
}