using System;
using Battleships.Controllers;
using Battleships.Models;
using Battleships.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace Battleships.Tests.Controllers
{
	public class BattleshipsControllerTestsFixture
	{
		internal readonly BattleshipsController BattleshipsController;
		internal readonly Mock<IBattleshipsService> MockBattleshipsService;

		public BattleshipsControllerTestsFixture()
		{
			MockBattleshipsService = new Mock<IBattleshipsService>();
			BattleshipsController = new BattleshipsController(MockBattleshipsService.Object);
		}
	}

	public class BattleshipsControllerTests : IClassFixture<BattleshipsControllerTestsFixture>, IDisposable
	{
		private readonly BattleshipsController battleshipsController;
		private readonly Mock<IBattleshipsService> mockBattleshipsService;

		public BattleshipsControllerTests(BattleshipsControllerTestsFixture fixture)
		{
			mockBattleshipsService = fixture.MockBattleshipsService;
			battleshipsController = fixture.BattleshipsController;
		}

		public void Dispose()
		{
			mockBattleshipsService.Reset();
		}

		[Fact]
		public void Start_ShouldInitializeNewGame()
		{
			battleshipsController.Start()
				.AssertOkResult();

			mockBattleshipsService.Verify(x => x.InitializeNewGame(), Times.Once);
		}

		[Fact]
		public void Fire_ShouldHitGivenCoordinatesAndReturnResult()
		{
			mockBattleshipsService.Setup(x => x.Fire(It.IsAny<string>())).Returns(new Result
			{
				IsSuccess = true,
				Message = "Ship was sunk"
			});

			var result = battleshipsController.Fire("A5")
				.AssertOkObjectResult().Model<Result>();

			mockBattleshipsService.Verify(x => x.Fire("A5"), Times.Once);

			result.Should().NotBeNull();
			result.IsSuccess.Should().BeTrue();
			result.AllShipsSunken.Should().BeFalse();
			result.Message.Should().Be("Ship was sunk");
		}
	}
}
