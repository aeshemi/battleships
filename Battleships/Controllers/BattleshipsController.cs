using Battleships.Services;
using Microsoft.AspNetCore.Mvc;

namespace Battleships.Controllers
{
	[Route("api/[controller]")]
	public class BattleshipsController : Controller
	{
		private readonly IBattleshipsService battleshipsService;

		public BattleshipsController(IBattleshipsService battleshipsService)
		{
			this.battleshipsService = battleshipsService;
		}

		// Starts new game
		[HttpPost("start")]
		public IActionResult Start()
		{
			battleshipsService.InitializeNewGame();
			return Ok();
		}

		// Fires at a specific square
		[HttpPost("fire/{coordinates}")]
		public IActionResult Fire(string coordinates)
		{
			return new OkObjectResult(battleshipsService.Fire(coordinates));
		}
	}
}
