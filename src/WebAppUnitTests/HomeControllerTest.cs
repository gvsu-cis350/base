using Microsoft.Extensions.Logging;
using sports_betting_app.Controllers;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace WebAppUnitTests
{
    public class HomeControllerTest
    {
        HomeController controller;
        private readonly ILogger<HomeController> _logger;

        public HomeControllerTest()
        {
            controller = new HomeController(_logger);
        }

        // Simple unit test to ensure a string is passed from controller to model
        [Fact]
        public void Index()
        {
            var result = controller.Index() as ViewResult;
            Assert.NotNull(result); 
            var model = result.Model as String;
            Assert.NotNull(model);
            Assert.Equal("this is some test data", model);


        }
    }
}