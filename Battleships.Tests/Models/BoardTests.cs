using System.Linq;
using Battleships.Models;
using Battleships.Models.Enums;
using FluentAssertions;
using Xunit;

namespace Battleships.Tests.Models
{
	public class BoardTestsFixture
	{
		internal readonly Board Board;

		public BoardTestsFixture()
		{
			Board.Build();
			Board = Board.Instance;
		}
	}

	public class BoardTests : IClassFixture<BoardTestsFixture>
	{
		private readonly Board board;

		public BoardTests(BoardTestsFixture fixture)
		{
			board = fixture.Board;
		}

		[Fact]
		public void Build_ShouldCreateEnemyShips(){
			board.Ships.Count.Should().Be(3);
			board.Ships.Values.Count(x => x.Type == ShipType.Battleship).Should().Be(1);
			board.Ships.Values.Count(x => x.Type == ShipType.Destroyer).Should().Be(2);
		}

		[Fact]
		public void Build_ShouldPlaceEnemyShipsOnBoard()
		{
			foreach (var ship in board.Ships)
			{
				var shipSettings = ship.Value;
				board.Hits.Values.Count(x => x == ship.Key).Should().Be(shipSettings.Size);
				board.Hits.Where(x => x.Value == ship.Key).Select(x => x.Key).Should().Equal(shipSettings.Hits.Keys);
			}
		}

		[Theory]
		[InlineData("", false)]
		[InlineData("  ", false)]
		[InlineData("a", false)]
		[InlineData("5", false)]
		[InlineData("5a", false)]
		[InlineData("a5", false)]
		[InlineData("A5", true)]
		[InlineData("B10", true)]
		public void IsValidBoardSquare(string input, bool expected)
		{
			board.IsValidBoardSquare(input).Should().Be(expected);
		}
	}
}
