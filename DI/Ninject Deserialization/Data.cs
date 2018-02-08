namespace PZone.Samples
{
    class Data : IData
    {
        public string StringValue { get; set; }
        public int IntValue { get; set; }
        public Service Service { get; set; }


        public Data(Service service)
        {
            Service = service;
        }
    }
}