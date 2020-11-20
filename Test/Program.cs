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


            string sConfigurationFile = "d:\\test.json";
            Console.Write($"Load external configuration file: {sConfigurationFile}");
            var _mConfig = new ConfigurationBuilder();
            _mConfig.AddJsonFile(sConfigurationFile);
            var xxx = _mConfig.Build();

            // Create a service container with Elsa services.
            var services = new ServiceCollection()
                .AddTransient<IDbContext, DbContext>()
                .AddTransient<ISerializer, JsonSerializer>()
                .AddTransient<IConnectionProvider, MySql>()
               .AddSingleton<IConfiguration>(xxx);


            List<string> aDir = new List<string>();
            string ZERO_ADDONS_DIR = string.Empty;

         //   NLogger.Info("Plugin Location(ZERO_ADDONS_DIR) = [" + ZERO_ADDONS_DIR + "]");

            if (!string.IsNullOrEmpty(ZERO_ADDONS_DIR))
            {
                ZERO_ADDONS_DIR = ZERO_ADDONS_DIR + ";";

                foreach (string dd2 in ZERO_ADDONS_DIR.Split(new char[1] { ';' }))
                {
                    if (!string.IsNullOrEmpty(dd2) && Directory.Exists(dd2))
                    {
                        aDir.Add(dd2);
                    }
                }
            }

            string sAddonsDirectory = Directory.GetParent(Directory.GetCurrentDirectory()) + Path.DirectorySeparatorChar.ToString() + "addons";
            //NLogger.Info("AddonsDirectory = " + sAddonsDirectory);

            if (Directory.Exists(sAddonsDirectory) && aDir.IndexOf(sAddonsDirectory) < 0)
            {
                aDir.Add(sAddonsDirectory);
            }

            string sBinDirectory = Directory.GetCurrentDirectory();
            if (Directory.Exists(sBinDirectory))
            {
                aDir.Add(sBinDirectory);
            }

            StringBuilder ScanClass = new StringBuilder();
            ScanClass.AppendLine();
            //services.Scan(scan =>
            //        scan.FromAssemblies(_Assembly)
            //        .AddClasses(classes =>
            //            classes.Where(t => {
            //                ScanClass.AppendLine(t.Name);
            //                return true;
            //            })
            //            )
            //        .AsSelf()
            //        .AsImplementedInterfaces()
            //        .WithTransientLifetime());

            Console.WriteLine(ScanClass.ToString());

            var svc = services.BuildServiceProvider();

            // Run startup actions (not needed when registering Elsa with a Host).
            var startupRunner = svc.GetRequiredService<IDbContext>();
            startupRunner.Open("master");
            startupRunner.BeginTransaction();

            for (int i = 0; i < 10; i++)
            {
                Person vPerson = new Person();
                startupRunner.AddNewEntity<Person>(vPerson);
            }

            startupRunner.Writeable = true;
            startupRunner.Commit();

            Console.ReadLine();
        }
    }
}


