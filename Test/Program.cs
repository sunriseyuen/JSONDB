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
            //startupRunner.BeginTransaction();

            for (int i = 0; i < 10; i++)
            {
                Person vPerson = new Person();
                startupRunner.AddNewEntity<Person>(vPerson);
            }

            //startupRunner.Writeable = true;
            //startupRunner.Commit();
            Console.WriteLine("OK,press any key to exit");
            Console.ReadLine();
        }
    }
}


