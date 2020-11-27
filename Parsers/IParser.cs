namespace lab3
{
    interface IParser
    {
        T Parse<T>(string filePath);
    }
}
