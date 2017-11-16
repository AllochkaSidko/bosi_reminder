using System.ServiceProcess;
using System.ComponentModel;
using System.Configuration.Install;


namespace BoSi_ReminderService
{
        [RunInstaller(true)]
        public class ProjectInstaller : Installer
        {
            private void InitializeComponent()
            {
                _serviceProcessInstaller = new ServiceProcessInstaller();
                _serviceInstaller = new ServiceInstaller();
                _serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
                _serviceProcessInstaller.Password = null;
                _serviceProcessInstaller.Username = null;
                _serviceInstaller.ServiceName = BoSi_ReminderWindowsService.CurrentServiceName;
                _serviceInstaller.DisplayName = BoSi_ReminderWindowsService.CurrentServiceDisplayName;
                _serviceInstaller.Description = BoSi_ReminderWindowsService.CurrentServiceDescription;
                _serviceInstaller.StartType = ServiceStartMode.Automatic;
                Installers.AddRange(new Installer[]
                {
                _serviceProcessInstaller,
                _serviceInstaller
                });
            }

            public ProjectInstaller()
            {
                InitializeComponent();
            }

            private ServiceProcessInstaller _serviceProcessInstaller;
            private ServiceInstaller _serviceInstaller;
        }
}
