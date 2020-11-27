using System;

namespace lab3
{
    class Parser : IParser
    {
        private readonly XmlParser _xmlParser;
        private readonly JsonParser _jsonParser;

        public T Parse<T>(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                switch (System.IO.Path.GetExtension(filePath))
                {
                    case (".json"):
                        {
                            return _jsonParser.Parse<T>(filePath);
                        }
                    case (".xml"):
                        {
                            return _xmlParser.Parse<T>(filePath);
                        }
                }

            }
            else
            {
                throw new System.IO.FileNotFoundException();
            }

            throw new Exception("Cannot to parse!");
        }
        public Parser()
        {
            _xmlParser = new XmlParser();
            _jsonParser = new JsonParser();
        }
    }
}