using System.Collections.Generic;

namespace Battleships.Models.Enums
{
	public enum ShipType : short
	{
		Battleship = 1,
		Destroyer = 2
	}

	public static class ShipTypeExtensions
	{
		public static Dictionary<ShipType, int> ShipSizes = new Dictionary<ShipType, int>
		{
			{ ShipType.Battleship, 5 },
			{ ShipType.Destroyer, 4 }
		};

		public static int Size(this ShipType shipType)
		{
			return ShipSizes[shipType];
		}
	}
}
