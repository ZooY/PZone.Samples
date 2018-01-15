using System.ComponentModel;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;


namespace PZone.Samples
{
    [DesignerCategory("Code")]
    public class SampleService : ServiceBase
    {
        private string[] _args;


        public SampleService()
        {
            ServiceName = "PZone.Sample";
            CanStop = true;
            CanShutdown = true;
            CanPauseAndContinue = true;
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            _args = args;
            EventLog.WriteEntry($"Start. Args: {(args == null ? "null" : string.Join(" ", args))}. _args: {(_args == null ? "null" : string.Join(" ", _args))}");

            Task.Run(() =>
            {
                do
                {
                    EventLog.WriteEntry("Do");
                    Thread.Sleep(3000);
                } while (true);
            });
        }


        protected override void OnStop()
        {
            base.OnStop();

            EventLog.WriteEntry("Stop");
        }


        protected override void OnShutdown()
        {
            base.OnShutdown();

            EventLog.WriteEntry("Shutdown");
        }


        protected override void OnPause()
        {
            base.OnPause();

            EventLog.WriteEntry("Pause");
        }


        protected override void OnContinue()
        {
            base.OnContinue();

            EventLog.WriteEntry($"Continue. _args: {(_args == null ? "null" : string.Join(" ", _args))}");
        }
    }
}