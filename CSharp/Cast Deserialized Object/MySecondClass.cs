namespace PZone.Samples
{
    public class MySecondClass : MyClassBase
    {
        public string SecondValue { get; set; }


        public MySecondClass(string value, string secondValue) : base(value)
        {
            SecondValue = secondValue;
        }


        public override string Do()
        {
            return SecondValue;
        }
    }
}