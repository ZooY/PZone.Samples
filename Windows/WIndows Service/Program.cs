using System.ServiceProcess;


namespace PZone.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBase.Run(new SampleService());
        }
    }
}
