using System;

namespace Restaurant
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.Clear ();

			var restaurant = new Controller ();
			restaurant.Run ();
		}
	}
}
