using System;
using System.Collections.Generic;
using System.Linq;
using Battleships.Models;
using Battleships.Services;
using FluentAssertions;
using Xunit;

namespace Battleships.Tests.Services
{
	public class BattleshipsServiceTestsFixture
	{
		internal readonly BattleshipsService BattleshipsService;
		internal readonly Random Random;

		public BattleshipsServiceTestsFixture()
		{
			BattleshipsService = new BattleshipsService();
			Random = new Random();
		}
	}

	public class BattleshipsServiceTests : IClassFixture<BattleshipsServiceTestsFixture>
	{
		private readonly BattleshipsService battleshipsService;
		private readonly Random random;
		private Board board;
		
		public BattleshipsServiceTests(BattleshipsServiceTestsFixture fixture)
		{
			battleshipsService = fixture.BattleshipsService;
			battleshipsService.InitializeNewGame();
			board = Board.Instance;
			random = fixture.Random;
		}

		[Fact]
		public void InitializeNewGame_ClearsPreviousGame()
		{
			foreach (var ship in board.Ships.Values)
			{
				var coordinates = new List<string>(ship.Hits.Keys);

				foreach (var coordinate in coordinates)
					ship.Hits[coordinate] = true;
			}

			board.AllShipsSunken.Should().BeTrue();

			battleshipsService.InitializeNewGame();

			board = Board.Instance;

			foreach (var ship in board.Ships.Values)
				ship.Hits.All(x => x.Value == false).Should().BeTrue();
		}

		[Theory]
		[InlineData("")]
		[InlineData("  ")]
		[InlineData("a")]
		[InlineData("5")]
		[InlineData("5a")]
		[InlineData("a5")]
		public void Fire_InvalidSquareOnBoard(string coordinates)
		{
			var result = battleshipsService.Fire(coordinates);

			result.IsSuccess.Should().BeFalse();
			result.AllShipsSunken.Should().BeFalse();
			result.Message.Should().Be("Invalid square in the board");
		}

		[Fact]
		public void Fire_MissedCoordinate()
		{
			for (var i = 0; i < 5; i++)
			{
				string coordinates;

				do
				{
					coordinates = $"{Board.BoardLetters[random.Next(1, Board.BoardBounds) - 1]}{random.Next(1, Board.BoardBounds)}";
				} while (board.Hits.ContainsKey(coordinates));

				var result = battleshipsService.Fire(coordinates);

				result.IsSuccess.Should().BeFalse();
				result.AllShipsSunken.Should().BeFalse();
				result.Message.Should().Be($"Missed at {coordinates}");
			}
		}

		[Fact]
		public void Fire_PreviouslyHitCoordinate()
		{
			for (var i = 0; i < 5; i++)
			{
				var coordinates = board.Hits.Keys.ElementAt(random.Next(0, board.Hits.Count));

				battleshipsService.Fire(coordinates);

				var result = battleshipsService.Fire(coordinates);

				result.IsSuccess.Should().BeFalse();
				result.AllShipsSunken.Should().BeFalse();
				result.Message.Should().Be($"Ship was already previously hit at {coordinates}");
			}
		}

		[Fact]
		public void Fire_HitSuccess()
		{
			var hits = new List<string>();

			for (var i = 0; i < 5; i++)
			{
				string coordinates;

				do
				{
					coordinates = board.Hits.Keys.ElementAt(random.Next(0, board.Hits.Count));
				} while (hits.Contains(coordinates));

				hits.Add(coordinates);

				var result = battleshipsService.Fire(coordinates);

				result.IsSuccess.Should().BeTrue();
				result.AllShipsSunken.Should().BeFalse();
				result.Message.Should().Be($"Ship was hit at {coordinates}");
			}
		}

		[Fact]
		public void Fire_ShipSunk()
		{
			var coordinates = new List<string>();

			foreach (var ship in board.Ships.Values)
			{
				var keys = new List<string>(ship.Hits.Keys);

				var coordinate = keys.ElementAt(random.Next(0, keys.Count));

				coordinates.Add(coordinate);

				foreach (var key in keys)
					if (key != coordinate) ship.Hits[key] = true;
			}

			for (var i = 0; i < coordinates.Count; i++)
			{
				var result = battleshipsService.Fire(coordinates[i]);

				result.IsSuccess.Should().BeTrue();
				result.AllShipsSunken.Should().Be(i == coordinates.Count - 1);
				result.Message.Should().Be("Ship was sunk");
			}
		}
	}
}
