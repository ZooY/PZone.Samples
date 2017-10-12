namespace PZone.Samples
{
    public abstract class MyClassBase
    {
        public string Value { get; set; }


        protected MyClassBase()
        {
        }


        protected MyClassBase(string value)
        {
            Value = value;
        }


        public abstract string Do();
    }
}