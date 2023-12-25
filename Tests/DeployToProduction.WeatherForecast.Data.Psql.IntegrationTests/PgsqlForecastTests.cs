using DeployToProduction.WeatherForecast.Data.Psql;

[TestClass]
public class PgsqlForecastTest
{
    [TestMethod]
    public async Task PredictAsync_ReturnSameResult_ForSameLocations()
    {
        var containerName = Guid.NewGuid().ToString("D");
        var container = new TestcontainersBuilder<PostgreSqlTestcontainer>()
             .WithName(containerName)
             .WithDatabase(new PostgreSqlTestcontainerConfiguration()
             {
                 Username = "postgres",
                 Password = "postgres",
                 Port = 5432,
                 Database = "weather"
             })
              .Build();
        try
        {
            await container.StartAsync().ConfigureAwait(false);

            new Db(container.ConnectionString).Setup();

            var connectionString = container.ConnectionString
            .Replace("User Id=postgres", "User Id=webapp")
            .Replace("Password=postgres", "Password=webappPASSWORD");

            var forecast = new PgsqlForecast(connectionString);
            var weather1 = await forecast.PredictAsync("Sydney");
            var weather2 = await forecast.PredictAsync("Sydney");

            Assert.AreEqual(weather1.Temperature.Value, weather2.Temperature.Value);
            Assert.AreEqual(weather1.Kind.Code, weather2.Kind.Code);

        }
        finally
        {
            await container.DisposeAsync().ConfigureAwait(false);
        }
    }
}