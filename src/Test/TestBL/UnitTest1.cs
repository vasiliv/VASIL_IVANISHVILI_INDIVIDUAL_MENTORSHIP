using BL;
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
            string actual = await WeatherForecast.Instructions(30.5);            
            //Assert
            Assert.Equal(expected, actual);
        }
    }
}