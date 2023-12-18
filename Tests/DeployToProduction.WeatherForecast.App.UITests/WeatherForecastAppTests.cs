using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.Diagnostics;

namespace DeployToProduction.WeatherForecast.App.UITests
{
    [TestClass]
    public class WeatherForecastAppTests
    {
        [TestMethod]
        public async Task Generate_RandomWeather()
        {
            await DockerComposeHelper.StartComposeStack();

            //var container = new ContainerBuilder()
            //    .WithImage("weather-forecast-img")
            //    .WithNetwork("weather-forecast-net")
            //    .WithPortBinding(5055, 80)
            //    .Build();

            //await container.StartAsync().ConfigureAwait(false);

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
                //await container.DisposeAsync().ConfigureAwait(false);
                await DockerComposeHelper.StopComposeStack();

            }
        }
    }


    public class DockerComposeHelper
    {
        public static async Task StartComposeStack()
        {
            await ExecuteDockerComposeCommand("up -d");
        }

        public static async Task StopComposeStack()
        {
            await ExecuteDockerComposeCommand("down");
        }

        private static async Task ExecuteDockerComposeCommand(string arguments)
        {
            var path = Path.GetFullPath(@"../../../");
            await Console.Out.WriteLineAsync(path);

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "docker-compose",
                Arguments = arguments,
                WorkingDirectory = @"../../../",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                await process.WaitForExitAsync();

                Thread.Sleep(1000);
                // Handle the output and error as needed
                Console.WriteLine(output);
                Console.WriteLine(error);
            }
        }
    }
}