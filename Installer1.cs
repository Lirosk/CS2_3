﻿using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace lab3
{
    [RunInstaller(true)]
    public partial class Installer1 : Installer
    {
        ServiceInstaller serviceInstaller;
        ServiceProcessInstaller processInstaller;

        public Installer1()
        {
            InitializeComponent();
            serviceInstaller = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Manual;
            serviceInstaller.ServiceName = "Service1";
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
