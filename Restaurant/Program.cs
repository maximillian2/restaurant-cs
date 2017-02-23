using System;

namespace Restaurant
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.Clear ();
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var restaurant = new Controller ();
			restaurant.Run ();
		}
	}
}
