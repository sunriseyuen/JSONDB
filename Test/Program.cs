using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Volte.Data.Dapper;
using Volte.Data.Dapper.Serializer;

namespace ConsoleApp2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var _mConfig = new ConfigurationBuilder();
            var xxx = _mConfig.Build();
            var services = new ServiceCollection()
                .AddTransient<IDbContext, DbContext>()
                .AddTransient<ISerializer, JsonSerializer>()
                .AddTransient<IConnectionProvider, MySql>()
                .AddSingleton<IConfiguration>(xxx);

            var svc = services.BuildServiceProvider();

            // Run startup actions (not needed when registering Elsa with a Host).
            var startupRunner = svc.GetRequiredService<IDbContext>();
            startupRunner.Open("master");
            var docs = await startupRunner.Query<Person>();
            int i = 0;
            foreach (var x in docs)
            {
                i++;
                Console.WriteLine(i+"  ---->  "+x.Id);
            }

            startupRunner.BeginTransaction();

            for (int ndx = 0; ndx < 10; ndx++)
            {
                Person vPerson = new Person();
                int doc=(await startupRunner.AddNewEntity<Person>(vPerson));

                Console.WriteLine("x = "+ doc);
                Console.WriteLine("i = "+ndx);
            }

            startupRunner.Writeable = true;
            startupRunner.Commit();
            Console.WriteLine("OK,press any key to exit");
            Console.ReadLine();
        }
    }
}


