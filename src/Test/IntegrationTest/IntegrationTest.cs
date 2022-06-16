using BL;
using ConsoleApp;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest
{
    public class IntegrationTest
    {
        [Fact]
        public async Task GetTemperature_WhenCityIsCorrect_ReturnsTemperature()
        {
            using CancellationTokenSource tokenSource = new CancellationTokenSource();
            //Arrange
            var weatherForecast = DependencyInjection.Resolve<WeatherForecast>();
            //Act
            var temperature = await weatherForecast.GetTemperature("Tbilisi", tokenSource.Token);
            //Assert
            Assert.InRange<double>((double)temperature,- 50, 50);
        }
        [Fact]
        public async Task GetTemperature_WhenCItyIsIncorrect_ReturnsNull()
        {
            using CancellationTokenSource tokenSource = new CancellationTokenSource();
            //Arrange
            var weatherForecast = DependencyInjection.Resolve<WeatherForecast>();
            //Act
            var temperature = await weatherForecast.GetTemperature("TbilisiGeorgia", tokenSource.Token);
            //Assert
            Assert.Null(temperature);
        }
    }
}