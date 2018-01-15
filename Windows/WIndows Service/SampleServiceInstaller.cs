using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;


namespace PZone.Samples
{
    [RunInstaller(true)]
    [DesignerCategory("Code")]
    public class SampleServiceInstaller : Installer
    {
        /// <summary>
        /// Конструтор класса.
        /// </summary>
        public SampleServiceInstaller()
        {
            Installers.Add(GetServiceInstaller());
            Installers.Add(GetServiceProcessInstaller());
        }


        private ServiceInstaller GetServiceInstaller()
        {
            var installer = new ServiceInstaller
            {
                ServiceName = "PZone.Sample",
                DisplayName = "PZone Sample Service",
                StartType = ServiceStartMode.Automatic
            };
            return installer;
        }


        private static ServiceProcessInstaller GetServiceProcessInstaller()
        {
            var installer = new ServiceProcessInstaller { Account = ServiceAccount.LocalSystem };
            return installer;
        }
    }
}