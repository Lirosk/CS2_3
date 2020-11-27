using System.IO;
using System.Threading;

namespace lab3
{
    class Overseer
	{
        private bool enabled = true;
        private Commands slave;// does all the work
        private string sourceDirectoryPath;
		private string logPath;
		private object obj = new object();

        internal FileSystemWatcher watcher;

        public Overseer(Options options)
        {
            sourceDirectoryPath = options.SourceDirectoryPath;
			logPath = options.LogPath;

            slave = new Commands(options);

            watcher = new FileSystemWatcher(sourceDirectoryPath);
            
            watcher.Deleted += Watcher_Deleted;
            watcher.Created += Watcher_Created;
            watcher.Created += slave.OnAdded;
            watcher.Changed += Watcher_Changed;
            watcher.Renamed += Watcher_Renamed;
        }

        public void Start()
        {
            watcher.EnableRaisingEvents = true;
            while (enabled)
            {
                Thread.Sleep(1000);
            }
        }

        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
            enabled = false;
        }

		private void Watcher_Renamed(object sender, RenamedEventArgs e)
		{
			string fileEvent = "renamed to " + e.FullPath;
			string filePath = e.OldFullPath;
			RecordEntry(fileEvent, filePath);
		}

		private void Watcher_Changed(object sender, FileSystemEventArgs e)
		{
			string fileEvent = "changed";
			string filePath = e.FullPath;
			RecordEntry(fileEvent, filePath);
		}


		private void Watcher_Created(object sender, FileSystemEventArgs e)
		{
			string fileEvent = "created";
			string filePath = e.FullPath;
			RecordEntry(fileEvent, filePath);
		}


		private void Watcher_Deleted(object sender, FileSystemEventArgs e)
		{
			string fileEvent = "deleted";
			string filePath = e.FullPath;
			RecordEntry(fileEvent, filePath);
		}

		private void RecordEntry(string fileEvent, string filePath)
		{
			lock (obj)
			{
				using (StreamWriter writer = new StreamWriter(logPath, true))
				{
					writer.WriteLine(System.String.Format("{0} file {1} was {2}\n",
						System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), filePath, fileEvent));
					writer.Flush();
				}
			}
		}
	}
}
