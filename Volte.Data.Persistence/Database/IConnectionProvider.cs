using System.Data;
using Volte.Data.Dapper;
using Volte.Data.SqlKata.Compilers;

namespace Volte.Data.Dapper
{
    public interface IConnectionProvider
    {
        string ConnectionString { get; set; }
        IDbConnection Connection { get; set; }
        Compiler Compiler { get; set; }
        void Initialize(string connectionString);
        void Open();
    }
}