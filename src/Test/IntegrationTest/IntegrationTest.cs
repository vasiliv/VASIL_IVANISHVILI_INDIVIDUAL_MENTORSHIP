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
            //Arrange
            var weatherForecast = DependencyInjection.Resolve<WeatherForecast>();
            //Act
            var temperature = await weatherForecast.GetTemperature("Tbilisi");
            //Assert
            Assert.InRange<double>((double)temperature,- 50, 50);
        }
        [Fact]
        public async Task GetTemperature_WhenCItyIsIncorrect_ReturnsNull()
        {            
            //Arrange
            var weatherForecast = DependencyInjection.Resolve<WeatherForecast>();
            //Act
            var temperature = await weatherForecast.GetTemperature("TbilisiGeorgia");
            //Assert
            Assert.Null(temperature);
        }
    }
}