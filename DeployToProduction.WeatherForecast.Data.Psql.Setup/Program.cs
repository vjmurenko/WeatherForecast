using Microsoft.Extensions.Configuration;

namespace DeployToProduction.WeatherForecast.Data.Psql.Setup
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            var pgsqlConnectionString = configuration.GetConnectionString("Postgres");
            new Db(pgsqlConnectionString).Setup();
        }
    }
}
