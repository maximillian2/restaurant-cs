using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Restaurant
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var restaurant = new Controller ();
			restaurant.Run ();
			Console.ReadKey ();
		}
	}
}
