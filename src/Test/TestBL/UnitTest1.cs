using BL;
using System.Threading.Tasks;
using Xunit;

namespace TestBL
{
    public class UnitTest1
    {
        [Fact]
        public async void TestInstructionsMethod()
        {
            //Arrange
            string expected = "It's time to go to the beach";            
            //Act
            Task<string> temperature = WeatherForecast.Instructions(30.5);
            //string actual1 = actual.ToString();
            string actual = await temperature;
            //Assert
            Assert.Equal(expected, actual);
        }
    }
}