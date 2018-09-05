using Battleships.Models;

namespace Battleships.Services
{
	public class BattleshipsService : IBattleshipsService
	{
		public void InitializeNewGame()
		{
			Board.Build();
		}

		public Result Fire(string coordinates)
		{
			var board = Board.Instance;

			var result = new Result();

			if (!board.IsValidBoardSquare(coordinates))
			{
				result.Message = "Invalid square in the board";
				return result;
			}

			if (!board.Hits.ContainsKey(coordinates))
			{
				result.Message = $"Missed at {coordinates}";
				return result;
			}

			var ship = board.Ships[board.Hits[coordinates]];

			if (ship.Hits[coordinates])
			{
				result.Message = $"Ship was already previously hit at {coordinates}";
				return result;
			}

			ship.Hits[coordinates] = true;

			result.IsSuccess = true;

			result.AllShipsSunken = board.AllShipsSunken;

			result.Message = ship.IsSunken ? "Ship was sunk" : $"Ship was hit at {coordinates}";

			return result;
		}
	}
}
