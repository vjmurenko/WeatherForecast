using Ductus.FluentDocker.Builders;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace DeployToProduction.WeatherForecast.App.UITests;

[TestClass]
public class WeatherForecastAppTests
{
    [TestMethod]
    public void Generate_RandomWeather()
    {
        var solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName;
        var composeDb = Path.Combine(solutionDirectory, "docker-compose.db.yml");
        var composeDev = Path.Combine(solutionDirectory, "docker-compose.dev.yml");
        using (var svc = new Builder()
                   .UseContainer()
                   .UseCompose()
                   .FromFile(composeDev)
                   .FromFile(composeDb)
                   .RemoveOrphans()
                   .Build()
                   .Start())

        {
            var driver = new EdgeDriver();
            try
            {
                driver.Navigate().GoToUrl("http://localhost:5050/");

                driver.FindElement(By.Id("location")).SendKeys("Moscow");
                driver.FindElement(By.Id("submit_btn")).Click();
                var forecastBlock = driver.FindElement(By.Id("forecast_block"));

                Assert.IsNotNull(forecastBlock);
            }
            finally
            {
                driver.Quit();
                driver.Dispose();
                svc.Remove();
                svc.Stop();
            }
        }
    }
}