using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volte.Data.Dapper.Serializer;
using Volte.Data.SqlKata;

namespace Volte.Data.Dapper
{

    public class DbContext : IDbContext, IDisposable {
        private IConnectionProvider _connectionProvider;
        private readonly IServiceProvider _serviceProvider;
        private IConfiguration config;
        private ISerializer _serializer;
        private IDbConnection _Connection;
        private IDbTransaction _Transaction;
        const string JSON_FIELD_NAME= "Contents";

        public DbContext(IServiceProvider serviceProvider, ISerializer serializer, IConfiguration config)
        {
            this.config = config;
            _serializer = serializer;
            _serviceProvider = serviceProvider;
        }

        public void Open(string dbName)
        {
            string ConnectionString = "Database='eai-pro';Port=3306;Data Source=127.0.0.1;User ID=root;Password=123456;CharSet=utf8;SslMode=None;Convert Zero Datetime=True;";

            _connectionProvider = _serviceProvider.GetRequiredService<IConnectionProvider>();

            _connectionProvider.Initialize(ConnectionString);
            _connectionProvider.Open();
            this.DbName = dbName;
        }

        public bool IsOpen
        {
            get {
                if (_Connection != null && _Connection != null && _Connection.State == ConnectionState.Open) {
                    return true;
                } else {
                    return false;
                }
            }
        }
        public void Close()
        {
            if (_Connection == null)
            {
                return;
            }
            this.Dispose();
        }
        public async Task<int> AddNewEntity<T>(IDataObject entity) where T : class, new()
        {
            ObjectProperty _ObjectProperty = ObjectPropertyMaps.Build<T>();
            Query query = new Query(_ObjectProperty.TableName);

            var defaultValues = new Dictionary<string, object>();
            foreach (var item in _ObjectProperty.Property)
            {
                if (item.Indexes)
                {
                    var otherProp = typeof(T).GetProperty(item.ColumnName);
                    var value = otherProp.GetValue(entity, null);
                    defaultValues.Add(item.ColumnName, value);
                }
            }
            Serializer.JsonSerializer serializer = new Serializer.JsonSerializer();

            defaultValues[JSON_FIELD_NAME] = serializer.Serialize(entity);

            SqlResult sqlResult = _connectionProvider.Compiler.Compile(query.AsInsert(defaultValues));

            Console.WriteLine(sqlResult.Sql.ToString());

            var result = (await _connectionProvider.Connection.ExecuteAsync(sqlResult.Sql,
              sqlResult.NamedBindings,
              _Transaction,
              this.CommandTimeout));
            
            return result;
        }
        public async Task<bool> DeleteEntity<T>(IDataObject entity) where T : class, new()
        {
            ObjectProperty _ObjectProperty = ObjectPropertyMaps.Build<T>();
            Query query = new Query(_ObjectProperty.TableName);

            foreach (var item in _ObjectProperty.Property)
            {
                if (item.PrimaryKey)
                {
                    var otherProp = typeof(T).GetProperty(item.ColumnName);
                    var value = otherProp.GetValue(entity, null);
                    query.Where(item.ColumnName, value);
                }
            }

            SqlResult sqlResult = _connectionProvider.Compiler.Compile(query.AsDelete());
            var result = (await _connectionProvider.Connection.ExecuteAsync(sqlResult.Sql,
              sqlResult.NamedBindings,
              _Transaction,
              this.CommandTimeout));
            return result > 0;
        }

        public async Task<bool> UpdateEntity<T>(IDataObject entity) where T : class, new()
        {
            ObjectProperty _ObjectProperty = ObjectPropertyMaps.Build<T>();
            Query query = new Query(_ObjectProperty.TableName);

            var defaultValues = new Dictionary<string, object>();

            foreach (var item in _ObjectProperty.Property)
            {
                if (item.Indexes || item.PrimaryKey)
                {
                    var otherProp = typeof(T).GetProperty(item.ColumnName);
                    var value = otherProp.GetValue(entity, null);
                    if (item.Indexes)
                    {
                        defaultValues.Add(item.ColumnName, value);
                    }
                    if (item.PrimaryKey)
                    {
                        query.Where(item.ColumnName, value);
                    }
                }
                if (item.ColumnName == JSON_FIELD_NAME)
                {
                    Serializer.JsonSerializer serializer = new Serializer.JsonSerializer();

                    defaultValues[JSON_FIELD_NAME] = serializer.Serialize(entity);
                }
            }
            SqlResult sqlResult = _connectionProvider.Compiler.Compile(query.AsUpdate(defaultValues));
            var result = (await _connectionProvider.Connection.ExecuteAsync(sqlResult.Sql,
              sqlResult.NamedBindings,
              _Transaction,
              this.CommandTimeout));
            return result > 0;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<IEnumerable<T>> Query<T>() where T : class
        {
            ObjectProperty _ObjectProperty = ObjectPropertyMaps.Build<T>();
            Query query = new Query(_ObjectProperty.TableName);

            SqlResult sqlResult = _connectionProvider.Compiler.Compile(query);

            var documents = (await _connectionProvider.Connection.QueryAsync<DataObject>(sqlResult.Sql,
                sqlResult.NamedBindings,
                _Transaction,
                this.CommandTimeout));

            return documents.Select(d => _serializer.Deserialize<T>(d.Contents));
        }
        /// <summary>
        /// 获取默认一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> SingleOrDefault<T>() where T : class
        {
            ObjectProperty _ObjectProperty = ObjectPropertyMaps.Build<T>();
            Query query = new Query(_ObjectProperty.TableName);

            SqlResult sqlResult = _connectionProvider.Compiler.Compile(query);

            var result = (await _connectionProvider.Connection.QueryAsync<DataObject>(sqlResult.Sql,
                sqlResult.NamedBindings,
                _Transaction,
                this.CommandTimeout)).FirstOrDefault();

            return  _serializer.Deserialize<T>(result.Contents);
        }
        public void Dispose()
        {
            if (_Connection != null)
            {
                try
                {
                    _Connection.Close();
                    _Connection.Dispose();
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
            }
        }
        public void BeginTransaction()
        {

            _Transaction = _connectionProvider.Connection.BeginTransaction();

        }

        public void Commit()
        {
            try {
                _Transaction.Commit();
            } catch (Exception exception1) {
                _Transaction.Rollback();
                throw exception1;
            }
        }

        public void RollBack()
        {

            _Transaction.Rollback();
        }

        // Properties
        public string DbName { get; set; }
        public bool IsForceCommit  { get; set; } = true;
        public bool Writeable      { get; set; } = false;
        public int  CommandTimeout { get; set; } = 120;
    }
}
