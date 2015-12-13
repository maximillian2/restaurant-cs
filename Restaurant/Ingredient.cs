using System;

namespace Restaurant
{
	public class Ingredient
	{
		public string Name { get; set; }
		public double Price { get; set; }

		public Ingredient()
		{
			Name = "";
			Price = 0;
		}

		public Ingredient (string name, double price)
		{
			Name = name;
			Price = price;
		}
			
		public override string ToString ()
		{
			return string.Format ("{0} ({1})", Name, Price);
		}
	}
}	

