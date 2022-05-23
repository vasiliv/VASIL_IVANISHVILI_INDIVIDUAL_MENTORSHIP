using BL;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace TestBL
{
    public class UnitTest1
    {        
        [Fact]
        public async Task GetTemperature_WhenCityIsCorrect_ReturnsTemperature()
        {
            //Arrange
            WeatherForecast weatherForecast = new WeatherForecast(FakeHttpClient("{'main':{'temp':-5.5}}"));
            //Act
            var temperature = await weatherForecast.GetTemperature(It.IsAny<string>());
            //Assert
            Assert.Equal(temperature, -5.5);
        }
        [Fact]
        public async Task GetInstructions_WhenTemperatureIsCorrect_ReturnsTemperature()
        {
            //Arrange
            WeatherForecast weatherForecast = new WeatherForecast(FakeHttpClient("{'main':{'temp':-5.5}}"));
            string instruction = await weatherForecast.Instructions(-5.5);
            //Act
            string actual = "Dress warmly";
            //Assert
            Assert.Equal(instruction,actual);
        }
        public static HttpClient FakeHttpClient(string response)
        {
            response ??= "{'main':{'temp':10.0}}";
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(response),
               });
            return new HttpClient(handlerMock.Object);
        }
    }    
}