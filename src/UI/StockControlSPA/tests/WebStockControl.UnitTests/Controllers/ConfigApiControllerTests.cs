using WebStockControl.API.Controllers;
using WebStockControl.API.Infrastructure.Settings;

namespace WebStockControl.UnitTests.Controllers;

public class ConfigApiControllerTests
{
	[Fact]
	public void Get_should_return_urls_for_using_spa()
	{
		// Arrange
		// Подготавливаем данные для теста

		var appSetting = new AppSettings()
		{
			BffUrl = "bffUrl",
			IdentityUrl = "identityUrl"
		};

		var logger = new Mock<ILogger<ConfigApiController>>();

		var options = new Mock<IOptionsSnapshot<AppSettings>>();
		options.Setup(o => o.Value).Returns(appSetting);

		// Act
		// Создаём тестируемый объект и вызываем действие, получаем результат

		var controller = new ConfigApiController(options.Object, logger.Object);
		var actionResult = controller.Get();

		// Assert
		// Проверяем результат

		var result = Assert.IsType<OkObjectResult>(actionResult).Value as AppSettings;

		Assert.NotNull(result);
		Assert.Equal(appSetting.GetType().GetProperties().Length, result.GetType().GetProperties().Length);
		Assert.Equal(appSetting.BffUrl, result.BffUrl);
		Assert.Equal(appSetting.IdentityUrl, result.IdentityUrl);
	}
}