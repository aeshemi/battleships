using Battleships.Models;

namespace Battleships.Services
{
	public interface IBattleshipsService
	{
		void InitializeNewGame();

		Result Fire(string coordinates);
	}
}
