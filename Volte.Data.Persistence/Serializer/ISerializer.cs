namespace Volte.Data.Dapper.Serializer
{
    public interface ISerializer
    {
        string Serialize(object obj);
        T Deserialize<T>(string value);
    }
}