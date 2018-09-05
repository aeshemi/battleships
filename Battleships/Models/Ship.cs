using System.Collections.Generic;
using System.Linq;
using Battleships.Models.Enums;

namespace Battleships.Models
{
	public class Ship
	{
		public Ship(int id, ShipType type)
		{
			Id = id;
			Type = type;
			Size = type.Size();
		}

		public int Id { get; set; }

		public ShipType Type { get; set; }

		public int Size { get; set; }

		public Dictionary<string, bool> Hits { get; set; }

		public bool IsSunken
		{
			get { return Hits.Values.All(x => x); }
		}
	}
}
