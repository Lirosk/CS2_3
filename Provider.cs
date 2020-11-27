namespace lab3
{
	//as Artsiom said
	class Provider
	{
		private readonly IParser _configParser;

		public T GetConfig<T>(string filePath)
		{
			return _configParser.Parse<T>(filePath);
		}

		public Provider(IParser configParser)
		{
			_configParser = configParser;
		}

		public Provider() { }
	}
}
