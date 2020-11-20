using Microsoft.Extensions.Configuration;

namespace Volte.Data.Dapper
{
    public class ProviderProps
    {
        public ProviderProps(IConfiguration conf,string dbName)
        {
            IConfigurationSection data = conf.GetSection(dbName);
            this.ProviderName = data["ProviderName"];
            this.ConnectionString = data["ConnectionString"];

            //ConnectionString = Util.PlaceHolderRegex(conf, ConnectionString);
        }
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }
        public string ClassMapPath { get; set; }
        public string TypeName { get; set; }
    }
}
