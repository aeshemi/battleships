using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Battleships.Models.Enums;

namespace Battleships.Models
{
	public class Board
	{
		private const int BoardSize = 10;

		private static Board instance;

		private static readonly Random Random = new Random();

		public Dictionary<int, Ship> Ships { get; private set; }

		public Dictionary<string, int> Hits { get; set; }

		public bool AllShipsSunken
		{
			get { return Ships.Values.All(x => x.IsSunken); }
		}

		public static readonly int BoardBounds = BoardSize + 1;

		public static readonly char[] BoardLetters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };

		public static Board Instance => instance ?? (instance = Build());

		public static Board Build()
		{
			instance = new Board
			{
				Ships = new Dictionary<int, Ship>
				{
					{ 1, new Ship(1, ShipType.Battleship) },
					{ 2, new Ship(2, ShipType.Destroyer) },
					{ 3, new Ship(3, ShipType.Destroyer) }
				},
				Hits = new Dictionary<string, int>()
			};

			PositionShips(instance.Ships.Values);

			return instance;
		}

		public bool IsValidBoardSquare(string coordinates)
		{
			return Regex.IsMatch(coordinates, "^([A-z][0-9]+)$") && BoardLetters.Contains(coordinates[0]) && int.Parse(coordinates.Substring(1)) <= BoardSize;
		}

		private static void PositionShips(IEnumerable<Ship> ships)
		{
			foreach (var ship in ships)
			{
				List<string> positions;

				do
				{
					positions = GetPositions(ship.Size);
				} while (positions.Any(instance.Hits.Keys.Contains)); // has overlap with other ships

				ship.Hits = positions.ToDictionary(x => x, x => false);
				positions.ForEach(x => instance.Hits.Add(x, ship.Id));
			}
		}

		private static List<string> GetPositions(int shipSize)
		{
			var positions = new List<string>();
			var isHorizontal = Random.Next(0, 2) == 0;

			var fixedCoordinate = Random.Next(1, BoardBounds);
			var movingCoordinate = Random.Next(1, BoardBounds - shipSize);

			for (var i = 0; i < shipSize; i++)
				positions.Add(isHorizontal
					? $"{BoardLetters[fixedCoordinate - 1]}{movingCoordinate + i}"
					: $"{BoardLetters[movingCoordinate + i - 1]}{fixedCoordinate}");

			return positions;
		}
	}
}
