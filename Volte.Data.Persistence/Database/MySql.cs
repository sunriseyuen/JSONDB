using System;
using System.Data;
using MySqlConnector;
using Volte.Data.SqlKata.Compilers;
//using Volte.Data.Json;
//using Volte.Data.VolteDi;

namespace Volte.Data.Dapper
{
    public class MySql : IConnectionProvider
    {
        public string ConnectionString { get; set; }
        public IDbConnection Connection { get; set; }
        public Compiler Compiler { get; set; }

        public void Initialize(string connectionString)
        {
            this.Compiler = new MySqlCompiler();
            this.ConnectionString = connectionString;
            this.Connection= new MySqlConnection(this.ConnectionString);
        }
        public void Open()
        {
            try
            {
                if (this.Connection.State == ConnectionState.Closed)
                {
                    this.Connection.Open();
                }
            }
            catch (Exception exception1)
            {
                Console.Write(exception1);
                this.TryOpen();
            }
        }
        private void TryOpen()
        {
            string _OcnnString = this.ConnectionString;

            if (this.Connection.State != ConnectionState.Closed)
            {
                this.Connection.Close();
            }

            this.Connection.ConnectionString = _OcnnString + ";Pooling=false;";
            this.Connection.Open();
        }
    }
}
