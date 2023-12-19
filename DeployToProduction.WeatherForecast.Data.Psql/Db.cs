using System.Data;
using System.Reflection;

namespace DeployToProduction.WeatherForecast.Data.Psql
{
    public class Db
    {
        private readonly string _connectionString;

        public Db(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool Setup()
        {
            var upgrader = DbUp.DeployChanges.To
                .PostgresqlDatabase(_connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            WaitDatabaseConnectionReady();

            var result = upgrader.PerformUpgrade();

            return result.Successful;
        }

        private void WaitDatabaseConnectionReady()
        {
            do
            {
                using var connection = new Npgsql.NpgsqlConnection(_connectionString);
                try
                {
                    connection.Open();

                    if (connection.State == ConnectionState.Open)
                    {
                        break;
                    }
                    else
                    {
                        Thread.Sleep(500);
                    }
                }
                catch
                {
                    Thread.Sleep(500);
                }

            } while (true);
        }
    }
}
