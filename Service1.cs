using System.ServiceProcess;
using System.Threading;

namespace lab3
{
	public partial class Service1 : ServiceBase
	{
		internal static Overseer overseer;
		private string configFileName = "config.xml";

		public Service1()
		{			
			InitializeComponent();

			CanStop = true;
			CanPauseAndContinue = true;
			AutoLog = true;
		}

		protected override void OnStart(string[] args)
		{
			var provider = new Provider(new Parser());

			var configPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, configFileName);

			if (!System.IO.File.Exists(configPath))
			{
				throw new System.IO.FileNotFoundException("No config file!");
			}

			Options options = provider.GetConfig<Options>(configPath);
			overseer = new Overseer(options);

			Thread managerThread = new Thread(new ThreadStart(overseer.Start));
			managerThread.Start();
		}

		protected override void OnStop()
		{
			overseer.Stop();
			Thread.Sleep(1000);
		}
	}
}
