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
        public async Task TestInstructionsMethod()
        {
            //Arrange
            string expected = "It's time to go to the beach";
            //Act
            //string actual = await WeatherForecast.Instructions(30.5);            
            //Assert
            //Assert.Equal(expected, actual);
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