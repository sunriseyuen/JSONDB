using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volte.Data.SqlKata;
using Volte.Data.SqlKata.Compilers;

namespace Volte.Data.Dapper
{
    public abstract class XQueryBuilder
    {
        internal ObjectProperty _ObjectProperty;
        internal Query query;
        internal IDbContext DbContext { get; set; }
        private int CommandTimeout { get; set; }
        
        private static readonly MySqlCompiler Compiler = new MySqlCompiler();

        public Query XQuery()
        {
            return query;
        }

        ///// <summary>
        ///// 修改
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="dbs"></param>
        ///// <param name="sql">按条件修改</param>
        ///// <returns></returns>
        //public async Task<bool> AddNewEntity<T>(IDataObject entity)
        //{
        //    var defaultValues = new Dictionary<string, object>();
        //    ObjectProperty _ObjectProperty = ObjectPropertyMaps.Build<T>();
        //    foreach (var item in _ObjectProperty.Property)
        //    {
        //        if (item.Indexes)
        //        {
        //            var otherProp = typeof(T).GetProperty(item.ColumnName);
        //            var value = otherProp.GetValue(entity, null);
        //            defaultValues.Add(item.ColumnName, value);
        //        }
        //    }
        //    Serializer.JsonSerializer serializer = new Serializer.JsonSerializer();

        //    defaultValues["sKey"] = entity.sKey;
        //    defaultValues["sCorporation"] = entity.sCorporation;
        //    defaultValues["Contents"] = serializer.Serialize(entity);

        //    SqlResult sqlResult = Compiler.Compile(query.AsInsert(defaultValues));

        //    NLogger.Debug(sqlResult.Sql);

        //    try
        //    {
        //        var result = DbContext.DbConnection.ExecuteAsync(sqlResult.Sql,
        //          sqlResult.NamedBindings,
        //          DbContext.DbTransaction,
        //          this.CommandTimeout);

        //    }
        //    catch (Exception e)
        //    {
        //        NLogger.Debug(e);
        //    }

        //    return true;
        //}
        ///// <summary>
        ///// 修改
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="dbs"></param>
        ///// <param name="sql">按条件修改</param>
        ///// <returns></returns>
        //public async Task<bool> UpdateEntity<T>(T entity) where T : class, new()
        //{

        //    var defaultValues = new Dictionary<string, object>();

        //    bool HashContents = false;
        //    ObjectProperty _ObjectProperty = ObjectPropertyMaps.Build<T>();
        //    foreach (var item in _ObjectProperty.Property)
        //    {
        //        if (item.Indexes)
        //        {
        //            var otherProp = typeof(T).GetProperty(item.ColumnName);
        //            var value = otherProp.GetValue(entity, null);
        //            defaultValues.Add(item.ColumnName, value);
        //        }
        //        if (item.ColumnName == "Contents")
        //        {
        //            HashContents = true;
        //        }
        //    }
        //    if (HashContents)
        //    {
        //        Volte.Data.Dapper.Serializer.JsonSerializer serializer = new Volte.Data.Dapper.Serializer.JsonSerializer();

        //        defaultValues["Contents"] = serializer.Serialize(entity);
        //    }
        //    SqlResult sqlResult = Compiler.Compile(query.AsUpdate(defaultValues));
        //    var result = (await DbContext.DbConnection.ExecuteAsync(sqlResult.Sql,
        //      sqlResult.NamedBindings,
        //      DbContext.DbTransaction,
        //      this.CommandTimeout));
        //    return result > 0;
        //}

        ///// <summary>
        ///// 查询列表
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <returns></returns>
        //public async Task<IEnumerable<T>> Query<T>() where T : class
        //{
        //    SqlResult sqlResult = Compiler.Compile(query);

        //    var result = (await DbContext.DbConnection.QueryAsync<T>(sqlResult.Sql,
        //        sqlResult.NamedBindings,
        //        DbContext.DbTransaction,
        //        this.CommandTimeout));
        //    return result;
        //}
        ///// <summary>
        ///// 获取默认一条数据
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <returns></returns>
        //public async Task<T> SingleOrDefault<T>() where T : class
        //{
        //    SqlResult sqlResult = Compiler.Compile(query);

        //    var result = (await DbContext.DbConnection.QueryAsync<T>(sqlResult.Sql,
        //        sqlResult.NamedBindings,
        //        DbContext.DbTransaction,
        //        this.CommandTimeout)).FirstOrDefault();
        //    return result;
        //}
    }
    public class XQueryBuilder<T> : XQueryBuilder where T : class
    {
        public XQueryBuilder(IDbContext _DbContext) : base()
        {
            DbContext = _DbContext;
            _ObjectProperty = ObjectPropertyMaps.Build<T>();
            query = new Query(_ObjectProperty.TableName);
        }
    }
}
